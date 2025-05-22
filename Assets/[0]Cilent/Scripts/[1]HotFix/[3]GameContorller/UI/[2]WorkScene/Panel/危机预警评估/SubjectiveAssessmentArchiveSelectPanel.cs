using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class SubjectiveAssessmentArchiveSelectPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 开始评估按钮;
    [SerializeField] Text 已选择人员;
    [SerializeField] Text 主干预老师;
    [SerializeField] Button 人员确定按钮;
    PersonalPersonnelCrisisEventMessage 当前人员;
    EntryDisPanelNew entryDisPanel;
    void Start()
    {
        开始评估按钮.onClick.AddListener(开始评估按钮监听);
        人员确定按钮.onClick.AddListener(人员确定按钮监听);
        主干预老师.text = this.GetSystem<WorkSceneSystem>().干预者;
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
                WorkSceneManager.Instance.加载提示("人员选择成功").Forget();
                return;
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
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
