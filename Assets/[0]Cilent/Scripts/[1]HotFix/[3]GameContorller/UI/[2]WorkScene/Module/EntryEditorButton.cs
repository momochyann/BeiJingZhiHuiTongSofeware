using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using System;
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
                删除条目(entryModelName);
                break;
            case EntryEditorButtonType.编辑按钮:
                编辑条目(添加条目界面名称).Forget();
                break;
            case EntryEditorButtonType.添加按钮:
                添加条目按钮监听(添加条目界面名称).Forget();
                break;
        }
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
}
