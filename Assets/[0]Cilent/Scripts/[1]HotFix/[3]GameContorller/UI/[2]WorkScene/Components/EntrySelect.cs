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
}
