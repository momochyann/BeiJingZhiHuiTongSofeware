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
    [SerializeField] TMP_Text 组别名称;
    [SerializeField] CustomDropdown 主干预人员;
    [SerializeField] TMP_Text 助理人员;
    public TMP_Text 人员详情;
    [SerializeField] Button 选择人员按钮;
    public TMP_Text 人员数量;
    [SerializeField] TMP_Text 人员备注;
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
        
        组别名称 = 弹出页面.transform.Find("组别栏/输入框/Text Area/Text").GetComponent<TMP_Text>();
        主干预人员 = 弹出页面.transform.Find("主干预人员栏/下拉选择框").GetComponent<CustomDropdown>();
        助理人员 = 弹出页面.transform.Find("助理人员栏/输入框/Text Area/Text").GetComponent<TMP_Text>();
        人员详情 = 弹出页面.transform.Find("人员详情栏/显示文本").GetComponent<TMP_Text>();
        选择人员按钮 = 弹出页面.transform.Find("人员详情栏/选择按键").GetComponent<Button>();
        人员数量 = 弹出页面.transform.Find("人员数量栏/输入框").GetComponent<TMP_Text>();
        人员备注 = 弹出页面.transform.Find("备注栏/输入框/Text Area/Text").GetComponent<TMP_Text>();
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存数据按钮监听);
        人员数据 = this.GetModel<IntervenersModel>();
        填充人员下拉框();
        选择人员按钮.onClick.AddListener(选择人员按钮监听);
        OpenPanel();
    }
    void 保存数据按钮监听()
    {
        Debug.Log("保存数据");
        干预实施团队 应激事件消息 = new 干预实施团队();
        应激事件消息.组别名称 = 组别名称.text;
        应激事件消息.主干预人员 = 主干预人员.items[主干预人员.selectedItemIndex].itemName;
        应激事件消息.助理人员 = 助理人员.text;
        应激事件消息.备注 = 人员备注.text;

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
    void 人员详情显示(GroupPersonnelCrisisEventMessage 人员)
    {
        人员详情.text = 人员.name;
    }

    public void 更新选中人员(List<string> 选中人员列表)
    {
        if (选中人员列表 == null || 选中人员列表.Count == 0)
        {
            人员详情.text = "未选择人员";
            return;
        }

        // 将选中的名字用逗号连接显示
        人员详情.text = string.Join("、", 选中人员列表);
        人员数量.text = 选中人员列表.Count.ToString();
    }
}