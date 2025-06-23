using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
public class SubjectiveAssessmentArchiveStartPanel3 : PanelBase
{
    // Start is called before the first frame update
    public GameObject 调级面板;
  //  [SerializeField] ToggleColumn[] 选择项;
    protected override void Start()
    {
        base.Start();
        
    }

    protected override void 返回按钮监听Virtual()
    {
        base.返回按钮监听Virtual();
        Destroy(gameObject);
    }

    protected override void 下一步按钮监听Virtual()
    {
        // 显示确认提示弹窗
        显示确认提示弹窗().Forget();
    }

    /// <summary>
    /// 异步显示确认提示弹窗
    /// </summary>
    async UniTaskVoid 显示确认提示弹窗()
    {
        // 异步加载确认提示弹窗预制体
        var 提示面板pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("确认提示弹窗");

        // 在Canvas下实例化提示面板
        var P_TipPanel = Instantiate(提示面板pfb, FindObjectOfType<Canvas>().transform);

        // 设置提示面板显示的文本内容
        P_TipPanel.GetComponent<P_TipPanel>().显示面板("确认保存主观评估数据？");

        // 为提示面板的确认按钮添加回调方法
        P_TipPanel.GetComponent<P_TipPanel>().确认回调 += 确认保存评估数据;
        
    }

    /// <summary>
    /// 确认保存评估数据的回调方法
    /// </summary>
    void 确认保存评估数据()
    {
        收集评估数据();
        base.下一步按钮监听Virtual();
    }

    void 收集评估数据()
    {
        
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
        评估系统.当前主观评估.createDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.SendCommand(new AddEntryCommand(评估系统.当前主观评估,"SubjectiveAssessmentArchiveModel"));
    }
}
