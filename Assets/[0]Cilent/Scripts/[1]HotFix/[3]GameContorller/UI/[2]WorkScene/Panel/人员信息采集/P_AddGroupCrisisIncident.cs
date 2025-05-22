using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Linq;
using System;
using DG.Tweening;
public class P_AddGroupCrisisIncident : PopPanelBase
{
    // Start is called before the first frame update
    [必填InputField("事件名称不能为空")]
    InputField 事件名称框;
    [必填InputField("发生时间不能为空")]
    InputField 发生时间框;
    [必填InputField("发生地点不能为空")]
    InputField 发生地点框;
    [必填InputField("事件描述不能为空")]
    InputField 事件描述框;
    [SerializeField] Image 相关素材图片;
    Button 下一步按钮;

    Button 取消按钮;
    // Dropdown 日期选择下拉框;
    ICan2List 旧数据;
    protected override void Awake()
    {
        base.Awake();
        事件名称框 = 弹出页面.transform.Find("事件名称栏/输入框").GetComponent<InputField>();
        发生时间框 = 弹出页面.transform.Find("发生时间栏/输入框").GetComponent<InputField>();
        发生地点框 = 弹出页面.transform.Find("发生地点栏/输入框").GetComponent<InputField>();
        事件描述框 = 弹出页面.transform.Find("事件描述栏/输入框").GetComponent<InputField>();
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
        return true;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
