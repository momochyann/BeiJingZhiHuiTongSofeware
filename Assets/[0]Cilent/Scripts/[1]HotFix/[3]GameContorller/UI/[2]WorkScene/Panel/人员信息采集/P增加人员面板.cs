using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using Michsky.MUIP;
using System;


public class P增加人员面板 : PopPanelBase
{
    // Start is called before the first frame update
    [必填InputField("姓名不能为空")]
    TMP_InputField 姓名输入框;
    [必填InputField("请选择出生日期")]
    TMP_Text 出生日期输入框;
    Button 出生日期选择按钮;
    滚轮万年历管理器 _calendarInstance;
    ToggleColumn 性别选择;
    ToggleColumn 状态选择;
    CustomDropdown 应激事件属性下拉框;
    CustomDropdown 部门选择下拉框;
    ICan2List 旧数据;
    protected override void Awake()
    {
        base.Awake();
        姓名输入框 = 弹出页面.transform.Find("姓名栏/输入框").GetComponent<TMP_InputField>();
        // 设置姓名输入框的黑色光标样式
        姓名输入框.caretColor = Color.black;
        部门选择下拉框 = 弹出页面.transform.Find("部门栏/下拉选择框").GetComponent<CustomDropdown>();
        出生日期输入框 = 弹出页面.transform.Find("出生日期栏/背景/日期输入框").GetComponent<TMP_Text>();
        
        // --- 调试步骤 1: 检查按钮是否被找到 ---
        出生日期选择按钮 = 弹出页面.transform.Find("出生日期栏/背景/选择日期按钮").GetComponent<Button>();
        if (出生日期选择按钮 == null)
        {
            Debug.LogError("[调试] 错误：未能找到 '选择日期按钮'！请检查预制体中的路径 '出生日期栏/背景/选择日期按钮' 是否正确。");
        }
        else
        {
            Debug.Log("[调试] 成功找到 '选择日期按钮'，正在为其添加监听器...");
            出生日期选择按钮.onClick.AddListener(On选择日期按钮点击);
        }
        // --- 调试结束 ---

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
            .GroupBy(部门 => 部门.单位名称 ?? "未分类")
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
                部门选择下拉框.CreateNewItem(部门.单位名称 +"-"+ 部门.部门名称);
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
        姓名输入框.text = 应激事件消息.name;
        Debug.Log(应激事件消息.dateOfBirth);
        出生日期输入框.text = 应激事件消息.dateOfBirth;
 
        // 查找包含该部门名称的完整项目名称（格式：级别-部门名称）
        int 部门索引 = 部门选择下拉框.items.FindIndex(item => item.itemName.EndsWith("-" + 应激事件消息.category));
        if (部门索引 == -1)
        {
            // 如果没找到带前缀的，直接查找部门名称
            部门索引 = 部门选择下拉框.items.FindIndex(item => item.itemName == 应激事件消息.category);
        }
        部门选择下拉框.ChangeDropdownInfo(部门索引);
        Debug.Log("部门索引" + 部门索引);
        
        性别选择.currentIndex = 应激事件消息.gender == "男" ? 0 : 1;
        状态选择.currentIndex = (int)应激事件消息.personalCrisisEventMessageFlag;
        //   应激事件属性下拉框.value = 应激事件属性下拉框.options.FindIndex(选项 => 选项.text == 应激事件消息.personalCrisisEventProperty.eventDescription);
        旧数据 = ICan2List;
    }
    async void On选择日期按钮点击()
    {
        // --- 调试步骤 2: 检查监听器是否被触发 ---
        Debug.Log("[调试] 成功进入 On选择日期按钮点击() 方法！现在开始加载万年历...");
        // --- 调试结束 ---
        
        if (_calendarInstance == null)
        {
            Debug.Log("[调试] _calendarInstance 为空，开始加载预制体 '万年历'...");
            var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("万年历");
            if (pfb == null)
            {
                Debug.LogError("[调试] 错误：万年历预制体加载失败！请检查资源包和名称是否正确。");
                return;
            }
            Debug.Log("[调试] 预制体加载成功，开始实例化...");

            var go = Instantiate(pfb, 弹出页面.transform.parent);
            if (go == null)
            {
                Debug.LogError("[调试] 错误：实例化预制体失败！");
                return;
            }
            Debug.Log("[调试] 预制体实例化成功，开始获取 滚轮万年历管理器 组件...");
            
            _calendarInstance = go.GetComponent<滚轮万年历管理器>();

            if (_calendarInstance == null)
            {
                Debug.LogError("[调试] 错误：在实例化的预制体上找不到 滚轮万年历管理器 组件！");
                return;
            }
            Debug.Log("[调试] 成功获取 滚轮万年历管理器 组件。");

            // --- 核心修复：精确等待到UI布局计算完成 ---
            // 目的：默认的 UniTask.Yield() 只等到Update。但UI布局在LateUpdate才完成。
            // 我们必须等待到 LateUpdate 之后，以确保所有UI尺寸（如rect.height）都已计算完毕，避免后续计算出错。
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        }

        // --- 核心修复：先激活UI，再进行数据操作 ---
        // 确保万年历对象在设置数据前是激活的，这样它上面的脚本才能正常启动协程。
        _calendarInstance.显示万年历();

        Debug.Log("[调试] 开始设置万年历初始日期...");
        // 尝试从输入框解析当前日期，如果无效则使用今天
        if (DateTime.TryParse(出生日期输入框.text, out DateTime currentDate))
        {
            _calendarInstance.设置选择日期(currentDate);
        }
        else
        {
            _calendarInstance.设置选择日期(DateTime.Now);
        }
        Debug.Log("[调试] 日期设置完毕，开始订阅 On日期确认 事件...");

        _calendarInstance.On日期确认 = null; 
        _calendarInstance.On日期确认 += (selectedDate) =>
        {
            出生日期输入框.text = selectedDate.ToString("yyyy-MM-dd");
            _calendarInstance.隐藏万年历();
        };
        Debug.Log("[调试] 事件订阅完毕。");
    }
}
