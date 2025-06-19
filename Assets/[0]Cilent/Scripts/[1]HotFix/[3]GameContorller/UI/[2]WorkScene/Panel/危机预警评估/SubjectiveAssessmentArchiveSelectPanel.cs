using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using TMPro;
public class SubjectiveAssessmentArchiveSelectPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 开始评估按钮;
    [SerializeField] TMP_Text 已选择人员;
    [SerializeField] TMP_Text 主干预老师;
    PersonalPersonnelCrisisEventMessage 当前人员;
    EntryDisPanelNew entryDisPanel;
    private IEntry[] currentEntries;
    private bool isWatching = false;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);
        if(PlayerPrefs.GetString("当前干预人员")=="")
        {
            主干预老师.text = this.GetSystem<WorkSceneSystem>().干预者==""? "未选择干预者": this.GetSystem<WorkSceneSystem>().干预者;
        }else
        {
            主干预老师.text = PlayerPrefs.GetString("当前干预人员");
        }
        
        
        // // 延迟初始化Toggle监听
        DelayedInit().Forget();
        
        // 启动entries监控
        WatchEntries().Forget();
    }
    
    async UniTaskVoid DelayedInit()
    {
        await UniTask.Delay(500); // 0.5秒
        选择人员();
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
            return;
        }
        开始评估().Forget();

    }
    void 选择人员()
    {
        if (entryDisPanel == null)
        {
            entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        }
        
        if (entryDisPanel == null) return;
        
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        currentEntries = entries; // 保存当前entries引用用于监控
        foreach (IEntry entry in entries)
        {
            if (entry is MonoBehaviour entryMono)
            {
                Toggle toggle = entryMono.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    // 清除旧的监听器，避免重复绑定
                    toggle.onValueChanged.RemoveAllListeners();
                    
                    toggle.onValueChanged.AddListener((bool isOn) => {
                        PersonalPersonnelCrisisEventMessage personalPersonnelCrisisEventMessage = entry.can2ListValue as PersonalPersonnelCrisisEventMessage;
                        
                        if (isOn && entry.IsChoose)
                        {
                            foreach (IEntry otherEntry in currentEntries)
                            {
                                if (otherEntry != entry && otherEntry.IsChoose)
                                {
                                    otherEntry.IsChoose = false;
                                }
                            }
                            if(已选择人员.text == "待选择")
                            {
                                已选择人员.text = personalPersonnelCrisisEventMessage.name;
                            }
                            else 
                            {
                                if(已选择人员.text != personalPersonnelCrisisEventMessage.name)
                                {
                                    
                                    已选择人员.text = personalPersonnelCrisisEventMessage.name;
                                }
                            }
                            当前人员 = personalPersonnelCrisisEventMessage;
                            WorkSceneManager.Instance.加载提示("人员选择成功").Forget();
                        }else
                        {
                            
                            if(已选择人员.text.Length - personalPersonnelCrisisEventMessage.name.Length - 1 < 0)已选择人员.text = "待选择";
                            else 已选择人员.text = 已选择人员.text.Remove(已选择人员.text.Length - personalPersonnelCrisisEventMessage.name.Length - 1);
                            WorkSceneManager.Instance.加载提示("人员去除成功").Forget();
                        }
                    });
                }
            }
        }
    }
    async UniTaskVoid 开始评估()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        Debug.Log("开始评估");
        var pfb = await model.LoadPfb("3-4-1-1评估页面");
        var subjectiveAssessmentStartPanel = Instantiate(pfb, transform.parent.parent);
        this.GetSystem<ObjectiveSelectSystem>().当前人员 = 当前人员;
        var subjectiveAssessmentArchive = new SubjectiveAssessmentArchive();
        this.GetSystem<ObjectiveSelectSystem>().当前主观评估 = subjectiveAssessmentArchive;
        subjectiveAssessmentArchive.name = 当前人员.name;
        subjectiveAssessmentArchive.gender = 当前人员.gender;
        subjectiveAssessmentArchive.category = 当前人员.category;
   
        

        Destroy(gameObject.transform.parent.gameObject);
    }
    // Update is called once per frame
    void OnDestroy()
    {
        isWatching = false;
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
