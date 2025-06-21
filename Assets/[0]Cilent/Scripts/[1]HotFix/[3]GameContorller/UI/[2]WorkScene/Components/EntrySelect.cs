using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using System;
using TMPro;

public class EntrySelect : MonoBehaviour, IController
{
    // Start is called before the first frame update
    Button 搜索按钮;
    TMP_InputField 搜索输入框;
    [SerializeField]
    private string 搜索数据类型名;
    bool 是否已经搜索 = false;
    
    // 添加自定义键盘扩展组件引用
    private CustomKeyboard.CustomInputFieldExtension customKeyboardExtension;
    
    private void Awake()
    {
        搜索按钮 = GetComponentInChildren<Button>();
        搜索输入框 = GetComponentInChildren<TMP_InputField>();
      
        搜索按钮.onClick.AddListener(OnSearchButtonClick);
        if (搜索输入框 != null)
        {
            搜索输入框.onValueChanged.AddListener(OnValueChanged);
            
            // 设置光标样式
            设置光标样式();
            
            // 添加自定义键盘支持
            添加自定义键盘支持();
        }
    }

    /// <summary>
    /// 设置光标样式
    /// </summary>
    void 设置光标样式()
    {
        if (搜索输入框 != null)
        {
            // 设置光标颜色为黑色
            搜索输入框.caretColor = Color.black;
            
            // 启用自定义光标颜色
            搜索输入框.customCaretColor = true;
            
            // 设置光标闪烁频率
            搜索输入框.caretBlinkRate = 0.85f;
            
            // 设置光标宽度
            搜索输入框.caretWidth = 2;
            
            // 确保输入框可以交互
            搜索输入框.interactable = true;
            
            // 设置输入框激活时选中所有文本
            搜索输入框.onFocusSelectAll = true;
        }
    }
    
    /// <summary>
    /// 添加自定义键盘支持
    /// </summary>
    void 添加自定义键盘支持()
    {
        if (搜索输入框 != null)
        {
            // 检查是否已经有自定义键盘扩展组件
            customKeyboardExtension = 搜索输入框.GetComponent<CustomKeyboard.CustomInputFieldExtension>();
            
            // 如果没有，添加一个
            if (customKeyboardExtension == null)
            {
                customKeyboardExtension = 搜索输入框.gameObject.AddComponent<CustomKeyboard.CustomInputFieldExtension>();
            }
            
            // 配置自定义键盘设置
            if (customKeyboardExtension != null)
            {
                // 在移动端使用自定义键盘，在PC端使用系统键盘
                bool isMobile = Application.isMobilePlatform;
                customKeyboardExtension.SetUseCustomKeyboard(isMobile);
                
                // 设置输入框属性
                if (isMobile)
                {
                    // 移动端：隐藏系统键盘，使用自定义键盘
                    搜索输入框.shouldHideMobileInput = true;
                }
                else
                {
                    // PC端：使用系统键盘
                    搜索输入框.shouldHideMobileInput = false;
                }
            }
            
            // 添加输入框焦点事件监听
            搜索输入框.onSelect.AddListener(OnInputFieldSelected);
            搜索输入框.onDeselect.AddListener(OnInputFieldDeselected);
        }
    }
    
    /// <summary>
    /// 输入框获得焦点事件
    /// </summary>
    void OnInputFieldSelected(string value)
    {
        // 确保光标在搜索框中可见
        if (搜索输入框 != null)
        {
            搜索输入框.ActivateInputField();
            
            // 在移动端，确保光标位置正确
            if (Application.isMobilePlatform)
            {
                // 延迟一帧设置光标位置，确保UI更新完成
                StartCoroutine(延迟设置光标位置());
            }
        }
    }
    
    /// <summary>
    /// 输入框失去焦点事件
    /// </summary>
    void OnInputFieldDeselected(string value)
    {
        // 可以在这里添加失去焦点时的处理逻辑
    }
    
    /// <summary>
    /// 延迟设置光标位置
    /// </summary>
    IEnumerator 延迟设置光标位置()
    {
        yield return null; // 等待一帧
        
        if (搜索输入框 != null)
        {
            // 确保光标在文本末尾
            int textLength = 搜索输入框.text.Length;
            搜索输入框.caretPosition = textLength;
            搜索输入框.selectionAnchorPosition = textLength;
            搜索输入框.selectionFocusPosition = textLength;
        }
    }

    private void OnValueChanged(string arg0)
    {
        if (!是否已经搜索)
            return;
        FindObjectOfType<EntryDisPanelNew>().更新搜索页面(false);
        是否已经搜索 = false;
    }

    private void OnSearchButtonClick()
    {
        string keyword = 搜索输入框.text;
        if (!string.IsNullOrEmpty(keyword))
        {
            this.SendCommand(new SearchByNameCommand(keyword, 搜索数据类型名));
            FindObjectOfType<EntryDisPanelNew>().更新搜索页面(true);
            是否已经搜索 = true;
        }
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    
    /// <summary>
    /// 手动激活搜索框（用于外部调用）
    /// </summary>
    public void 激活搜索框()
    {
        if (搜索输入框 != null)
        {
            搜索输入框.Select();
            搜索输入框.ActivateInputField();
        }
    }
    
    /// <summary>
    /// 设置搜索框文本
    /// </summary>
    /// <param name="text">要设置的文本</param>
    public void 设置搜索文本(string text)
    {
        if (搜索输入框 != null)
        {
            搜索输入框.text = text;
        }
    }
    
    /// <summary>
    /// 清空搜索框
    /// </summary>
    public void 清空搜索框()
    {
        if (搜索输入框 != null)
        {
            搜索输入框.text = "";
        }
    }
}
