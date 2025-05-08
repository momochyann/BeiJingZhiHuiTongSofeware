using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System;
public class P_AddGroupCrisisIncident_2 : PopPanelBase
{
    // Start is called before the first frame update
    [SerializeField] ScrollRect 事件面板滚动条;
    [SerializeField] LayoutGroup 个体事件面板布局;
    [SerializeField] ToggleColumn 事件类型选项;
    [SerializeField] GameObject 公共危机事件面板;
    [SerializeField] GameObject 个体事件面板;
    ToggleColumn 公共事件选项;
    ToggleColumn 个体事件选项;
    GameObject 团体事件选项预制体;
    public GroupCrisisIncident groupCrisisIncident;
    GroupCrisisIncidentOptionConfig groupCrisisIncidentOptionConfig;
    // bool 是否初始化 = false;
    protected override void Awake()
    {
        base.Awake();
        //   OpenPanel();
    }
    private void Start()
    {
        // 设置数据并打开面板(new GroupCrisisIncident());
    }
    public void 设置数据并打开面板(GroupCrisisIncident groupCrisisIncident)
    {
        this.groupCrisisIncident = groupCrisisIncident;
        OpenPanel();
    }
    protected override void OpenPanel()
    {
        弹出页面.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0);
        关闭按钮.onClick.AddListener(ClosePanel);
        初始化面板().Forget();
    }
    async UniTaskVoid 初始化面板()
    {
        团体事件选项预制体 = await this.GetModel<YooAssetPfbModel>().LoadPfb("团体事件选项");
        groupCrisisIncidentOptionConfig = await this.GetModel<YooAssetPfbModel>().LoadConfig<GroupCrisisIncidentOptionConfig>("GroupCrisisIncidentOptionConfig");
        事件类型选项.currentIndex = 1;
        初始化公共事件面板();
        初始化个体事件面板();
        事件类型选项.OnToggleChange.AddListener(OnToggleChange);
        弹出页面.transform.Find("下一步按钮").GetComponent<Button>().onClick.AddListener(下一步按钮监听);
        弹出页面.transform.Find("上一步按钮").GetComponent<Button>().onClick.AddListener(上一步按钮监听);
    }

    private void 上一步按钮监听()
    {
        上一步按钮监听async().Forget();
    }
    async UniTaskVoid 上一步按钮监听async()
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("增加事件面板");
        弹出页面.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(async () =>
               {
                   Instantiate(pfb, FindObjectOfType<Canvas>().transform);
                   await UniTask.Delay(100);
                   Destroy(gameObject);
                   //    ClosePanel();
               });
    }
    private void 下一步按钮监听()
    {
        下一步按钮监听async().Forget();
    }
    async UniTaskVoid 下一步按钮监听async()
    {
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("增加事件面板3");
        GroupCrisisIncidentType groupCrisisIncidentType = new GroupCrisisIncidentType();
        if (事件类型选项.currentIndex == 0)
        {
              groupCrisisIncidentType.CrisisIncidentTypeName = 公共事件选项.transform.GetChild(公共事件选项.currentIndex).transform.Find("类型名称").GetComponent<Text>().text;
              groupCrisisIncidentType.CrisisIncidentTypeDescription = 公共事件选项.transform.GetChild(公共事件选项.currentIndex).transform.Find("类型描述").GetComponent<Text>().text;
        }
        else
        {
            groupCrisisIncidentType.CrisisIncidentTypeName = 个体事件选项.transform.GetChild(个体事件选项.currentIndex).transform.Find("类型名称").GetComponent<Text>().text;
            groupCrisisIncidentType.CrisisIncidentTypeDescription = 个体事件选项.transform.GetChild(个体事件选项.currentIndex).transform.Find("类型描述").GetComponent<Text>().text;
        }
        groupCrisisIncident.groupCrisisIncidentType = groupCrisisIncidentType;
        弹出页面.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
       {
           Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_AddGroupCrisisIncident_3>().设置数据并打开面板(groupCrisisIncident);
           Destroy(gameObject);
       }).SetEase(Ease.InSine);
    }
    private void OnToggleChange(int index)
    {
        if (index == 0)
        {
            公共危机事件面板.SetActive(true);
            个体事件面板.SetActive(false);
        }
        else
        {
            个体事件面板.SetActive(true);
            公共危机事件面板.SetActive(false);
        }
    }

    void 初始化个体事件面板()
    {
        foreach (var item in groupCrisisIncidentOptionConfig.personalCrisisIncidentTypes)
        {
            var 团体事件选项 = Instantiate(团体事件选项预制体, 个体事件面板布局.transform);
            团体事件选项.gameObject.name = item.CrisisIncidentTypeName;
            团体事件选项.transform.Find("类型名称").GetComponent<Text>().text = item.CrisisIncidentTypeName;
            团体事件选项.transform.Find("类型描述").GetComponent<Text>().text = item.CrisisIncidentTypeDescription;
        }
        个体事件选项 = 个体事件面板布局.gameObject.AddComponent<ToggleColumn>();
        // Canvas.ForceUpdateCanvases();
        // 事件面板滚动条.velocity = Vector2.zero;
        // 事件面板滚动条.normalizedPosition = new Vector2(0, 1); // 滚动到顶部
    }
    void 初始化公共事件面板()
    {
        foreach (var item in groupCrisisIncidentOptionConfig.publicCrisisIncidentTypes)
        {
            var 团体事件选项 = Instantiate(团体事件选项预制体, 公共危机事件面板.transform);
            团体事件选项.gameObject.name = item.CrisisIncidentTypeName;
            团体事件选项.transform.Find("类型名称").GetComponent<Text>().text = item.CrisisIncidentTypeName;
            团体事件选项.transform.Find("类型描述").GetComponent<Text>().text = item.CrisisIncidentTypeDescription;
        }
        公共事件选项 = 公共危机事件面板.gameObject.AddComponent<ToggleColumn>();
    
        公共危机事件面板.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
