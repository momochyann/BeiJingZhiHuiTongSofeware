using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
public class ObjectiveAssessmentPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField]
    Text 量表名称;
    [SerializeField]
    Text 量表简介;
    [SerializeField]
    Button 开始评估按钮;

    void Start()
    {
        开始评估按钮.onClick.AddListener(OnStartAssessmentButtonClick);
        var 量表列表 = this.GetSystem<ObjectiveSelectSystem>().当前量表;
        量表名称.text = 量表列表.量表名称;
        量表简介.text = 量表列表.量表简介;
        //this.get
    }


    void OnStartAssessmentButtonClick()
    {
        Debug.Log("开始评估按钮点击");
        LoadObjectiveAssessmentPage().Forget();
    }
    async UniTaskVoid LoadObjectiveAssessmentPage()
    {
        var 客观评估页面 = await this.GetModel<YooAssetPfbModel>().LoadPfb("3-4-2-1-1_开始客观评估");
        var 客观评估页面实例 = Instantiate(客观评估页面, this.transform.parent);
        Destroy(this.gameObject);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
