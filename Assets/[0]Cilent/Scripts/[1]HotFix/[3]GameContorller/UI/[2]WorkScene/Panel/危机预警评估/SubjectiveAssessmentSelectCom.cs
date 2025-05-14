using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using System.Linq;
public class SubjectiveAssessmentSelectCom : MonoBehaviour, IController
{
    // Start is called before the first frame update
    List<Toggle> toggleList = new List<Toggle>();
    List<int> 等级列表 = new List<int>();
    [SerializeField] GameObject 调级面板;
    string 等级滑动条PfbName = "等级滑动条";
    GameObject 等级滑动条pfb;
    // 添加初始选项数量记录
    private int 初始子物体数量 = 5;
    // 添加上下空白区域大小设置
    [SerializeField] private float 上方空白区域 = 10;
    [SerializeField] private float 下方空白区域 = 10;
    void Start()
    {

        调级面板 = FindObjectOfType<SubjectiveAssessmentArchiveStartPanel3>().调级面板;
        AsyncInit().Forget();
        
        // 记录初始子物体数量
        // 初始子物体数量 = 调级面板.transform.childCount;
    }
    async UniTaskVoid AsyncInit()
    {
        toggleList = transform.GetComponentsInChildren<Toggle>(true).ToList();
        var model = this.GetModel<YooAssetPfbModel>();
        等级滑动条pfb = await model.LoadPfb(等级滑动条PfbName);
        检查并设置已有选项的Toggle状态();
        toggleList.ForEach(toggle =>
        {

          等级列表.Add(0);
          toggle.onValueChanged.AddListener(isOn =>
          {
              if (isOn)
              {
                  //    toggleList.Where(t => t != toggle && t.isOn).ToList().ForEach(t => t.isOn = false);
                  生成调节选项(toggle);
              }
              else
              {
                  清除调节选项(toggle);
              }
          });
        });
    }
    void 检查并设置已有选项的Toggle状态()
    {
        // 遍历所有Toggle
        foreach (var toggle in toggleList)
        {
            // 获取Toggle子物体下的Text组件
            Text toggleText = toggle.GetComponentInChildren<Text>();
            if (toggleText == null) continue;
            if (调级面板.transform.childCount == 0)
            {
                toggle.isOn = false;
                continue;
            }

            // 遍历调级面板的一级子物体
            for (int i = 0; i < 调级面板.transform.childCount; i++)
            {
                Transform child = 调级面板.transform.GetChild(i);

                // 检查该子物体是否有第一个子物体
                if (child.childCount > 0)
                {
                    // 获取第一个子物体的Text组件
                    Text childText = child.GetChild(0).GetComponent<Text>();

                    // 如果找到Text组件且文本与Toggle的Text相同，则将该Toggle设置为选中状态
                    if (childText != null && childText.text == toggleText.text)
                    {
                        toggle.isOn = true;
                        break;
                    }
                    else
                    {
                        toggle.isOn = false;
                    }
                }
            }
        }
    }
    void 清除调节选项(Toggle toggle)
    {
        // 获取当前选中的Toggle的索引
        int currentIndex = toggleList.IndexOf(toggle);

        // 获取Toggle子物体下的Text组件
        Text toggleText = toggle.GetComponentInChildren<Text>();

        // 遍历调级面板的一级子物体
        for (int i = 0; i < 调级面板.transform.childCount; i++)
        {
            Transform child = 调级面板.transform.GetChild(i);

            // 检查该子物体是否有第一个子物体
            if (child.childCount > 0)
            {
                // 获取第一个子物体的Text组件
                Text childText = child.GetChild(0).GetComponent<Text>();

                // 如果找到Text组件且文本与Toggle的Text相同，则销毁该一级子物体
                if (childText != null && toggleText != null && childText.text == toggleText.text)
                {
                    Destroy(child.gameObject);
                    break;
                }
            }
        }
        刷新滚动区域(false);
    }
    void 生成调节选项(Toggle toggle)
    {
        // 获取当前选中的Toggle的索引
        int currentIndex = toggleList.IndexOf(toggle);

        // 获取Toggle子物体下的Text组件
        Text toggleText = toggle.GetComponentInChildren<Text>();


        // 实例化滑动条
        var sliderObj = Instantiate(等级滑动条pfb, 调级面板.transform);

        // 设置滑动条第一个子物体的Text等于toggle子物体下的Text
        Text sliderText = sliderObj.transform.GetChild(0).GetComponent<Text>();
        if (sliderText != null && toggleText != null)
        {
            sliderText.text = toggleText.text;
        }

        // 获取滑动条组件
        Slider slider = sliderObj.GetComponentInChildren<Slider>(true);

        // 查找显示数值的Text组件
        Text valueText = null;
        valueText = slider.GetComponentInChildren<Text>(true);



        if (slider != null)
        {
            // 设置滑动条为整数模式
            slider.wholeNumbers = true;

            // 设置滑动条的初始值为等级列表中对应索引的值
            slider.value = 等级列表[currentIndex];

            // 如果找到了显示数值的Text，初始化它的值
            if (valueText != null)
            {
                valueText.text = 等级列表[currentIndex].ToString();
            }

            // 添加滑动事件监听
            slider.onValueChanged.AddListener((value) =>
            {
                // 将值转换为整数并更新等级列表
                int intValue = Mathf.RoundToInt(value);
                等级列表[currentIndex] = intValue;

                // 更新显示数值的Text
                if (valueText != null)
                {
                    valueText.text = intValue.ToString();
                }
            });
        }
        刷新滚动区域(true);
    }
    void 刷新滚动区域(bool 滚动到底部 = true)
    {
        // 获取当前子物体数量
        int 当前子物体数量 = 调级面板.transform.childCount;
        
        // 如果当前子物体数量小于等于初始数量，不进行刷新
        if (当前子物体数量 <= 初始子物体数量)
        {
            return; 
        }    
        
        // 延迟一帧执行，确保Grid Layout Group有时间计算布局
        StartCoroutine(延迟刷新布局(滚动到底部));
    }

    IEnumerator 延迟刷新布局(bool 滚动到底部)
    {
        // 等待一帧
        yield return null;
        
        // 获取Content对象
        RectTransform contentRect = 调级面板.GetComponent<RectTransform>();
        
        // 获取Grid Layout Group组件
        GridLayoutGroup gridLayout = 调级面板.GetComponent<GridLayoutGroup>();
        if (gridLayout != null)
        {
            // 计算内容总高度
            int 子物体数量 = 调级面板.transform.childCount;
            int 每行数量 = Mathf.Max(1, Mathf.FloorToInt((contentRect.rect.width - gridLayout.padding.left - gridLayout.padding.right + gridLayout.spacing.x) / (gridLayout.cellSize.x + gridLayout.spacing.x)));
            int 行数 = Mathf.CeilToInt((float)子物体数量 / 每行数量);
            
            // 计算基础高度
            float 基础高度 = gridLayout.padding.top + gridLayout.padding.bottom + 行数 * gridLayout.cellSize.y + (行数 - 1) * gridLayout.spacing.y;
            
            // 添加上下空白区域
            float 总高度 = 基础高度 + 上方空白区域 + 下方空白区域;
            
            // 设置Content的高度
            contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, 总高度);
            
            // 调整Grid Layout Group的上边距，增加上方空白
            gridLayout.padding.top += (int)上方空白区域;
            
            // 调整Grid Layout Group的下边距，增加下方空白
            gridLayout.padding.bottom += (int)下方空白区域;
        }
        
        // 强制刷新布局
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        
        // 获取ScrollRect组件
        ScrollRect scrollRect = 调级面板.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            // 确保Content是ScrollRect的内容
            if (scrollRect.content != contentRect)
            {
                scrollRect.content = contentRect;
            }
            
            // 刷新ScrollRect
            Canvas.ForceUpdateCanvases();
            
            // 如果需要滚动到底部
            if (滚动到底部)
            {
                // 等待一帧，确保布局已更新
                yield return null;
                
                // 滚动到底部 (0表示底部，1表示顶部)
                scrollRect.normalizedPosition = new Vector2(scrollRect.normalizedPosition.x, 0);
                
                // 再次刷新Canvas确保滚动位置已应用
                Canvas.ForceUpdateCanvases();
            }
        }
        
        // 再次强制刷新布局，确保所有更改都已应用
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }
    void OnDestroy()
    {
       StopCoroutine(延迟刷新布局(true));
    }
    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
