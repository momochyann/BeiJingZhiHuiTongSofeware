using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using TMPro;
using Michsky.MUIP;
using Cysharp.Threading.Tasks;
using System.Linq;

public class P_增加团队面板 : PopPanelBase
{
    // Start is called before the first frame update
    [SerializeField] TMP_InputField 组别名称;
    [SerializeField] CustomDropdown 主干预人员;
    [SerializeField] TMP_InputField 助理人员;
    public TMP_Text 人员详情;
    [SerializeField] Button 选择人员按钮;
    public TMP_Text 人员数量;
    [SerializeField] TMP_InputField 人员备注;
    干预实施团队 干预实施团队数据;
    IntervenersModel 人员数据;
    List<GroupPersonnelCrisisEventMessage> 选择人员列表 = new List<GroupPersonnelCrisisEventMessage>();
    GameObject 选择界面;
    void Start()
    {
        InitAsync().Forget();
    }
    async UniTask InitAsync()
    {
        选择界面 = await this.GetModel<YooAssetPfbModel>().LoadPfb("选择界面");
        Debug.Log("选择界面加载完成");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Awake()
    {
        base.Awake();
        
        Debug.Log("[调试] 开始获取UI组件...");
        
        // 检查弹出页面是否存在
        if (弹出页面 == null)
        {
            Debug.LogError("[调试] 错误：弹出页面为null！");
            return;
        }
        Debug.Log("[调试] 弹出页面获取成功");
        
        // 获取组别名称组件
        var 组别名称Transform = 弹出页面.transform.Find("组别栏/输入框");
        if (组别名称Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '组别栏/输入框'");
        }
        else
        {
            组别名称 = 组别名称Transform.GetComponent<TMP_InputField>();
            if (组别名称 == null)
            {
                Debug.LogError("[调试] 错误：在 '组别栏/输入框' 上未找到TMP_InputField组件");
            }
            else
            {
                Debug.Log("[调试] 组别名称组件获取成功");
            }
        }
        
        // 获取主干预人员组件
        var 主干预人员Transform = 弹出页面.transform.Find("主干预人员栏/下拉选择框");
        if (主干预人员Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '主干预人员栏/下拉选择框'");
        }
        else
        {
            主干预人员 = 主干预人员Transform.GetComponent<CustomDropdown>();
            if (主干预人员 == null)
            {
                Debug.LogError("[调试] 错误：在 '主干预人员栏/下拉选择框' 上未找到CustomDropdown组件");
            }
            else
            {
                Debug.Log("[调试] 主干预人员组件获取成功");
            }
        }
        
        // 获取助理人员组件
        var 助理人员Transform = 弹出页面.transform.Find("助理人员栏/输入框");
        if (助理人员Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '助理人员栏/输入框'");
        }
        else
        {
            助理人员 = 助理人员Transform.GetComponent<TMP_InputField>();
            if (助理人员 == null)
            {
                Debug.LogError("[调试] 错误：在 '助理人员栏/输入框' 上未找到TMP_InputField组件");
            }
            else
            {
                Debug.Log("[调试] 助理人员组件获取成功");
            }
        }
        
        // 获取人员详情组件
        var 人员详情Transform = 弹出页面.transform.Find("人员详情栏/显示文本");
        if (人员详情Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '人员详情栏/显示文本'");
        }
        else
        {
            人员详情 = 人员详情Transform.GetComponent<TMP_Text>();
            if (人员详情 == null)
            {
                Debug.LogError("[调试] 错误：在 '人员详情栏/显示文本' 上未找到TMP_Text组件");
            }
            else
            {
                Debug.Log("[调试] 人员详情组件获取成功");
            }
        }
        
        // 获取选择人员按钮组件
        var 选择人员按钮Transform = 弹出页面.transform.Find("人员详情栏/选择按键");
        if (选择人员按钮Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '人员详情栏/选择按键'");
        }
        else
        {
            选择人员按钮 = 选择人员按钮Transform.GetComponent<Button>();
            if (选择人员按钮 == null)
            {
                Debug.LogError("[调试] 错误：在 '人员详情栏/选择按键' 上未找到Button组件");
            }
            else
            {
                Debug.Log("[调试] 选择人员按钮组件获取成功");
            }
        }
        
        // 获取人员数量组件
        var 人员数量Transform = 弹出页面.transform.Find("人员数量栏/输入框");
        if (人员数量Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '人员数量栏/输入框'");
        }
        else
        {
            人员数量 = 人员数量Transform.GetComponent<TMP_Text>();
            if (人员数量 == null)
            {
                Debug.LogError("[调试] 错误：在 '人员数量栏/输入框' 上未找到TMP_Text组件");
            }
            else
            {
                Debug.Log("[调试] 人员数量组件获取成功");
            }
        }
        
        // 获取人员备注组件
        var 人员备注Transform = 弹出页面.transform.Find("备注栏/输入框");
        if (人员备注Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '备注栏/输入框'");
        }
        else
        {
            人员备注 = 人员备注Transform.GetComponent<TMP_InputField>();
            if (人员备注 == null)
            {
                Debug.LogError("[调试] 错误：在 '备注栏/输入框' 上未找到TMP_InputField组件");
            }
            else
            {
                Debug.Log("[调试] 人员备注组件获取成功");
            }
        }
        
        // 添加空值检查，确保组件获取成功后再设置属性
        if (组别名称 != null) 
        {
            组别名称.caretColor = Color.black;
            Debug.Log("[调试] 组别名称光标颜色设置成功");
        }
        if (助理人员 != null) 
        {
            助理人员.caretColor = Color.black;
            Debug.Log("[调试] 助理人员光标颜色设置成功");
        }
        if (人员备注 != null) 
        {
            人员备注.caretColor = Color.black;
            Debug.Log("[调试] 人员备注光标颜色设置成功");
        }
        
        // 获取保存按钮
        var 保存按钮Transform = 弹出页面.transform.Find("保存按钮");
        if (保存按钮Transform == null)
        {
            Debug.LogError("[调试] 错误：未找到路径 '保存按钮'");
        }
        else
        {
            var 保存按钮 = 保存按钮Transform.GetComponent<Button>();
            if (保存按钮 == null)
            {
                Debug.LogError("[调试] 错误：在 '保存按钮' 上未找到Button组件");
            }
            else
            {
                保存按钮.onClick.AddListener(保存数据按钮监听);
                Debug.Log("[调试] 保存按钮监听器添加成功");
            }
        }
        
        人员数据 = this.GetModel<IntervenersModel>();
        Debug.Log("[调试] 人员数据模型获取成功");
        
        填充人员下拉框();
        选择人员按钮.onClick.AddListener(选择人员按钮监听);
        OpenPanel();
        
        Debug.Log("[调试] Awake方法执行完成");
    }
    void 保存数据按钮监听()
    {
        Debug.Log("保存数据");
        干预实施团队 应激事件消息 = new 干预实施团队();
        应激事件消息.组别名称 = 组别名称.text;
        应激事件消息.主干预人员 = 主干预人员.items[主干预人员.selectedItemIndex].itemName;
        应激事件消息.助理人员 = 助理人员.text;
        应激事件消息.备注 = 人员备注.text;
        应激事件消息.人员列表 = 选择人员列表;
        this.SendCommand(new AddEntryCommand(应激事件消息 as ICan2List, "干预实施Model"));
        ClosePanel();
    }
    void 填充人员下拉框()
    {
        if (人员数据.intervenerList.Count <= 0)
            return;
        主干预人员.items.Clear();
        foreach (var 人员 in 人员数据.intervenerList)
        {
            主干预人员.CreateNewItem(人员.name);
        }
        主干预人员.SetupDropdown();
        
    }
    void 选择人员按钮监听()
    {
        if (选择界面 == null)
        {
            Debug.LogError("选择界面未加载");
            return;
        }
        Instantiate(选择界面, 弹出页面.transform);
    }

    public void 更新选中人员(List<GroupPersonnelCrisisEventMessage> 选中人员列表)
    {
        选择人员列表 = 选中人员列表 ?? new List<GroupPersonnelCrisisEventMessage>();
        
        if (选择人员列表.Count == 0)
        {
            if (人员详情 != null)
                人员详情.text = "未选择人员";
            if (人员数量 != null)
                人员数量.text = "0";
            return;
        }
        
        Debug.Log("选中人员列表[0].name是" + 选中人员列表[0].name);
        
        // 将选中的名字用逗号连接显示
        var 人员姓名列表 = 选择人员列表.Select(p => p.name).ToList();
        Debug.Log($"选中的姓名: {string.Join(", ", 人员姓名列表)}");
        
        if (人员详情 != null)
        {
            人员详情.text = string.Join(",", 人员姓名列表);
            Debug.Log("人员详情是" + 人员详情.text);
        }
        else
        {
            Debug.LogError("人员详情组件为null！");
        }
        
        if (人员数量 != null)
            人员数量.text = 选择人员列表.Count.ToString();
    }
}