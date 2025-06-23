using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;
using DG.Tweening;
using TMPro;
public class P_AddGroupCrisisIncident : PopPanelBase
{
    // Start is called before the first frame update
    [必填InputField("事件名称不能为空")]
    TMP_InputField 事件名称框;
    [SerializeField]
    TMP_Text 发生时间框;
    Button 发生时间选择按钮;
    滚轮万年历管理器 _calendarInstance;

    // [必填InputField("发生地点不能为空")]
    TMP_InputField 发生地点框;
    // [必填InputField("事件描述不能为空")]
    TMP_InputField 事件描述框;

    [SerializeField] 图片选择组件 相关素材图片;
    Button 下一步按钮;

    Button 取消按钮;
    // Dropdown 日期选择下拉框;
    ICan2List 旧数据;
    protected override void Awake()
    {
        base.Awake();
        事件名称框 = 弹出页面.transform.Find("事件名称栏/输入框").GetComponent<TMP_InputField>();
        发生时间框 = 弹出页面.transform.Find("发生时间栏/背景/日期输入框").GetComponent<TMP_Text>();
        发生时间选择按钮 = 弹出页面.transform.Find("发生时间栏/背景/选择日期按钮").GetComponent<Button>();
        发生时间选择按钮.onClick.AddListener(On选择日期按钮点击);

        发生地点框 = 弹出页面.transform.Find("发生地点栏/输入框").GetComponent<TMP_InputField>();
        事件描述框 = 弹出页面.transform.Find("事件描述栏/输入框").GetComponent<TMP_InputField>();
        //    相关素材图片 = 弹出页面.transform.Find("相关素材栏").GetComponent<Image>();
        弹出页面.transform.Find("下一步按钮").GetComponent<Button>().onClick.AddListener(下一步按钮监听);
        弹出页面.transform.Find("取消按钮").GetComponent<Button>().onClick.AddListener(取消按钮监听);
        OpenPanel();
    }

    private void 取消按钮监听()
    {
        ClosePanel();
    }


    private void 下一步按钮监听()
    {
        加载增加事件面板2().Forget();
    }
    async UniTaskVoid 加载增加事件面板2()
    {
        if (!验证输入情况())
        {
            return;
        }
        var groupCrisisIncident = new GroupCrisisIncident();
        groupCrisisIncident.incidentName = 事件名称框.text;
        groupCrisisIncident.occurrenceTime = 发生时间框.text;
        groupCrisisIncident.occurrencePlace = 发生地点框.text;
        groupCrisisIncident.incidentDescription = 事件描述框.text;
        groupCrisisIncident.incidentImageURL = 相关素材图片.GetCurrentImageCacheName() == null ? "" : 相关素材图片.GetCurrentImageCacheName();
        var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("增加事件面板2");
        弹出页面.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() =>
        {
            Instantiate(pfb, FindObjectOfType<Canvas>().transform).GetComponent<P_AddGroupCrisisIncident_2>().设置数据并打开面板(groupCrisisIncident);
            // await UniTask.Delay(100);
            Destroy(gameObject);
        }).SetEase(Ease.InSine);


    }

    protected override bool 验证输入情况()
    {
        if (!base.验证输入情况())
        {
            return false;
        }
        if (string.IsNullOrEmpty(发生时间框.text))
        {
            WorkSceneManager.Instance.加载提示("发生时间不能为空").Forget();
            return false;
        }
        return true;
    }
    
    async void On选择日期按钮点击()
    {
        if (_calendarInstance == null)
        {
            var pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb("万年历");
            if (pfb == null)
            {
                Debug.LogError("万年历预制体加载失败！");
                return;
            }
            var go = Instantiate(pfb, 弹出页面.transform.parent);
            _calendarInstance = go.GetComponent<滚轮万年历管理器>();
            
            await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        }

        _calendarInstance.显示万年历();

        if (DateTime.TryParse(发生时间框.text, out DateTime currentDate))
        {
            _calendarInstance.设置选择日期(currentDate);
        }
        else
        {
            _calendarInstance.设置选择日期(DateTime.Now);
        }

        _calendarInstance.On日期确认 = null; 
        _calendarInstance.On日期确认 += (selectedDate) =>
        {
            发生时间框.text = selectedDate.ToString("yyyy-MM-dd");
            _calendarInstance.隐藏万年历();
        };
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void 编辑条目(ICan2List ICan2List)
    {
        var 应激事件消息 = ICan2List as GroupCrisisIncident;
        事件名称框.text = 应激事件消息.incidentName;
        发生时间框.text = 应激事件消息.occurrenceTime;
        发生地点框.text = 应激事件消息.occurrencePlace;
        事件描述框.text = 应激事件消息.incidentDescription;
        旧数据 = ICan2List;
    }
}
