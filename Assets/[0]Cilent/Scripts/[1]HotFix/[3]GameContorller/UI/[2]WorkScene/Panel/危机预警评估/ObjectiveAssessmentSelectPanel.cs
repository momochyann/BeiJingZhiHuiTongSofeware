using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class ObjectiveAssessmentSelectPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField]
    Button 开始评估按钮;
    [SerializeField]
    Text 已选择人员;
    [SerializeField]
    Text 已选择量表;
    [SerializeField]
    Text 主干预老师;
    [SerializeField]
    Button 量表确定按钮;
    ObjectiveAssessment 当前量表;
    EntryDisPanelNew[] entryDisPanels;
    PersonalPersonnelCrisisEventMessage 当前人员;
    private IEntry[] currentEntries;
    private bool isWatching = false;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);
        量表确定按钮.onClick.AddListener(量表确定按钮监听);

        已选择人员.text = "未选择";
        已选择量表.text = "未选择";
        Init().Forget();
        
        // 启动entries监控
        WatchEntries().Forget();
    }
    async UniTaskVoid Init()
    {
        await UniTask.Delay(10);
        entryDisPanels = FindObjectsOfType<EntryDisPanelNew>();
        Debug.Log("entryDisPanel.count: " + entryDisPanels.Length);
        主干预老师.text = this.GetSystem<WorkSceneSystem>().干预者;
        
        // 延迟初始化Toggle监听
        await UniTask.Delay(500,cancellationToken: this.GetCancellationTokenOnDestroy());
        选择人员();
    }
    
    async UniTaskVoid WatchEntries()
    {
        isWatching = true;
        var cancellationToken = this.GetCancellationTokenOnDestroy();
        
        while (isWatching && !cancellationToken.IsCancellationRequested)
        {
            await UniTask.Delay(200, cancellationToken: cancellationToken); // 每0.2秒检测一次
            
            if (currentEntries != null)
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
        if (当前人员 == null || 当前量表 == null)
        {
            return;
        }
        开始评估().Forget();

    }

    async UniTaskVoid 开始评估()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("3-4-2-1_客观评估页面");
        var objectiveAssessmentStartPanel = Instantiate(pfb, transform.parent.parent);
        this.GetSystem<ObjectiveSelectSystem>().当前人员 = 当前人员;
        this.GetSystem<ObjectiveSelectSystem>().当前量表 = 当前量表;
        this.GetSystem<ObjectiveSelectSystem>().当前题序 = 0;
        this.GetSystem<ObjectiveSelectSystem>().当前量表得分 = new List<int>();
        
        Destroy(gameObject.transform.parent.gameObject);
    }
    void 选择人员()
    {
        foreach (EntryDisPanelNew entryDisPanel in entryDisPanels)
        {
            Debug.Log("entryDisPanel.gameObject.name: " + entryDisPanel.gameObject.name);

            if (entryDisPanel.gameObject.name != "危机评估选择面板")
            {
                continue;
            }
            
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
                                // 单选逻辑：清除其他选择
                                foreach (IEntry otherEntry in currentEntries)
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
                            else if (!isOn)
                            {
                                已选择人员.text = "未选择";
                                当前人员 = null;
                                WorkSceneManager.Instance.加载提示("人员去除成功").Forget();
                            }
                        });
                }
            }
            }
            break; // 找到目标面板后退出循环
        }
    }
    void 量表确定按钮监听()
    {
        foreach (EntryDisPanelNew entryDisPanel in entryDisPanels)
        {
            if (entryDisPanel.gameObject.name != "个人信息管理面板")
            {
                continue;
            }
            IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
            foreach (IEntry entry in entries)
            {
                if (entry.IsChoose)
                {
                    ObjectiveAssessment objectiveAssessment = entry.can2ListValue as ObjectiveAssessment;
                    已选择量表.text = objectiveAssessment.量表名称;
                    当前量表 = objectiveAssessment;
                    WorkSceneManager.Instance.加载提示("量表选择成功").Forget();
                    return;
                }
            }
        }
    }

    void OnDestroy()
    {
        isWatching = false;
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
