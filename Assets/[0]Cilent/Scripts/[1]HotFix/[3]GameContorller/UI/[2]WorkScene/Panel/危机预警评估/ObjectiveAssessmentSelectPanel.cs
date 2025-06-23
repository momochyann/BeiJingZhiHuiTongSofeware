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
    ObjectiveAssessment 当前量表;
    EntryDisPanelNew[] entryDisPanels;
    PersonalPersonnelCrisisEventMessage 当前人员;
    private IEntry[] currentEntries;
    private IEntry[] currentScaleEntries; // 新增：用于监控量表条目
    private bool isWatching = false;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);

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
            
            // 检查人员条目
            if (currentEntries != null)
            {
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
                    Debug.Log("检测到人员entries被销毁，重新获取并绑定");
                    await UniTask.Delay(500, cancellationToken: cancellationToken);
                    选择人员();
                }
            }
            
            // 检查量表条目
            if (currentScaleEntries != null)
            {
                bool scaleEntriesDestroyed = false;
                foreach (IEntry entry in currentScaleEntries)
                {
                    if (entry == null || (entry is MonoBehaviour mono && mono == null))
                    {
                        scaleEntriesDestroyed = true;
                        break;
                    }
                }
                
                if (scaleEntriesDestroyed)
                {
                    Debug.Log("检测到量表entries被销毁，重新获取并绑定");
                    await UniTask.Delay(500, cancellationToken: cancellationToken);
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
        Debug.Log("开始选择人员方法，entryDisPanels数量: " + entryDisPanels.Length);
        
        foreach (EntryDisPanelNew entryDisPanel in entryDisPanels)
        {
            if (entryDisPanel == null)
            {
                Debug.LogWarning("发现null的entryDisPanel，跳过");
                continue;
            }
            
            Debug.Log("检查面板: " + entryDisPanel.gameObject.name);
            
            if (entryDisPanel.gameObject.name == "危机评估选择面板")
            {
                Debug.Log("找到危机评估选择面板，调用客观评估选择人员监听");
                客观评估选择人员监听(entryDisPanel);
                continue;
            }
            else if (entryDisPanel.gameObject.name == "评估量表选择面板")
            {
                Debug.Log("找到评估量表选择面板，调用评估量表选择监听");
                评估量表选择监听(entryDisPanel);
                continue;
            }
            else
            {
                Debug.Log("面板名称不匹配: " + entryDisPanel.gameObject.name);
            }
        }
    }
    void 客观评估选择人员监听(EntryDisPanelNew entryDisPanel)
    {
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
    }
    void 评估量表选择监听(EntryDisPanelNew entryDisPanel)
    {
        Debug.Log("进入评估量表选择监听方法");
        
        IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
        Debug.Log("找到量表条目数量: " + entries.Length);
        
        currentScaleEntries = entries;
        
        foreach (IEntry entry in entries)
        {
            if (entry is MonoBehaviour entryMono)
            {
                Toggle toggle = entryMono.GetComponentInChildren<Toggle>();
                if (toggle != null)
                {
                    Debug.Log("为量表条目绑定Toggle监听器");
                    
                    // 清除旧的监听器，避免重复绑定
                    toggle.onValueChanged.RemoveAllListeners();
                    
                    toggle.onValueChanged.AddListener((bool isOn) => {
                        Debug.Log("量表Toggle状态改变: " + isOn);
                        
                        ObjectiveAssessment objectiveAssessment = entry.can2ListValue as ObjectiveAssessment;
                        if (objectiveAssessment == null)
                        {
                            Debug.LogError("无法获取ObjectiveAssessment对象");
                            return;
                        }
                        
                        if (isOn && entry.IsChoose)
                        {
                            Debug.Log("量表被选中: " + objectiveAssessment.量表名称);
                            
                            // 单选逻辑：清除其他选择
                            foreach (IEntry otherEntry in entries)
                            {
                                if (otherEntry != entry && otherEntry.IsChoose)
                                {
                                    otherEntry.IsChoose = false;
                                }
                            }
                            
                            已选择量表.text = objectiveAssessment.量表名称;
                            当前量表 = objectiveAssessment;
                            WorkSceneManager.Instance.加载提示("量表选择成功").Forget();
                        }
                        else if (!isOn)
                        {
                            Debug.Log("量表被取消选中");
                            已选择量表.text = "未选择";
                            当前量表 = null;
                            WorkSceneManager.Instance.加载提示("量表去除成功").Forget();
                        }
                    });
                }
                else
                {
                    Debug.LogWarning("量表条目没有找到Toggle组件");
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
