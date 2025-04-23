using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using System;
public class EntrySelect : MonoBehaviour, IController
{
    // Start is called before the first frame update
    Button 搜索按钮;
    InputField 搜索输入框;
    [SerializeField]
    private string 搜索数据类型名;
    bool 是否已经搜索 = false;
    private void Awake()
    {
        搜索按钮 = GetComponentInChildren<Button>();
        搜索输入框 = GetComponentInChildren<InputField>();
        搜索按钮.onClick.AddListener(OnSearchButtonClick);
        搜索输入框.onValueChanged.AddListener(OnValueChanged);
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
