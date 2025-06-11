using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using Michsky.MUIP;
using UnityEditor.Android;


public class P增加人员面板 : PopPanelBase
{
    // Start is called before the first frame update
    [必填InputField("姓名不能为空")]
    TMP_InputField 姓名输入框;
    [必填InputField("请选择出生日期"), SerializeField]
    TMP_InputField 出生日期输入框;
    ToggleColumn 性别选择;
    ToggleColumn 状态选择;
    CustomDropdown 应激事件属性下拉框;
    CustomDropdown 部门选择下拉框;
    ICan2List 旧数据;
    protected override void Awake()
    {
        base.Awake();
        姓名输入框 = 弹出页面.transform.Find("姓名栏/输入框").GetComponent<TMP_InputField>();
        部门选择下拉框 = 弹出页面.transform.Find("部门栏/下拉选择框").GetComponent<CustomDropdown>();
        //  出生日期输入框 = 弹出页面.transform.Find("出生日期栏/输入框").GetComponent<TMP_InputField>();
        性别选择 = 弹出页面.transform.Find("性别栏").GetComponent<ToggleColumn>();
        状态选择 = 弹出页面.transform.Find("状态栏").GetComponent<ToggleColumn>();
        应激事件属性下拉框 = 弹出页面.transform.Find("应激事件属性栏/下拉选择框").GetComponent<CustomDropdown>();
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存数据按钮监听);
        添加应激事件属性().Forget();
        添加部门选项();
        OpenPanel();
    }
    void 添加部门选项()
    {
        var 部门数据 = this.GetModel<部门数据Model>().部门列表;
        
        // 按级别分组
        var 分组后部门列表 = 部门数据
            .GroupBy(部门 => 部门.部门级别 ?? "未分类")
            .OrderBy(组 => 组.Key)
            .ToList();
            
        // 清空现有选项
        部门选择下拉框.items.Clear();
        
        // 添加分组后的部门
        foreach (var 组 in 分组后部门列表)
        { 
            // 添加该组的所有部门
            foreach (var 部门 in 组.OrderBy(p => p.部门名称))
            {
                部门选择下拉框.CreateNewItem(部门.部门级别 +"-"+ 部门.部门名称);
            }
        }
        
        if (部门选择下拉框.items.Count > 0)
        {
            部门选择下拉框.ChangeDropdownInfo(0);
            部门选择下拉框.SetupDropdown();
        }
        else
        {
            部门选择下拉框.CreateNewItem("待填入部门");
        }
    }
    void 保存数据按钮监听()
    {
        if (!验证输入情况())
        {
            return;
        }
        PersonalPersonnelCrisisEventMessage 应激事件消息 = new PersonalPersonnelCrisisEventMessage();
        
        应激事件消息.name = 姓名输入框.text;
        
        // 获取选中的项目名称
        var 选中项目 = 部门选择下拉框.items[部门选择下拉框.selectedItemIndex].itemName;
        
        // 提取"-"后面的文字
        string 部门名称 = 选中项目;
        if (选中项目.Contains("-"))
        {
            int dashIndex = 选中项目.LastIndexOf('-');
            if (dashIndex >= 0 && dashIndex < 选中项目.Length - 1)
            {
                部门名称 = 选中项目.Substring(dashIndex + 1).Trim();
            }
        }
        
        应激事件消息.category = 部门名称;
        应激事件消息.dateOfBirth = 出生日期输入框.text;
        应激事件消息.gender = 性别选择.currentIndex == 0 ? "男" : "女";
        应激事件消息.ID = this.GetModel<PersonalPersonnelCrisisEventMessageModel>().personalPersonnelCrisisEventMessages.Count;
        应激事件消息.personalCrisisEventProperty = new PersonalCrisisEventProperty() { eventDescription = 应激事件属性下拉框.items[应激事件属性下拉框.selectedItemIndex].itemName };
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

    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }

        if (性别选择.currentIndex == -1 || 状态选择.currentIndex == -1)
        {
            WorkSceneManager.Instance.加载提示("性别或状态选择不能为空").Forget();
            return false;
        }
        // if (!System.DateTime.TryParseExact(
        //     出生日期输入框.text,
        //     "yyyy-MM-dd",
        //     System.Globalization.CultureInfo.InvariantCulture,
        //     System.Globalization.DateTimeStyles.None,
        //     out _))
        // {
        //     WorkSceneManager.Instance.加载提示("出生日期格式不正确或日期无效，请使用YYYY-MM-DD格式输入有效日期").Forget();
        //     return false;
        // }
        return true;
    }
    async UniTaskVoid 添加应激事件属性()
    {
        var 应激事件属性配置 = await this.GetModel<YooAssetPfbModel>().LoadConfig<PersonalCrisisEventPropertyConfig>("PersonalCrisisEventPropertyConfig");
        foreach (var 属性 in 应激事件属性配置.personalCrisisEventProperties)
        {
            Debug.Log("添加应激事件属性" + 属性.eventDescription);
            应激事件属性下拉框.CreateNewItem(属性.eventDescription);
        }
        应激事件属性下拉框.ChangeDropdownInfo(3);
        应激事件属性下拉框.SetupDropdown();
        // 应激事件属性下拉框.AddOptions(应激事件属性配置.personalCrisisEventProperties
        //     .Select(属性 => 属性.eventDescription)
        //     .ToList());
    }
    public override void 编辑条目(ICan2List ICan2List)
    {
        var 应激事件消息 = ICan2List as PersonalPersonnelCrisisEventMessage;
        Debug.Log("编辑条目" + ICan2List);
        ;
        性别选择.currentIndex = 应激事件消息.gender == "男" ? 0 : 1;
        状态选择.currentIndex = (int)应激事件消息.personalCrisisEventMessageFlag;
        //   应激事件属性下拉框.value = 应激事件属性下拉框.options.FindIndex(选项 => 选项.text == 应激事件消息.personalCrisisEventProperty.eventDescription);
        旧数据 = ICan2List;
    }
    void Update()
    {

    }
}
