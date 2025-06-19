using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
using TMPro;

public class IndividualInterventionSelectPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 开始评估按钮;
    [SerializeField] Button 情绪放松按钮;
    [SerializeField] TMP_Text 已选择人员;
    [SerializeField] TMP_Text 干预者;
    [HideInInspector] public PersonalPersonnelCrisisEventMessage 当前人员;

    EntryDisPanelNew entryDisPanel;
    private IEntry[] currentEntries;
    private bool isWatching = false;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);
        情绪放松按钮.onClick.AddListener(情绪放松按钮监听);
        干预者.text = this.GetSystem<WorkSceneSystem>().干预者;
        DelayedInit().Forget();

    }
    async UniTaskVoid DelayedInit()
    {
        await UniTask.Delay(500); // 0.5秒
        选择人员();
    }

    private async void 情绪放松按钮监听()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("情绪放松");
        var emotionalRelaxationPanel = Instantiate(pfb, transform.parent);
        Destroy(gameObject.transform.gameObject);
    }
    async UniTaskVoid WatchEntries()
    {
        isWatching = true;
        var cancellationToken = this.GetCancellationTokenOnDestroy();

        while (isWatching && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(200, cancellationToken: cancellationToken); // 每0.2秒检测一次

            if (entryDisPanel != null && currentEntries != null)
            {
                // 检测当前entries是否被销毁
                bool entriesDestroyed = false;
                foreach (IEntry entry in currentEntries)
                {
                    if (entry == null || (entry is MonoBehaviour mono && mono == null))
                    {
                        entriesDestroyed = true;
                        break;
                    }
                }

                if (entriesDestroyed)
                {
                    Debug.Log("检测到entries被销毁，重新获取并绑定");
                    await UniTask.Delay(500, cancellationToken: cancellationToken); // 等待新entries生成
                    选择人员();
                }
            }
        }
    }

    void 开始评估按钮监听()
    {
        if (当前人员 == null)
        {
            WorkSceneManager.Instance.加载提示("请选择人员").Forget();
            return;
        }
        开始干预实施().Forget();

    }
    void 选择人员()
    {
        if (entryDisPanel == null)
        {
            entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        }

        if (entryDisPanel == null) return;

        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        foreach (IEntry entry in entries)
        {
            if (entry is MonoBehaviour entryMono)
            {
                Toggle toggle = entryMono.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    // 清除旧的监听器，避免重复绑定
                    toggle.onValueChanged.RemoveAllListeners();

                    toggle.onValueChanged.AddListener((bool isOn) =>
                    {
                        PersonalPersonnelCrisisEventMessage personalPersonnelCrisisEventMessage = entry.can2ListValue as PersonalPersonnelCrisisEventMessage;

                        if (isOn && entry.IsChoose)
                        {
                            foreach (IEntry otherEntry in entries)
                            {
                                if (otherEntry != entry && otherEntry.IsChoose)
                                {
                                    otherEntry.IsChoose = false;
                                }
                            }
                            已选择人员.text = personalPersonnelCrisisEventMessage.name;
                            当前人员 = personalPersonnelCrisisEventMessage;
                            WorkSceneManager.Instance.加载提示("人员选择成功").Forget();
                        }
                        else
                        {
                            已选择人员.text = "待选择";
                            WorkSceneManager.Instance.加载提示("人员去除成功").Forget();
                        }
                    });
                }
            }
        }
    }
    [HideInInspector] public string 实施开始时间;
    async UniTaskVoid 开始干预实施()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        Debug.Log("开始干预实施");
        var pfb = await model.LoadPfb("3-5-1-1_实施干预目录");
        var subjectiveAssessmentStartPanel = Instantiate(pfb, transform.parent);
        实施开始时间 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // this.GetSystem<InterventionSystem>().当前人员 = 当前人员;
        // var individualInterventionArchive = new IndividualInterventionArchive();
        // this.GetSystem<InterventionSystem>().当前干预档案 = individualInterventionArchive;
        // this.GetSystem<InterventionSystem>().当前干预档案.name = 当前人员.name;
        // //  this.GetSystem<InterventionSystem>().当前干预档案.录音地址 = new List<string>();
        // this.GetSystem<InterventionSystem>().是否开始干预 = true;
        // // this.GetSystem<InterventionSystem>().当前干预档案.createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        // this.GetSystem<InterventionSystem>().当前干预档案.startDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // this.GetSystem<InterventionSystem>().当前干预档案.endDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //Destroy(gameObject.transform.gameObject);
    }
    // Update is called once per frame
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
public class 干预实施咨询组件
{
    public TMP_Text 问题文本框;
    public TMP_InputField 回答输入框;
    public 干预实施录制按钮 录音按钮;
}