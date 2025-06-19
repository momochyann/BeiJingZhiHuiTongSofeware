using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;
public class SubjectiveAssessmentArchiveEntry : MonoBehaviour, IController, IEntry
{
    // Start is called before the first frame update
    [SerializeField]
    Toggle chooseToggle;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    TMP_Text 性别框;
    [SerializeField]
    TMP_Text 部门框;
    [SerializeField]
    TMP_Text dateText;
    [SerializeField]
    Button 生成按钮;
    [SerializeField]
    Button 查看按钮;
    [SerializeField]
    TMP_Text 干预人员文本框;
    void Start()
    {

    }



    public void DisEntry(int index)
    {
        EntryInit(index);

        // 显示基本信息
        if (EntryRawValue != null)
        {
            // 姓名
            if (nameText != null)
            {
                nameText.text = EntryRawValue.name;
            }

            // 性别
            if (性别框 != null)
            {
                性别框.text = EntryRawValue.gender;
            }

            // 部门
            if (部门框 != null)
            {
                部门框.text = EntryRawValue.category;
            }

            // 日期
            if (dateText != null)
            {
                dateText.text = EntryRawValue.createDate;
            }

            // 干预人员
            if (干预人员文本框 != null && EntryRawValue.Interveners != null)
            {
                干预人员文本框.text = EntryRawValue.Interveners;
            }

            // 更新生成按钮文本
            if (生成按钮 != null)
            {
                TMP_Text 按钮文本 = 生成按钮.GetComponentInChildren<TMP_Text>();
                生成按钮.onClick.AddListener(() =>
                {
                    if (!EntryRawValue.isCreateReport)
                    {
                        var 新条目 = EntryRawValue;
                        新条目.isCreateReport = true;
                        this.SendCommand(new EditEntryCommand(EntryRawValue, 新条目, "SubjectiveAssessmentArchiveModel"));
                        EntryRawValue.isCreateReport = true;
                        // 改变按钮颜色为灰色
                        按钮文本.text = "已生成";
                        ColorBlock 颜色 = 生成按钮.colors;
                        颜色.normalColor = new Color(0.5f, 0.5f, 0.5f);
                        按钮文本.color = new Color(0.7f, 0.7f, 0.7f);
                        生成按钮.colors = 颜色;
                        查看按钮.interactable = true;
                        // 或者禁用按钮
                        生成按钮.interactable = false;
                        WorkSceneManager.Instance.加载提示("生成成功").Forget();
                    }
                });
                if (按钮文本 != null)
                {
                    按钮文本.text = EntryRawValue.isCreateReport ? "已生成" : "生成";

                    // 可选：如果已生成，可以改变按钮颜色或禁用按钮
                    if (EntryRawValue.isCreateReport)
                    {
                        // 改变按钮颜色为灰色
                        ColorBlock 颜色 = 生成按钮.colors;
                        颜色.normalColor = new Color(0.5f, 0.5f, 0.5f);
                        生成按钮.colors = 颜色;
                        按钮文本.color = new Color(0.7f, 0.7f, 0.7f);
                        // 或者禁用按钮
                        生成按钮.interactable = false;

                    }
                }
            }

            // 设置查看按钮状态
            if (查看按钮 != null)
            {
                查看按钮.onClick.AddListener(() =>
                {
                    if (EntryRawValue.isCreateReport)
                    {
                        查看按钮监听().Forget();
                    }
                });
                // 如果报告已生成，启用查看按钮；否则禁用
                查看按钮.interactable = EntryRawValue.isCreateReport;

                // 可选：如果未生成报告，可以隐藏查看按钮
                // 查看按钮.gameObject.SetActive(EntryRawValue.isCreateReport);
            }
        }
        else
        {
            Debug.LogWarning("EntryRawValue 为空，无法显示数据");
        }
    }
    async UniTaskVoid 查看按钮监听()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        var pfb = await model.LoadPfb("主观评估档案查看");
        GameObject 界面生成节点 = GameObject.Find("界面生成节点");
        Instantiate(pfb, 界面生成节点.transform).GetComponent<SubjectiveAssessmentArchiveDisPanel>().OpenPanel(EntryRawValue);
        Destroy(gameObject);
    }
    public bool IsChoose { get => chooseToggle.isOn; set => chooseToggle.isOn = value; }
    public ICan2List can2ListValue { get => _can2List; set => _can2List = value; }
    ICan2List _can2List;
    SubjectiveAssessmentArchive EntryRawValue;
    void EntryInit(int index)
    {
        var model = this.GetModel<SubjectiveAssessmentArchiveModel>();
        var message = model.subjectiveAssessmentArchives[index];
        EntryRawValue = message;
        _can2List = message;
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
