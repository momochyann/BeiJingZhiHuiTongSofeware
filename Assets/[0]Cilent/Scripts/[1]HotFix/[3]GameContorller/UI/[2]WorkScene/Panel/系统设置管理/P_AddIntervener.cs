using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;

public class P_AddIntervener : PopPanelBase
{
    // Start is called before the first frame update
    [必填InputField("姓名不能为空")]
    InputField 姓名输入框;
    [必填InputField("密码不能为空")]
    InputField 密码输入框;
    InputField 确认密码输入框;
   // [必填InputField("邮箱不能为空")]
    InputField 邮箱输入框;
    [必填InputField("电话不能为空")]
    InputField 电话输入框;
    //[必填InputField("简介不能为空")]
    InputField 简介输入框;
    [必填InputField("用户名不能为空")]
    InputField 用户名输入框;
    ToggleColumn 性别选择;

    Dropdown 应激事件属性下拉框;
    ICan2List 旧数据;
    Text 提示信息;
    [SerializeField] Image 密码输入框背景;
    [SerializeField] Image 确认密码输入框背景;

    // 定义颜色
    private Color 正常颜色 = new Color(0.9f, 0.9f, 0.9f); // 灰色
    private Color 错误颜色 = new Color(1.0f, 0.7f, 0.7f); // 红色

    protected override void Awake()
    {
        base.Awake();
        姓名输入框 = 弹出页面.transform.Find("姓名栏/输入框").GetComponent<InputField>();
        密码输入框 = 弹出页面.transform.Find("密码栏/输入框").GetComponent<InputField>();
        确认密码输入框 = 弹出页面.transform.Find("确认密码栏/输入框").GetComponent<InputField>();
        邮箱输入框 = 弹出页面.transform.Find("邮箱栏/输入框").GetComponent<InputField>();
        电话输入框 = 弹出页面.transform.Find("电话栏/输入框").GetComponent<InputField>();
        简介输入框 = 弹出页面.transform.Find("简介栏/输入框").GetComponent<InputField>();
        用户名输入框 = 弹出页面.transform.Find("用户名栏/输入框").GetComponent<InputField>();
        性别选择 = 弹出页面.transform.Find("性别栏").GetComponent<ToggleColumn>();
        
        // 获取提示信息文本和输入框背景
        提示信息 = 弹出页面.transform.Find("提示信息").GetComponent<Text>();
      
        
        // 初始化提示信息
        提示信息.text = "";
        
        // 添加密码输入限制
        密码输入框.contentType = InputField.ContentType.Password;
        密码输入框.characterLimit = 8;  // 限制最多输入8个字符
        
        // 添加确认密码输入限制
        确认密码输入框.contentType = InputField.ContentType.Password;
        确认密码输入框.characterLimit = 8;  // 限制最多输入8个字符
        
        // 添加密码输入框的实时验证
        密码输入框.onValueChanged.AddListener(密码输入变化);
        
        // 添加确认密码输入框的实时验证
        确认密码输入框.onValueChanged.AddListener(确认密码输入变化);
        
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存数据按钮监听);
        
        OpenPanel();
    }

    void 保存数据按钮监听()
    {
        if (!验证输入情况())
        {
            return;
        }
        Intervener 干预人员 = new Intervener();
        干预人员.name = 姓名输入框.text;
        干预人员.性别 = 性别选择.currentIndex == 0 ? "男" : "女";
        干预人员.电话 = 电话输入框.text;
        干预人员.邮箱 = 邮箱输入框.text;
        干预人员.简介 = 简介输入框.text;
        干预人员.用户名 = 用户名输入框.text;
        干预人员.密码 = 密码输入框.text;
        
        if (旧数据 != null)
        {
            this.SendCommand(new EditEntryCommand(旧数据, 干预人员 as ICan2List, "IntervenersModel"));
        }
        else
        {
            this.SendCommand(new AddEntryCommand(干预人员 as ICan2List, "IntervenersModel"));
        }
        
        ClosePanel();
    }

    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }
        
        // 验证密码是否为6-8个数字
        if (!验证密码(密码输入框.text) || string.IsNullOrEmpty(密码输入框.text))
        {
            提示信息.text = "密码必须为6-8个数字";
            return false;
        }
        
        // 验证确认密码是否与密码相同
        if (密码输入框.text != 确认密码输入框.text)
        {
            提示信息.text = "确认密码与密码不一致";
            return false;
        }
        
        return true;
    }

    // 密码输入变化时的处理
    private void 密码输入变化(string 密码)
    {
        bool 密码有效 = 验证密码(密码);
        
        // 更新密码输入框背景颜色
        密码输入框背景.color = 密码有效 ? 正常颜色 : 错误颜色;
        
        // // 更新提示信息
        // if (!密码有效 && !string.IsNullOrEmpty(密码))
        // {
        //     提示信息.text = "密码必须为6-8个数字";
        // }
        // else
        // {
        //     提示信息.text = "";
        // }
        
        // 如果确认密码已经输入，也需要验证确认密码
        if (!string.IsNullOrEmpty(确认密码输入框.text))
        {
            确认密码输入变化(确认密码输入框.text);
        }
    }

    // 确认密码输入变化时的处理
    private void 确认密码输入变化(string 确认密码)
    {
        bool 密码匹配 = 密码输入框.text == 确认密码;
        
        // 更新确认密码输入框背景颜色
        确认密码输入框背景.color = 密码匹配 ? 正常颜色 : 错误颜色;
        
        // 更新提示信息
        // if (!密码匹配 && !string.IsNullOrEmpty(确认密码))
        // {
        //     提示信息.text = "确认密码与密码不一致";
        // }
        // else if (string.IsNullOrEmpty(提示信息.text)) // 如果没有其他错误提示
        // {
        //     提示信息.text = "";
        // }
    }
    protected override void 验证失败处理(string 错误提示)
    {
        提示信息.text = 错误提示;
    }
    // 验证密码是否为6-8个数字
    private bool 验证密码(string 密码)
    {
        // 空密码不显示错误
        if (string.IsNullOrEmpty(密码))
        {
            return true;
        }
        
        // 检查密码长度是否在6-8之间
        if (密码.Length < 6 || 密码.Length > 8)
        {
            return false;
        }
        
        // 检查密码是否只包含数字
        foreach (char c in 密码)
        {
            if (!char.IsDigit(c))
            {
                return false;
            }
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
        var 干预人员 = ICan2List as Intervener;
        Debug.Log("编辑条目" + ICan2List);
        用户名输入框.text = 干预人员.用户名;
        姓名输入框.text = 干预人员.name;
        密码输入框.text = 干预人员.密码;
        确认密码输入框.text = 干预人员.密码;
        邮箱输入框.text = 干预人员.邮箱;
        性别选择.currentIndex = 干预人员.性别 == "男" ? 0 : 1;
        电话输入框.text = 干预人员.电话;
        简介输入框.text = 干预人员.简介;
        旧数据 = ICan2List;
    }
    void Update()
    {

    }
}
