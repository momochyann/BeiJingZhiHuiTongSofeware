using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
public class SubjectiveAssessmentArchiveStartPanel3 : PanelBase
{
    // Start is called before the first frame update
    public GameObject 调级面板;
    [SerializeField] ToggleColumn[] 选择项;
    void Start()
    {

    }

    protected override void 返回按钮监听Virtual()
    {
        Destroy(gameObject);
    }
    protected override void 下一步按钮监听Virtual()
    {
        下一步().Forget();
    }
    async UniTaskVoid 下一步()
    {
        // 收集调级面板中的评估数据
        收集评估数据();
        
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("3-4-1-3评估页面");
        var subjectiveAssessmentStartPanel3 = Instantiate(pfb, transform.parent.parent);
        Destroy(gameObject);
    }

    void 收集评估数据()
    {
        // 获取调级面板
        GameObject 调级面板 = FindObjectOfType<SubjectiveAssessmentArchiveStartPanel3>().调级面板;
        
        // 获取ObjectiveSelectSystem
        var 评估系统 = this.GetSystem<ObjectiveSelectSystem>();
        
        // 确保personnelStateEvaluation已初始化
        if (评估系统.当前主观评估.personnelStateEvaluation == null)
        {
            评估系统.当前主观评估.personnelStateEvaluation = new PersonnelState();
        }
        
        // 确保personnelState字典已初始化
        if (评估系统.当前主观评估.personnelStateEvaluation.personnelState == null)
        {
            评估系统.当前主观评估.personnelStateEvaluation.personnelState = new Dictionary<string, string>();
        }
        
        // 遍历调级面板中的所有子物体
        for (int i = 0; i < 调级面板.transform.childCount; i++)
        {
            Transform 子物体 = 调级面板.transform.GetChild(i);
            
            // 检查该子物体是否有第一个子物体
            if (子物体.childCount > 0)
            {
                // 获取第一个子物体的Text组件（作为键）
                Text 文本组件 = 子物体.GetChild(0).GetComponent<Text>();
                
                if (文本组件 != null)
                {
                    string 键 = 文本组件.text;
                    
                    // 查找滑动条组件
                    Slider 滑动条 = 子物体.GetComponentInChildren<Slider>(true);
                    
                    if (滑动条 != null)
                    {
                        // 获取滑动条的值并转换为字符串（作为值）
                        string 值 = Mathf.RoundToInt(滑动条.value).ToString();
                        
                        // 将键值对添加到字典中（如果已存在则更新）
                        if (评估系统.当前主观评估.personnelStateEvaluation.personnelState.ContainsKey(键))
                        {
                            评估系统.当前主观评估.personnelStateEvaluation.personnelState[键] = 值;
                        }
                        else
                        {
                            评估系统.当前主观评估.personnelStateEvaluation.personnelState.Add(键, 值);
                        }
                    }
                }
            }
        }
    }
}
