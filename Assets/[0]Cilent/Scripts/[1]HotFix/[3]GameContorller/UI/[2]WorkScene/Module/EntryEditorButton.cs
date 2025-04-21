using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using QFramework;
public class EntryEditorButton : MonoBehaviour, IController
{
    // Start is called before the first frame update
    Button button;
    [SerializeField]
    private string entryModelName = "PersonalPersonnelCrisisEventMessageModel";
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }


    void OnClick()
    {
        string buttonName = gameObject.name;
        if (buttonName == "删除按钮")
        {
            删除条目();
        }
        else if (buttonName == "编辑按钮")
        {

        }
        else if (buttonName == "添加按钮")
        {

        }
    }
    void 删除条目()
    {
        EntryDisPanel entryDisPanel = FindObjectOfType<EntryDisPanel>();
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            if (entry.IsChoose)
            {
                this.SendCommand(new DeleteEntryCommand(entry, entryModelName));
            }
        }
    }
    void 编辑条目()
    {

    }
    void 添加条目()
    {

    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
