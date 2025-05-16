using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
public class IndividualInterventionSelectPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 开始评估按钮;
    [SerializeField] Button 情绪放松按钮;
    [SerializeField] Text 已选择人员;
    [SerializeField] Button 人员确定按钮;
    PersonalPersonnelCrisisEventMessage 当前人员;
    EntryDisPanelNew entryDisPanel;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);
        人员确定按钮.onClick.AddListener(人员确定按钮监听);
        情绪放松按钮.onClick.AddListener(情绪放松按钮监听);
    }

    private async void 情绪放松按钮监听()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("情绪放松");
        var emotionalRelaxationPanel = Instantiate(pfb, transform.parent);
        Destroy(gameObject.transform.gameObject);
    }

    void 开始评估按钮监听()
    {
        if (当前人员 == null)
        {
            return;
        }
        开始评估().Forget();

    }
    void 人员确定按钮监听()
    {
        if (entryDisPanel == null)
        {
            entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        }
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            if (entry.IsChoose)
            {
                PersonalPersonnelCrisisEventMessage personalPersonnelCrisisEventMessage = entry.can2ListValue as PersonalPersonnelCrisisEventMessage;
                已选择人员.text = personalPersonnelCrisisEventMessage.name;
                当前人员 = personalPersonnelCrisisEventMessage;
                return;
            }
        }
    }
    async UniTaskVoid 开始评估()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        Debug.Log("开始评估");
        var pfb = await model.LoadPfb("3-5-1-1_实施干预目录");
        var subjectiveAssessmentStartPanel = Instantiate(pfb, transform.parent);
        this.GetSystem<InterventionSystem>().当前人员 = 当前人员;
        var individualInterventionArchive = new IndividualInterventionArchive();
        this.GetSystem<InterventionSystem>().当前干预档案 = individualInterventionArchive;
        Destroy(gameObject.transform.gameObject);
    }
    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
