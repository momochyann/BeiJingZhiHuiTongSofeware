using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;

public class P_AddPerson : PopPanelBase
{
    // Start is called before the first frame update
    InputField 姓名输入框;
    InputField 部门输入框;
    InputField 出生日期输入框;
    ToggleColumn 性别选择;
    ToggleColumn 状态选择;
    Dropdown 应激事件属性下拉框;
    ICan2List 旧数据;
    protected override void Awake()
    {
        base.Awake();
        姓名输入框 = 弹出页面.transform.Find("姓名栏/输入框").GetComponent<InputField>();
        部门输入框 = 弹出页面.transform.Find("部门栏/输入框").GetComponent<InputField>();
        出生日期输入框 = 弹出页面.transform.Find("出生日期栏/输入框").GetComponent<InputField>();
        性别选择 = 弹出页面.transform.Find("性别栏").GetComponent<ToggleColumn>();
        状态选择 = 弹出页面.transform.Find("状态栏").GetComponent<ToggleColumn>();
        应激事件属性下拉框 = 弹出页面.transform.Find("应激事件属性栏/下拉选择框").GetComponent<Dropdown>();
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存数据按钮监听);
        添加应激事件属性().Forget();
        OpenPanel();
    }

    void 保存数据按钮监听()
    {
        if (!校验输入情况())
        {
            return;
        }
        PersonalPersonnelCrisisEventMessage 应激事件消息 = new PersonalPersonnelCrisisEventMessage();
        应激事件消息.name = 姓名输入框.text;
        应激事件消息.category = 部门输入框.text;
        应激事件消息.dateOfBirth = 出生日期输入框.text;
        应激事件消息.gender = 性别选择.currentIndex == 0 ? "男" : "女";
        应激事件消息.ID = this.GetModel<PersonalPersonnelCrisisEventMessageModel>().personalPersonnelCrisisEventMessages.Count;
        应激事件消息.personalCrisisEventProperty = new PersonalCrisisEventProperty() { eventDescription = 应激事件属性下拉框.options[应激事件属性下拉框.value].text };
        应激事件消息.Description = "";
        应激事件消息.personalCrisisEventMessageFlag = (PersonalCrisisEventMessageFlag)状态选择.currentIndex;
        if (旧数据 != null)
        {
            this.SendCommand(new EditEntryCommand(旧数据, 应激事件消息 as ICan2List, "PersonalPersonnelCrisisEventMessageModel"));
        }
        else
        {
            this.SendCommand(new AddEntryCommand(应激事件消息 as ICan2List, "PersonalPersonnelCrisisEventMessageModel"));
        }
        ClosePanel();
    }
    bool 校验输入情况()
    {
        if (姓名输入框.text == "" || 部门输入框.text == "" || 出生日期输入框.text == "")
        {
            Debug.LogError("输入框不能为空");
            return false;
        }
        if (性别选择.currentIndex == -1 || 状态选择.currentIndex == -1)
        {
            Debug.LogError("性别或状态选择不能为空");
            return false;
        }
        if (!System.DateTime.TryParseExact(
            出生日期输入框.text,
            "yyyy-MM-dd",
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out _))
        {
            Debug.LogError("出生日期格式不正确或日期无效，请使用YYYY-MM-DD格式输入有效日期");
            return false;
        }
        return true;
    }
    async UniTaskVoid 添加应激事件属性()
    {
        var 应激事件属性配置 = await this.GetModel<YooAssetPfbModel>().LoadConfig<PersonalCrisisEventPropertyConfig>("PersonalCrisisEventPropertyConfig");
        应激事件属性下拉框.AddOptions(应激事件属性配置.personalCrisisEventProperties
            .Select(属性 => 属性.eventDescription)
            .ToList());
    }
    public override void 编辑条目(ICan2List ICan2List)
    {
        var 应激事件消息 = ICan2List as PersonalPersonnelCrisisEventMessage;
        Debug.Log("编辑条目" + ICan2List);
        姓名输入框.text = 应激事件消息.name;
        部门输入框.text = 应激事件消息.category;
        出生日期输入框.text = 应激事件消息.dateOfBirth;
        性别选择.currentIndex = 应激事件消息.gender == "男" ? 0 : 1;
        状态选择.currentIndex = (int)应激事件消息.personalCrisisEventMessageFlag;
        应激事件属性下拉框.value = 应激事件属性下拉框.options.FindIndex(选项 => 选项.text == 应激事件消息.personalCrisisEventProperty.eventDescription);
        旧数据 = ICan2List;
    }
    void Update()
    {

    }
}
