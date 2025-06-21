using UnityEngine;
using QFramework;

/// <summary>
/// 搜索框管理器 - 确保自定义键盘系统正常工作
/// </summary>
public class 搜索框管理器 : MonoBehaviour, IController
{
    [Header("自动初始化设置")]
    [SerializeField] bool 自动初始化自定义键盘 = true;
    
    private void Start()
    {
        if (自动初始化自定义键盘)
        {
            初始化自定义键盘系统();
        }
    }
    
    /// <summary>
    /// 初始化自定义键盘系统
    /// </summary>
    void 初始化自定义键盘系统()
    {
        // 确保CustomKeyboardManager存在
        if (CustomKeyboard.CustomKeyboardManager.Instance == null)
        {
            Debug.LogWarning("搜索框管理器: CustomKeyboardManager未找到，正在创建...");
            
            // 创建一个空的GameObject并添加CustomKeyboardManager组件
            GameObject keyboardManagerGO = new GameObject("CustomKeyboardManager");
            keyboardManagerGO.AddComponent<CustomKeyboard.CustomKeyboardManager>();
            
            // 设置为不销毁
            DontDestroyOnLoad(keyboardManagerGO);
        }
        
        // 显示平台信息
        Debug.Log($"搜索框管理器: 当前平台 - {Application.platform}");
        Debug.Log($"搜索框管理器: 是否为移动端 - {Application.isMobilePlatform}");
        
        if (Application.isMobilePlatform)
        {
            Debug.Log($"搜索框管理器: Android版本信息 - {CustomKeyboard.AndroidKeyboardHelper.GetAndroidVersionInfo()}");
        }
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
} 