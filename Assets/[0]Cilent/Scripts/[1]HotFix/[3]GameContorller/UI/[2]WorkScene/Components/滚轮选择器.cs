using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QFramework;
using System;
using UnityEngine.EventSystems;


/// <summary>
/// 滚轮选择器 - 类似iOS日期选择器的滚轮效果，滚动即确认
/// </summary>
public class 滚轮选择器 : MonoBehaviour, IController
{
    [Header("UI组件")]
    [SerializeField] ScrollRect 滚轮ScrollRect;
    [SerializeField] Transform 内容容器;
    [SerializeField] GameObject 项目预制体; // 统一的项目预制体
    [SerializeField] RectTransform 选中区域; // 中间选中区域的高亮框
    
    [Header("滚轮配置")]
    [SerializeField] int 可见项目数量 = 5; // 在编辑器中设置，奇数效果最好
    [SerializeField] float 项目间距 = 5f;
    [SerializeField] float 滚动灵敏度 = 1f;
    
    [Header("自动确认设置")]
    [SerializeField] float 自动确认延迟 = 0.5f; // 停止滚动后多久自动确认
    [SerializeField] bool 启用自动确认 = true; // 是否启用自动确认
    
    [Header("视觉效果")]
    [SerializeField] Color 选中颜色 = Color.cyan;
    [SerializeField] Color 未选中颜色 = Color.white;
    [SerializeField] float 未选中透明度 = 0.5f;
    [SerializeField] float 缩放效果 = 0.8f;
    
    [Header("虚拟化")]
    [Tooltip("是否开启循环（无限）滚动")]
    [SerializeField] bool 启用循环滚动 = false;
    [Tooltip("为了平滑滚动而额外创建的预备项目数量，一般为2-4个")]
    [SerializeField] int 项目池大小缓冲 = 4;
    
    [Header("数据")]
    [SerializeField] List<string> 选项列表 = new List<string>();
    
    // 私有变量
    private List<GameObject> 项目池 = new List<GameObject>();
    private List<string> _displayList; // 用于虚拟化和循环滚动显示的列表
    private int _padding = 0; // 循环滚动时前后的填充数量
    private float 项目高度; // 再次引入，但由代码自动计算
    private int 当前选中索引 = 0; // 重要：此索引现在是相对于原始 `选项列表` 的
    private bool 正在滚动 = false;
    private float 目标滚动位置 = 0f;
    private Coroutine 自动确认协程;
    private bool 用户正在拖拽 = false;
    private bool isTeleporting = false;
    private float realContentHeight = 0; // 真实内容的高度
    private int _lastChangedIndex = -1; // 用于触发On选择改变事件
    
    // 事件
    public Action<int, string> On选择改变;
    public Action<int, string> On选择确认; // 新增确认事件
    
    void Start()
    {
        初始化滚轮();
    }
    
    /// <summary>
    /// 初始化滚轮
    /// </summary>
    void 初始化滚轮()
    {
        Debug.Log($"[滚轮选择器] 正在初始化: {gameObject.name} (虚拟化模式)");

        // --- 检查引用 ---
        if (滚轮ScrollRect == null || 内容容器 == null || 项目预制体 == null)
        {
            Debug.LogError($"[滚轮选择器] {gameObject.name} 的一个或多个关键UI引用未在Inspector中设置！");
            return;
        }
        
        // --- 警告：手动操作 ---
        if (内容容器.GetComponent<VerticalLayoutGroup>() != null || 内容容器.GetComponent<ContentSizeFitter>() != null)
        {
            Debug.LogError($"[滚轮选择器] {gameObject.name} 的内容容器上不能有 VerticalLayoutGroup 或 ContentSizeFitter 组件，请在Inspector中手动移除它们以启用虚拟化滚动！", gameObject);
        }

        // --- 清理旧项目 ---
        foreach (var item in 项目池)
        {
            if(item != null) Destroy(item);
        }
        项目池.Clear();

        if (选项列表 == null || 选项列表.Count == 0)
        {
            Debug.LogWarning($"[滚轮选择器] {gameObject.name} 的选项列表为空，初始化中止。");
            return;
        }

        // --- 核心修改：为循环滚动准备数据 ---
        _displayList = new List<string>(选项列表);
        _padding = 0;
        
        // --- 计算项目高度 ---
        if (滚轮ScrollRect.viewport != null && 可见项目数量 > 0)
        {
            float totalSpacing = Mathf.Max(0, (可见项目数量 - 1) * 项目间距);
            float totalItemHeight = 滚轮ScrollRect.viewport.rect.height - totalSpacing;
            项目高度 = totalItemHeight / 可见项目数量;

            if (项目高度 <= 0)
            {
                Debug.LogError($"[滚轮选择器] {gameObject.name} 的项目高度计算结果为负数或零！请检查'可见项目数量'和'项目间距'相对于视口高度是否过大。");
                项目高度 = 滚轮ScrollRect.viewport.rect.height / 可见项目数量;
            }
        }
        
        // --- 虚拟化核心：计算并设置内容总高度 ---
        float itemWithSpacingHeight = 项目高度 + 项目间距;
        realContentHeight = (选项列表.Count * itemWithSpacingHeight);
        float contentHeight = 启用循环滚动 ? realContentHeight * 1000f : realContentHeight; // 无限画布
        内容容器.GetComponent<RectTransform>().sizeDelta = new Vector2(内容容器.GetComponent<RectTransform>().sizeDelta.x, contentHeight);

        // --- 创建对象池 (只创建少量实例) ---
        int poolSize = 可见项目数量 + 项目池大小缓冲;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject 项目 = Instantiate(项目预制体, 内容容器);
            RectTransform itemRect = 项目.GetComponent<RectTransform>();
            if (itemRect != null)
            {
                // 手动设置锚点到顶部中心，方便位置计算
                itemRect.anchorMin = new Vector2(0.5f, 1f);
                itemRect.anchorMax = new Vector2(0.5f, 1f);
                itemRect.pivot = new Vector2(0.5f, 1f);
                itemRect.sizeDelta = new Vector2(itemRect.sizeDelta.x, 项目高度);
            }
            项目.SetActive(false);
            项目池.Add(项目);
        }
        
        // --- 根据是否循环，设置不同的滚动模式 ---
        滚轮ScrollRect.movementType = 启用循环滚动 ? ScrollRect.MovementType.Unrestricted : ScrollRect.MovementType.Elastic;
        滚轮ScrollRect.inertia = false;
        
        // --- 设置滚动灵敏度 ---
        滚轮ScrollRect.scrollSensitivity = 滚动灵敏度;

        // --- 添加监听器 ---
        滚轮ScrollRect.onValueChanged.RemoveAllListeners(); // 先清理旧监听
        滚轮ScrollRect.onValueChanged.AddListener(On滚动);
        添加拖拽监听();
        
        // --- 初始化UI状态 ---
        // 直接调用更新，并设置到初始位置
        更新项目显示();
        // 在下一帧设置，确保布局计算完成
        StartCoroutine(延迟设置初始位置());
    }

    /// <summary>
    /// 从外部设置是否启用自动确认
    /// </summary>
    /// <param name="enabled"></param>
    public void 设置自动确认(bool enabled)
    {
        启用自动确认 = enabled;
    }

    private IEnumerator 延迟设置初始位置()
    {
        yield return null;
        if (启用循环滚动)
        {
            // 从中间开始滚动
            滚轮ScrollRect.verticalNormalizedPosition = 0.5f;
        }
        else
        {
            设置选中索引(当前选中索引 >= 0 && 当前选中索引 < 选项列表.Count ? 当前选中索引 : 0);
        }
    }

    private IEnumerator 初始化UI状态()
    {
        // 此方法在虚拟化模式下不再需要，由 延迟设置初始位置 替代
        yield break;
    }
    
    /// <summary>
    /// 添加拖拽监听
    /// </summary>
    void 添加拖拽监听()
    {
        // 添加EventTrigger组件来监听拖拽事件
        EventTrigger eventTrigger = 滚轮ScrollRect.gameObject.GetComponent<EventTrigger>();
        if (eventTrigger == null)
        {
            eventTrigger = 滚轮ScrollRect.gameObject.AddComponent<EventTrigger>();
        }
        
        // 添加开始拖拽事件
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry();
        beginDragEntry.eventID = EventTriggerType.BeginDrag;
        beginDragEntry.callback.AddListener((data) => { On开始拖拽((PointerEventData)data); });
        eventTrigger.triggers.Add(beginDragEntry);
        
        // 添加结束拖拽事件
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) => { On结束拖拽((PointerEventData)data); });
        eventTrigger.triggers.Add(endDragEntry);
    }
    
    /// <summary>
    /// 开始拖拽
    /// </summary>
    void On开始拖拽(PointerEventData eventData)
    {
        用户正在拖拽 = true;
        if (自动确认协程 != null)
        {
            StopCoroutine(自动确认协程);
            自动确认协程 = null;
        }
    }
    
    /// <summary>
    /// 结束拖拽
    /// </summary>
    void On结束拖拽(PointerEventData eventData)
    {
        用户正在拖拽 = false;
        
        if (isTeleporting) return;

        // --- 核心修改：基于 Content 位置和中心点偏移计算 ---
        float itemWithSpacingHeight = 项目高度 + 项目间距;
        float centeringOffset = (滚轮ScrollRect.viewport.GetComponent<RectTransform>().rect.height - 项目高度) / 2f;
        float adjustedScrollOffset = 内容容器.GetComponent<RectTransform>().anchoredPosition.y + centeringOffset;
        
        int index = Mathf.RoundToInt(adjustedScrollOffset / itemWithSpacingHeight);
        
        int realIndex = GetRealIndex(index);
        
        设置选中索引(realIndex, () => {
            if (启用自动确认)
            {
                确认当前选择();
            }
        });
    }
    
    /// <summary>
    /// 确认当前选择
    /// </summary>
    void 确认当前选择()
    {
        // 重要：确认事件发送的是真实的索引和值
        if (当前选中索引 >= 0 && 当前选中索引 < 选项列表.Count)
        {
            On选择确认?.Invoke(当前选中索引, 选项列表[当前选中索引]);
        }
    }
    
    /// <summary>
    /// 滚动回调
    /// </summary>
    void On滚动(Vector2 value)
    {
        更新项目显示();
        正在滚动 = true;
        
        // --- 核心修复：在滚动时实时触发On选择改变事件 ---
        if (选项列表.Count == 0) return;

        // 计算当前中心索引
        float itemWithSpacingHeight = 项目高度 + 项目间距;
        if (itemWithSpacingHeight <= 0) return;
        float centeringOffset = (滚轮ScrollRect.viewport.rect.height - 项目高度) / 2f;
        float adjustedScrollOffset = 内容容器.GetComponent<RectTransform>().anchoredPosition.y + centeringOffset;
        int index = Mathf.RoundToInt(adjustedScrollOffset / itemWithSpacingHeight);
        int realIndex = GetRealIndex(index);

        // 如果索引发生变化，则触发事件
        if (realIndex != _lastChangedIndex)
        {
            _lastChangedIndex = realIndex;
            On选择改变?.Invoke(realIndex, 选项列表[realIndex]);
        }
    }

    void LateUpdate()
    {
        if (启用循环滚动 && 用户正在拖拽)
        {
            var contentRect = 内容容器.GetComponent<RectTransform>();
            // 检查是否需要重置Content的位置
            if (contentRect.anchoredPosition.y < 0)
            {
                contentRect.anchoredPosition += new Vector2(0, realContentHeight);
            }
            else if (contentRect.anchoredPosition.y > realContentHeight)
            {
                contentRect.anchoredPosition -= new Vector2(0, realContentHeight);
            }
        }
    }
    
    /// <summary>
    /// 更新项目显示 - 虚拟化核心
    /// </summary>
    void 更新项目显示()
    {
        // --- 健壮性修复：如果没有任何数据，则不执行任何操作 ---
        if (_displayList == null || _displayList.Count == 0)
        {
            return;
        }

        float itemWithSpacingHeight = 项目高度 + 项目间距;
        if (itemWithSpacingHeight <= 0) return; // 防止除以零

        RectTransform contentRect = 内容容器.GetComponent<RectTransform>();
        RectTransform viewportRect = 滚轮ScrollRect.viewport.GetComponent<RectTransform>();

        // --- 核心修改：使用 anchoredPosition 计算 ---
        float scrollOffset = contentRect.anchoredPosition.y;
        int firstDataIndex = Mathf.FloorToInt(scrollOffset / itemWithSpacingHeight);
        
        // --- 获取视觉效果计算所需的参数 ---
        Vector3 viewportCenter = 滚轮ScrollRect.viewport.TransformPoint(viewportRect.rect.center);
        float maxDistance = viewportRect.rect.height / 2f;

        // --- 循环利用对象池中的项目 ---
        for (int i = 0; i < 项目池.Count; i++)
        {
            // --- 核心修改：索引计算 ---
            int dataIndex = firstDataIndex + i;
            GameObject 项目 = 项目池[i];
            
            // 计算真实索引
            int realIndex = GetRealIndex(dataIndex);

            if (realIndex >= 0 && realIndex < _displayList.Count)
            {
                项目.SetActive(true);
                
                // 更新内容
                TMP_Text 文本组件 = 项目.GetComponentInChildren<TMP_Text>();
                if (文本组件 != null)
                {
                    文本组件.text = _displayList[realIndex];
                }

                // 更新位置
                RectTransform rect = 项目.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(0, -(dataIndex * itemWithSpacingHeight));
                
                // 应用视觉效果
                float distance = Mathf.Abs(viewportCenter.y - rect.position.y);
                float distanceRatio = Mathf.Clamp01(1 - (distance / maxDistance));
                应用视觉效果(项目, distanceRatio, realIndex == 当前选中索引);
                
                // 更新点击事件
                Button 按钮 = 项目.GetComponent<Button>();
                if (按钮 != null)
                {
                    按钮.onClick.RemoveAllListeners();
                    int 点击索引 = realIndex;
                    按钮.onClick.AddListener(() => 点击确认选择(点击索引));
                }
            }
            else
            {
                项目.SetActive(false);
            }
        }
    }
    
    /// <summary>
    /// 点击确认选择
    /// </summary>
    void 点击确认选择(int 索引)
    {
        // 点击事件现在直接传递真实索引
        设置选中索引(索引);
        确认当前选择();
    }
    
    /// <summary>
    /// 应用视觉效果
    /// </summary>
    void 应用视觉效果(GameObject 项目, float 距离比例, bool 是否选中)
    {
        Image 背景图片 = 项目.GetComponent<Image>();
        TMP_Text 文本组件 = 项目.GetComponentInChildren<TMP_Text>();
        RectTransform rect = 项目.GetComponent<RectTransform>();

        // 根据距离比例平滑地计算透明度和缩放
        float alpha = Mathf.Lerp(未选中透明度, 1f, 距离比例);
        float scale = Mathf.Lerp(缩放效果, 1f, 距离比例);

        // 应用颜色和透明度
        Color targetColor = 是否选中 ? 选中颜色 : 未选中颜色;
        targetColor.a = alpha;

        if (背景图片 != null)
        {
            背景图片.color = targetColor;
        }
        if (文本组件 != null)
        {
            文本组件.color = targetColor;
        }
        
        // 应用缩放
        if(rect != null)
        {
            rect.localScale = Vector3.one * scale;
        }
    }
    
    /// <summary>
    /// 设置选中索引
    /// </summary>
    public void 设置选中索引(int 索引, Action onComplete = null)
    {
        // 公开方法接收的是真实索引
        索引 = Mathf.Clamp(索引, 0, 选项列表.Count - 1);
        当前选中索引 = 索引;

        if (_displayList.Count <= 1)
        {
            onComplete?.Invoke();
            return;
        }
        
        float itemWithSpacingHeight = 项目高度 + 项目间距;
        float centeringOffset = (滚轮ScrollRect.viewport.GetComponent<RectTransform>().rect.height - 项目高度) / 2f;

        if (启用循环滚动)
        {
            float currentY = 内容容器.GetComponent<RectTransform>().anchoredPosition.y;
            float currentAdjustedY = currentY + centeringOffset;
            float targetAdjustedYBase = 索引 * itemWithSpacingHeight;
            
            // 使用稳健的模运算保证结果为正
            float currentWrapped = (currentAdjustedY % realContentHeight + realContentHeight) % realContentHeight;
            float targetWrapped = (targetAdjustedYBase % realContentHeight + realContentHeight) % realContentHeight;

            float diff = targetWrapped - currentWrapped;
            
            // 寻找最短路径
            if (Mathf.Abs(diff) > realContentHeight / 2)
            {
                 diff -= Mathf.Sign(diff) * realContentHeight;
            }
            
            float targetAdjustedY = currentAdjustedY + diff;
            目标滚动位置 = targetAdjustedY - centeringOffset;
        }
        else
        {
             目标滚动位置 = 索引 * itemWithSpacingHeight - centeringOffset;
        }
        
        StartCoroutine(平滑滚动(onComplete));
    }
    
    /// <summary>
    /// 平滑滚动
    /// </summary>
    IEnumerator 平滑滚动(Action onComplete)
    {
        正在滚动 = true;
        RectTransform contentRect = 内容容器.GetComponent<RectTransform>();
        float 开始位置 = contentRect.anchoredPosition.y;
        float 时间 = 0f;
        float 持续时间 = 0.3f;
        
        while (时间 < 持续时间)
        {
            时间 += Time.deltaTime;
            float 进度 = 时间 / 持续时间;
            float 当前位置Y = Mathf.Lerp(开始位置, 目标滚动位置, 进度);
            contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 当前位置Y);
            
            yield return null;
        }
        
        contentRect.anchoredPosition = new Vector2(contentRect.anchoredPosition.x, 目标滚动位置);
        正在滚动 = false;
        更新项目显示();
        
        // 动画结束，执行回调
        onComplete?.Invoke();
    }
    
    /// <summary>
    /// 设置选项列表
    /// </summary>
    public void 设置选项列表(List<string> 新选项列表)
    {
        选项列表 = 新选项列表;
        当前选中索引 = 0; // 重置索引
        
        // 强制重新初始化滚轮来使用新的数据
        // 这会清空旧项目、创建新项目并刷新UI
        初始化滚轮();
    }
    
    /// <summary>
    /// 获取当前选中值
    /// </summary>
    public string 获取当前选中值()
    {
        if (选项列表.Count > 0 && 当前选中索引 >= 0 && 当前选中索引 < 选项列表.Count)
        {
            return 选项列表[当前选中索引];
        }
        return "";
    }
    
    /// <summary>
    /// 获取当前选中索引
    /// </summary>
    public int 获取当前选中索引()
    {
        return 当前选中索引;
    }
    
    /// <summary>
    /// 手动确认当前选择
    /// </summary>
    public void 手动确认选择()
    {
        确认当前选择();
    }
    
    private int GetRealIndex(int displayIndex)
    {
        if (_displayList.Count == 0) return 0;
        return (displayIndex % _displayList.Count + _displayList.Count) % _displayList.Count;
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
} 