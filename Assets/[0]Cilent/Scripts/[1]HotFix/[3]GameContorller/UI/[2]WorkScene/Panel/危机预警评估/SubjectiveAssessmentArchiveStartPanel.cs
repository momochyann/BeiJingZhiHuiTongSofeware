using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class SubjectiveAssessmentArchiveStartPanel : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] ButtonColumn[] buttonColumns;
    [SerializeField] Button 下一步按钮;
    [SerializeField] Button 取消按钮;
    [SerializeField] Button 返回按钮;
    [SerializeField] InputField 描述输入框;
    void Start()
    {
        buttonColumns = FindObjectsOfType<ButtonColumn>();
        下一步按钮.onClick.AddListener(下一步按钮监听);
        取消按钮.onClick.AddListener(取消按钮监听);
        返回按钮.onClick.AddListener(返回按钮监听);
    }
    void 下一步按钮监听()
    {
        if (描述输入框.text == "")
        {
            return;
        }
        下一步().Forget();

    }
    async UniTaskVoid 下一步()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("3-4-1-1-1评估页面2");
        var subjectiveAssessmentStartPanel2 = Instantiate(pfb, transform.parent);
        var 录入系统 = this.GetSystem<ObjectiveSelectSystem>();
        录入系统.当前主观评估.stressEventDescription = 描述输入框.text;
        录入系统.当前主观评估.personnelStateEvaluation = new PersonnelState();
        录入系统.当前主观评估.personnelStateEvaluation.personnelState = new Dictionary<string, string>();
        
        // 记录已处理的键，用于检查重复
        HashSet<string> 已处理键集合 = new HashSet<string>();
        
        foreach (var buttonColumn in buttonColumns)
        {
            string 键 = buttonColumn.组描述;
            string 值 = buttonColumn.buttonList[buttonColumn.currentIndex].GetComponentInChildren<Text>().text;
            
            // 检查键是否为空
            if (string.IsNullOrEmpty(键))
            {
                Debug.LogWarning("发现空键，已跳过");
                continue;
            }
            
            // 检查键是否重复
            if (已处理键集合.Contains(键))
            {
                Debug.LogWarning($"存在重复的键: {键}，将覆盖之前的值");
            }
            已处理键集合.Add(键);
            
            // 检查键是否已存在于字典中
            if (录入系统.当前主观评估.personnelStateEvaluation.personnelState.ContainsKey(键))
            {
                Debug.LogWarning($"主观评估中已存在键: {键}，将覆盖之前的值");
                录入系统.当前主观评估.personnelStateEvaluation.personnelState[键] = 值;
            }
            else
            {
                录入系统.当前主观评估.personnelStateEvaluation.personnelState.Add(键, 值);
                Debug.Log($"已添加评估项: {键} = {值}");
            }
        }
        
        // 输出收集到的评估数据总数
        Debug.Log($"共收集到 {录入系统.当前主观评估.personnelStateEvaluation.personnelState.Count} 项评估数据");
        
        Destroy(gameObject);
    }
    void 取消按钮监听()
    {
        
    }
    void 返回按钮监听()
    {

    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
