using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using TMPro;



public class P_AddGroupCrisisIncident_3 : PopPanelBase
{
    // Start is called before the first frame update

    [SerializeField] GameObject 级别设置布局;

    GameObject 团体事件选项预制体;
    public GroupCrisisIncident groupCrisisIncident;
    GameObject 级别设置栏;
    // bool 是否初始化 = false;
    string[] 级别名称 = new string[] { "一级受害者:", "二级受害者:", "三级受害者:", "四级受害者:", "五级受害者:" };
    string[] 级别描述配置 = new string[] { "指突发危机事件直接受害者或死难者家属", "指现场目击者或幸存者", "指参与营救与救护的间接受害人员，主要有医生、护士、战士、警察等", "指突发危机事件区域的其他人员，如居民、记者、二级受害者家属等", "指通过媒体间接了解了突发危机事件的人" };
    protected override void Awake()
    {
        base.Awake();
        //   OpenPanel();
    }
    private void Start()
    {

    }
    public void 设置数据并打开面板(GroupCrisisIncident groupCrisisIncident)
    {
        this.groupCrisisIncident = groupCrisisIncident;
        OpenPanel();

    }
    protected override void OpenPanel()
    {
        弹出页面.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0).SetEase(Ease.OutSine);
        关闭按钮.onClick.AddListener(ClosePanel);
        初始化面板().Forget();

    }
    async UniTaskVoid 初始化面板()
    {
        级别设置栏 = await this.GetModel<YooAssetPfbModel>().LoadPfb("级别设置栏");

        弹出页面.transform.Find("增加级别按钮").GetComponent<Button>().onClick.AddListener(增加级别按钮监听);
        弹出页面.transform.Find("删除级别按钮").GetComponent<Button>().onClick.AddListener(删除级别按钮监听);
        弹出页面.transform.Find("上一步按钮").GetComponent<Button>().onClick.AddListener(上一步按钮监听);
        弹出页面.transform.Find("保存按钮").GetComponent<Button>().onClick.AddListener(保存按钮监听);
    }

    private void 删除级别按钮监听()
    {
        if (级别设置布局.transform.childCount > 0)
        {
            Destroy(级别设置布局.transform.GetChild(级别设置布局.transform.childCount - 1).gameObject);
        }
    }

    private void 增加级别按钮监听()
    {
        if (级别设置布局.transform.childCount >= 5)
            return;
        Debug.Log(级别设置布局.transform.childCount);
        var 级别设置栏实例 = Instantiate(级别设置栏, 级别设置布局.transform);
        级别设置栏实例.transform.Find("标题").GetComponent<TMP_Text>().text = 级别名称[级别设置布局.transform.childCount - 1];
        级别设置栏实例.transform.Find("输入框/Placeholder").GetComponent<TMP_Text>().text = 级别描述配置[级别设置布局.transform.childCount - 1];
    }

    private void 保存按钮监听()
    {
        var 级别设置 = new AffectedLevel();
        级别设置.affectedLevel = new Dictionary<string, string>();
        for (int i = 0; i < 级别设置布局.transform.childCount; i++)
        {
            var 级别设置栏实例 = 级别设置布局.transform.GetChild(i).gameObject;
            var 级别名称 = 级别设置栏实例.transform.Find("标题").GetComponent<TMP_Text>().text;
            var 级别描述 = 级别设置栏实例.transform.Find("输入框").GetComponent<TMP_InputField>().text;
            if (级别描述 == "")
            {
                级别描述 = 级别描述配置[i];
            }
            级别设置.affectedLevel.Add(级别名称, 级别描述);
        }
        groupCrisisIncident.affectedLevel = 级别设置;
        this.GetModel<GroupCrisisIncidentModel>().AddItem(groupCrisisIncident);
        ClosePanel();
    }

    private void 上一步按钮监听()
    {
        上一步按钮监听async().Forget();
    }
    async UniTaskVoid 上一步按钮监听async()
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("增加事件面板2");
        弹出页面.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
               {
                   Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_AddGroupCrisisIncident_2>().设置数据并打开面板(groupCrisisIncident);
                   Destroy(gameObject);
               });
    }




    // Update is called once per frame
    void Update()
    {

    }
}
