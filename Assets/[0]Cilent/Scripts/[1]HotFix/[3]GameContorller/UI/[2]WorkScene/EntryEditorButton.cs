using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine.Events;
enum EntryEditorButtonType
{
    删除按钮,
    编辑按钮,
    添加按钮
}
public class EntryEditorButton : MonoBehaviour, IController
{
    // Start is called before the first frame update
    // 在Inspector中显示提示信息

    [Header("基本设置")]
    [SerializeField] private EntryEditorButtonType buttonType;

    [Header("删除按钮设置 (仅当类型为删除按钮时使用)")]
    [SerializeField] private string entryModelName = "PersonalPersonnelCrisisEventMessageModel";

    [Header("创建按钮设置 (仅当类型为添加按钮或编辑按钮时使用)")]
    [SerializeField] private string 添加条目界面名称;
    Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }


    void OnClick()
    {
        switch (buttonType)
        {
            case EntryEditorButtonType.删除按钮:
                删除按钮处理();
                break;
            case EntryEditorButtonType.编辑按钮:
                编辑按钮处理();
                break;
            case EntryEditorButtonType.添加按钮:
                添加条目按钮监听(添加条目界面名称).Forget();
                break;
        }
    }
    
    /// <summary>
    /// 删除按钮处理逻辑
    /// </summary>
    void 删除按钮处理()
    {
        // 检查是否有选中的条目
        if (!检查是否有选中条目())
        {
            WorkSceneManager.Instance.加载提示("请先选择要删除的条目").Forget();
            return;
        }
        
        确认面板("是否要删除该条目？", () =>
        {
            删除条目(entryModelName);
        }).Forget();
    }
    
    /// <summary>
    /// 编辑按钮处理逻辑
    /// </summary>
    void 编辑按钮处理()
    {
        // 检查是否有选中的条目
        if (!检查是否有选中条目())
        {
            WorkSceneManager.Instance.加载提示("请先选择要编辑的条目").Forget();
            return;
        }
        
        编辑条目(添加条目界面名称).Forget();
    }
    
    /// <summary>
    /// 检查是否有选中的条目
    /// </summary>
    /// <returns>如果有选中的条目返回true，否则返回false</returns>
    bool 检查是否有选中条目()
    {
        EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        if (entryDisPanel == null)
        {
            Debug.LogWarning("未找到EntryDisPanelNew组件");
            return false;
        }
        
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        if (entries == null || entries.Length == 0)
        {
            Debug.LogWarning("未找到任何条目");
            return false;
        }
        
        foreach (IEntry entry in entries)
        {
            if (entry.IsChoose)
            {
                return true;
            }
        }
        
        return false;
    }
    
    async UniTaskVoid 添加条目按钮监听(string 添加条目界面名称)
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(添加条目界面名称);
        Instantiate(pfb, FindObjectOfType<Canvas>().transform);
    }
    
    void 删除条目(string entryModelName)
    {
        EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            if (entry.IsChoose)
            {
                this.SendCommand(new DeleteEntryCommand(entry.can2ListValue, entryModelName));
            }
        }
    }
    
    async UniTaskVoid 编辑条目(string 添加条目界面名称)
    {
        EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            Debug.Log("entry.IsChoose: " + entry.IsChoose + ", 物体名称: " + ((MonoBehaviour)entry).gameObject.name);
            
            if (entry.IsChoose)
            {
                var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(添加条目界面名称);
                var 编辑条目界面 = Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<PopPanelBase>();
                编辑条目界面.编辑条目(entry.can2ListValue);
                
                return;
            }
        }
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    async UniTaskVoid 确认面板(string 提示文本内容, UnityAction 确认回调, string 跳转面板名称 = "")
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");
        var 确认提示弹窗 = Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_TipPanel>();
        确认提示弹窗.显示面板(提示文本内容, 跳转面板名称, 确认回调);
    }
}
