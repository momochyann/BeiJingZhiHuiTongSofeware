using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using TMPro;

public class 干预人员管理界面 : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 确认干预人员;
    [SerializeField] TMP_Text 当前干预人员;

    void Start()
    {
        加载干预人员();
        确认干预人员.onClick.AddListener(改变干预人员);
    }


    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
    void 加载干预人员()
    {
        var 当前干预人员名称 = PlayerPrefs.GetString("当前干预人员");
        if (string.IsNullOrEmpty(当前干预人员名称))
        {
            当前干预人员.text = "主干预人员:待确认";
            return;
        }


        当前干预人员.text = "主干预人员:" + 当前干预人员名称;


        EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            var 干预人员 = entry.can2ListValue as Intervener;
            if(干预人员==null)
            return;
            if (干预人员.name == 当前干预人员名称)
            {
                entry.IsChoose = true;
            }
        }
    }

    void 改变干预人员()
    {
        EntryDisPanelNew entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            if (entry.IsChoose)
            {
                var 干预人员 = entry.can2ListValue as Intervener;
                PlayerPrefs.SetString("当前干预人员", 干预人员.name);
                当前干预人员.text = "主干预人员:" + 干预人员.name;
                this.GetSystem<WorkSceneSystem>().干预者 = 干预人员.name;
                WorkSceneManager.Instance.干预者 = 干预人员.name;
                WorkSceneManager.Instance.加载提示("干预人员已确认").Forget();
                return;
            }
        }
    }
}
