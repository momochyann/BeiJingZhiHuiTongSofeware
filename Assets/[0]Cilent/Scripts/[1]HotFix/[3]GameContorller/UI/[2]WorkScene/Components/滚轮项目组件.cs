using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 滚轮项目组件 - 滚轮选择器中的单个项目，滚动即确认
/// </summary>
public class 滚轮项目组件 : MonoBehaviour
{
    [Header("UI组件")]
    [SerializeField] Image 背景图片;
    [SerializeField] TMP_Text 文本组件;
    
    [Header("视觉效果")]
    [SerializeField] Color 默认颜色 = Color.white;
    [SerializeField] Color 选中颜色 = Color.cyan;
    [SerializeField] float 默认透明度 = 1f;
    [SerializeField] float 未选中透明度 = 0.5f;
    
    // 私有变量
    private string 项目值;
    private bool 是否选中 = false;
    
    void Awake()
    {
        初始化组件();
    }
    
    /// <summary>
    /// 初始化组件
    /// </summary>
    void 初始化组件()
    {
        if (背景图片 == null)
            背景图片 = GetComponent<Image>();
        
        if (文本组件 == null)
            文本组件 = GetComponentInChildren<TMP_Text>();
    }
    
    /// <summary>
    /// 设置项目数据
    /// </summary>
    public void 设置项目(string 值, string 显示文本)
    {
        项目值 = 值;
        if (文本组件 != null)
        {
            文本组件.text = 显示文本;
        }
    }
    
    /// <summary>
    /// 设置选中状态
    /// </summary>
    public void 设置选中状态(bool 选中)
    {
        是否选中 = 选中;
        更新视觉效果();
    }
    
    /// <summary>
    /// 设置透明度
    /// </summary>
    public void 设置透明度(float 透明度)
    {
        if (背景图片 != null)
        {
            Color 颜色 = 背景图片.color;
            颜色.a = 透明度;
            背景图片.color = 颜色;
        }
        
        if (文本组件 != null)
        {
            Color 文本颜色 = 文本组件.color;
            文本颜色.a = 透明度;
            文本组件.color = 文本颜色;
        }
    }
    
    /// <summary>
    /// 设置缩放
    /// </summary>
    public void 设置缩放(float 缩放)
    {
        transform.localScale = Vector3.one * 缩放;
    }
    
    /// <summary>
    /// 更新视觉效果
    /// </summary>
    void 更新视觉效果()
    {
        if (背景图片 != null)
        {
            背景图片.color = 是否选中 ? 选中颜色 : 默认颜色;
        }
    }
    
    /// <summary>
    /// 获取项目值
    /// </summary>
    public string 获取项目值()
    {
        return 项目值;
    }
    
    /// <summary>
    /// 获取选中状态
    /// </summary>
    public bool 获取选中状态()
    {
        return 是否选中;
    }
    
    /// <summary>
    /// 重置状态
    /// </summary>
    public void 重置状态()
    {
        是否选中 = false;
        设置透明度(默认透明度);
        设置缩放(1f);
        更新视觉效果();
    }
} 