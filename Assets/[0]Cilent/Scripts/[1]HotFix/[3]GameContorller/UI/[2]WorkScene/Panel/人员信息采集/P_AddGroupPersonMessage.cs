using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Linq;
public class P_AddGroupPersonMessage : PopPanelBase
{
    [必填InputField("姓名不能为空")]
    InputField 姓名输入框;
    [必填InputField("部门不能为空")]
    InputField 部门输入框;
    [必填InputField("出生日期不能为空")]
    InputField 接触时间框;
    InputField 备注输入框;
    InputField 事件描述输入框;
    ToggleColumn 性别选择;
    [SerializeField] Dropdown 团体事件选择下拉框;
    [SerializeField] Dropdown 受影响人员级别选择下拉框;
    [SerializeField] Button 切换面板按钮;
    [SerializeField] String 目标面板名称;
    GroupCrisisIncidentModel 团体事件数据;
    GroupCrisisIncident groupCrisisIncident => 团体事件数据.groupCrisisIncidents[团体事件选择下拉框.value];
    protected override void Awake()
    {
        base.Awake();
        姓名输入框 = 弹出页面.transform.Find("姓名栏/输入框").GetComponent<InputField>();
        部门输入框 = 弹出页面.transform.Find("部门栏/输入框").GetComponent<InputField>();
        接触时间框 = 弹出页面.transform.Find("接触时间栏/输入框").GetComponent<InputField>();
        备注输入框 = 弹出页面.transform.Find("备注栏/输入框").GetComponent<InputField>();
        事件描述输入框 = 弹出页面.transform.Find("事件描述栏/输入框").GetComponent<InputField>();
        性别选择 = 弹出页面.transform.Find("性别栏").GetComponent<ToggleColumn>();
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存数据按钮监听);
        团体事件数据 = this.GetModel<GroupCrisisIncidentModel>();
        团体事件选择下拉框.onValueChanged.AddListener(团体事件选择下拉框值改变);
        填充下拉框();
        OpenPanel();
    }

    void 填充下拉框()
    {
        团体事件选择下拉框.options.Clear();
        团体事件选择下拉框.AddOptions(团体事件数据.groupCrisisIncidents
         .Select(事件 => 事件.incidentName)
         .ToList());
        团体事件选择下拉框.options.Insert(团体事件选择下拉框.options.Count, new Dropdown.OptionData("-------"));
        填充事件等级下拉框();
    }
    void 填充事件等级下拉框()
    {
        受影响人员级别选择下拉框.value = 0;
        受影响人员级别选择下拉框.options.Clear();

        // 创建字符串列表
        List<string> 级别列表 = new List<string>();
        foreach (var 键值对 in groupCrisisIncident.affectedLevel.affectedLevel)
        {
            级别列表.Add(键值对.Key + "(" + 键值对.Value + ")");
        }
        级别列表.Add("-------");
        // 使用字符串列表添加选项
        受影响人员级别选择下拉框.AddOptions(级别列表);
    }
    private void 团体事件选择下拉框值改变(int index)
    {
        填充事件等级下拉框();
    }
    private void 保存数据按钮监听()
    {
        if (!验证输入情况())
        {
            return;
        }
        var message = new GroupPersonnelCrisisEventMessage();
        message.groupCrisisIncident = groupCrisisIncident;
        message.name = 姓名输入框.text;
        message.category = 部门输入框.text;
        message.EventContactTime = 接触时间框.text;
        message.notes = 备注输入框.text;
        message.gender = 性别选择.currentIndex == 0 ? "男" : "女";
        message.affectedLevelIndex = 受影响人员级别选择下拉框.value;
        message.Description = 事件描述输入框.text;
        message.EventContactProcess = "无";
        message.focusOfTheWork = "无";
        message.ID = this.GetSystem<GetCan2ListModelByStringSystem>().GetModel<IListModel>("GroupPersonnelCrisisEventMessageModel").GetAllItems().Count;
        this.SendCommand(new AddEntryCommand(message, "GroupPersonnelCrisisEventMessageModel"));
        ClosePanel();
    }
    public override void 编辑条目(ICan2List ICan2List)
    {
        base.编辑条目(ICan2List);
        // var 应激事件消息 = ICan2List as PersonalPersonnelCrisisEventMessage;
        // Debug.Log("编辑条目" + ICan2List);
        // 姓名输入框.text = 应激事件消息.name;
        // 部门输入框.text = 应激事件消息.category;
        // 出生日期输入框.text = 应激事件消息.dateOfBirth;
        // 性别选择.currentIndex = 应激事件消息.gender == "男" ? 0 : 1;
        // 状态选择.currentIndex = (int)应激事件消息.personalCrisisEventMessageFlag;
        // 应激事件属性下拉框.value = 应激事件属性下拉框.options.FindIndex(选项 => 选项.text == 应激事件消息.personalCrisisEventProperty.eventDescription);
        // 旧数据 = ICan2List;
    }
    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }

        if (性别选择.currentIndex == -1)
        {
            Debug.LogError("性别或状态选择不能为空");
            return false;
        }
        // if (!System.DateTime.TryParseExact(
        //     接触时间框.text,
        //     "yyyy-MM-dd",
        //     System.Globalization.CultureInfo.InvariantCulture,
        //     System.Globalization.DateTimeStyles.None,
        //     out _))
        // {
        //     Debug.LogError("出生日期格式不正确或日期无效，请使用YYYY-MM-DD格式输入有效日期");
        //     return false;
        // }
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
