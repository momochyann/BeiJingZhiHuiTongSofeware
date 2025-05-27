using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;
using TMPro;

public class EyeTest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Image 小球;
    [SerializeField] Button 开始按钮;
    [SerializeField] Slider 速度调节条;
    [SerializeField] RectTransform 移动区域; // 小球移动的区域
    [SerializeField] Button 结束按钮;
    [SerializeField] string 跳转界面名称;
    
    [Header("音乐设置")]
    [SerializeField] TMP_Dropdown 音乐下拉框;
    [SerializeField] AudioClip[] 音乐列表;
    [SerializeField] Slider 音量调节条;
    [SerializeField] bool 自动播放音乐 = true;
    
    private Vector2 初始位置;
    private float 基础移动时间 = 1.5f; // 基础移动时间（秒）
    private float 当前速度倍率 = 1.0f;
    private bool 正在运行 = false;
    private Sequence 移动序列;
    
    // 音频播放器
    private AudioSource 音频播放器;
    
    void Start()
    {
        // 初始化
        初始化界面();
        初始化音乐播放器();
        初始化音乐下拉框();
        
        // 注册事件
        开始按钮.onClick.AddListener(切换运行状态);
        结束按钮.onClick.AddListener(跳转界面);
        速度调节条.onValueChanged.AddListener(调整速度);
        
        if (音量调节条 != null)
        {
            音量调节条.onValueChanged.AddListener(调整音量);
        }
    }

    private void 跳转界面()
    {
        WorkSceneManager.Instance.跳转界面(跳转界面名称, this.gameObject).Forget();
    }

    void OnDestroy()
    {
        // 清理
        if (移动序列 != null)
        {
            移动序列.Kill();
        }
        
        // 停止音乐
        if (音频播放器 != null && 音频播放器.isPlaying)
        {
            音频播放器.Stop();
        }
        
        // 移除事件监听
        开始按钮.onClick.RemoveListener(切换运行状态);
        结束按钮.onClick.RemoveListener(跳转界面);
        速度调节条.onValueChanged.RemoveListener(调整速度);
        
        if (音量调节条 != null)
        {
            音量调节条.onValueChanged.RemoveListener(调整音量);
        }
        
        if (音乐下拉框 != null)
        {
            音乐下拉框.onValueChanged.RemoveAllListeners();
        }
    }
    
    private void 初始化界面()
    {
        // 设置初始位置（中心点）
        初始位置 = Vector2.zero;
        小球.rectTransform.anchoredPosition = 初始位置;
        
        // 设置速度调节条初始值
        速度调节条.value = 1.0f;
        速度调节条.minValue = 0.5f;
        速度调节条.maxValue = 2.0f;
        
        // 设置按钮文本
        Text 按钮文本 = 开始按钮.GetComponentInChildren<Text>();
        if (按钮文本 != null)
        {
            按钮文本.text = "开始";
        }
        
        // 设置音量调节条初始值
        if (音量调节条 != null)
        {
            音量调节条.value = 0.7f;
            音量调节条.minValue = 0f;
            音量调节条.maxValue = 1f;
        }
    }
    
    private void 初始化音乐播放器()
    {
        // 添加音频播放器组件
        音频播放器 = gameObject.AddComponent<AudioSource>();
        音频播放器.playOnAwake = false;
        音频播放器.loop = true;
        音频播放器.volume = 音量调节条 != null ? 音量调节条.value : 0.7f;
    }
    
    private void 初始化音乐下拉框()
    {
        if (音乐下拉框 == null || 音乐列表 == null || 音乐列表.Length == 0)
            return;
        
        // 清空下拉框选项
        音乐下拉框.ClearOptions();
        
        // 添加音乐名称到下拉框
        List<TMP_Dropdown.OptionData> 选项列表 = new List<TMP_Dropdown.OptionData>();
        
        // 添加"无音乐"选项
        选项列表.Add(new TMP_Dropdown.OptionData("无音乐"));
        
        // 添加音乐列表
        foreach (AudioClip 音乐 in 音乐列表)
        {
            if (音乐 != null)
            {
                选项列表.Add(new TMP_Dropdown.OptionData(音乐.name));
            }
        }
        
        音乐下拉框.AddOptions(选项列表);
        
        // 设置默认选项为第一首音乐（如果有）
        音乐下拉框.value = 音乐列表.Length > 0 ? 1 : 0;
        
        // 添加选择事件
        音乐下拉框.onValueChanged.AddListener(选择音乐);
        
        // 如果设置了自动播放，则播放默认选中的音乐
        if (自动播放音乐 && 音乐列表.Length > 0)
        {
            选择音乐(音乐下拉框.value);
        }
    }
    
    private void 选择音乐(int 索引)
    {
        // 停止当前播放的音乐
        if (音频播放器.isPlaying)
        {
            音频播放器.Stop();
        }
        
        // 索引0是"无音乐"选项
        if (索引 == 0)
        {
            return;
        }
        
        // 调整索引（因为添加了"无音乐"选项）
        int 音乐索引 = 索引 - 1;
        
        // 检查索引是否有效
        if (音乐索引 >= 0 && 音乐索引 < 音乐列表.Length && 音乐列表[音乐索引] != null)
        {
            // 设置音频剪辑
            音频播放器.clip = 音乐列表[音乐索引];
            
            // 播放音乐
            音频播放器.Play();
            
            Debug.Log($"正在播放音乐: {音乐列表[音乐索引].name}");
        }
    }
    
    private void 调整音量(float 音量)
    {
        if (音频播放器 != null)
        {
            音频播放器.volume = 音量;
        }
    }
    
    private void 切换运行状态()
    {
        正在运行 = !正在运行;
        
        // 更新按钮文本
        Text 按钮文本 = 开始按钮.GetComponentInChildren<Text>();
        if (按钮文本 != null)
        {
            按钮文本.text = 正在运行 ? "停止" : "开始";
        }
        
        if (正在运行)
        {
            开始移动();
        }
        else
        {
            停止移动();
        }
    }
    
    private void 调整速度(float 值)
    {
        当前速度倍率 = 值;
        
        // 如果正在运行，更新移动速度
        if (正在运行 && 移动序列 != null)
        {
            移动序列.timeScale = 当前速度倍率;
        }
    }
    
    private void 开始移动()
    {
        // 如果已有序列在运行，先停止
        if (移动序列 != null)
        {
            移动序列.Kill();
        }
        
        // 获取移动区域的边界
        float 左边界 = -移动区域.rect.width / 2 + 小球.rectTransform.rect.width / 2;
        float 右边界 = 移动区域.rect.width / 2 - 小球.rectTransform.rect.width / 2;
        float 上边界 = 移动区域.rect.height / 2 - 小球.rectTransform.rect.height / 2;
        float 下边界 = -移动区域.rect.height / 2 + 小球.rectTransform.rect.height / 2;
        
        // 创建移动序列
        移动序列 = DOTween.Sequence();
        
        // 1. 从中心到左边界
        移动序列.Append(小球.rectTransform.DOAnchorPosX(左边界, 基础移动时间).SetEase(Ease.Linear));
        
        // 2. 从左边界到右边界
        移动序列.Append(小球.rectTransform.DOAnchorPosX(右边界, 基础移动时间 * 2).SetEase(Ease.Linear));
        
        // 3. 从右边界回到中心
        移动序列.Append(小球.rectTransform.DOAnchorPosX(初始位置.x, 基础移动时间).SetEase(Ease.Linear));
        
        // 4. 从中心到上边界
        移动序列.Append(小球.rectTransform.DOAnchorPosY(上边界, 基础移动时间).SetEase(Ease.Linear));
        
        // 5. 从上边界到下边界
        移动序列.Append(小球.rectTransform.DOAnchorPosY(下边界, 基础移动时间 * 2).SetEase(Ease.Linear));
        
        // 6. 从下边界回到中心
        移动序列.Append(小球.rectTransform.DOAnchorPosY(初始位置.y, 基础移动时间).SetEase(Ease.Linear));
        
        // 设置循环
        移动序列.SetLoops(-1, LoopType.Restart);
        
        // 设置速度
        移动序列.timeScale = 当前速度倍率;
        
        // 播放序列                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        移动序列.Play();
    }
    
    private void 停止移动()
    {
        if (移动序列 != null)
        {
            移动序列.Pause();
            
            // 回到初始位置
            小球.rectTransform.DOAnchorPos(初始位置, 0.5f);
        }
    }
}
