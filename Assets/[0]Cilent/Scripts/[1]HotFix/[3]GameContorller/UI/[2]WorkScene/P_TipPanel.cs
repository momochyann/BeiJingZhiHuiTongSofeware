using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public class P_TipPanel : PopPanelBase
{
    // Start is called before the first frame update
    [SerializeField] Text 提示文本;
    public UnityAction 确认事件;
    [SerializeField] Button 确认按钮;
    [SerializeField] Button 取消按钮;
    [SerializeField] string 跳转面板名称;
    // [SerializeField] ;
    protected override void Awake()
    {
        base.Awake();
        if (确认按钮 != null)
        {
            确认按钮.onClick.AddListener(确认按钮点击);
        }
        if (取消按钮 != null)
        {
            取消按钮.onClick.AddListener(取消按钮点击);
        }
    }
    void Start()
    {


    }
    void 确认按钮点击()
    {
        确认事件?.Invoke();
        ClosePanel();
    }
    void 取消按钮点击()
    {
        ClosePanel();
    }
    public void 显示面板(string 提示文本内容, string 跳转面板名称 = "")
    {
        if (跳转面板名称 != "")
        {
            this.跳转面板名称 = 跳转面板名称;
            确认事件 += 跳转面板;
        }

        提示文本.text = 提示文本内容;
        OpenPanel();
    }
    protected override void OpenPanel()
    {
        base.OpenPanel();
    }
    async void 跳转面板()
    {
        await UniTask.Delay(1000);
        var 跳转面板pfb = await this.GetModel<YooAssetPfbModel>().LoadPfb(跳转面板名称);
      //  Instantiate(跳转面板pfb).GetComponent<PanelBase>().EnterPanel();
    }
    // Update is called once per frame
}
