using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class PanelBase : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 返回或上一步按钮;
    [SerializeField] Button 下一步按钮;
    [SerializeField] Button 取消按钮;
    [SerializeField] Button 保存按钮;
    [SerializeField] string 上个面板名称;
    [SerializeField] string 下个面板名称;
    [SerializeField] string 取消跳转界面名称;
    protected GameObject 上个面板pfb;
    protected GameObject 下个面板pfb;
    protected GameObject 取消跳转界面pfb;
    protected virtual void Start()
    {
        if (返回或上一步按钮 != null)
        {
            返回或上一步按钮.onClick.AddListener(返回按钮监听);
        }
        if (下一步按钮 != null)
        {
            下一步按钮.onClick.AddListener(下一步按钮监听);
        }
        if (取消按钮 != null)
        {
            取消按钮.onClick.AddListener(取消按钮监听);
        }
        if (保存按钮 != null)
        {
            保存按钮.onClick.AddListener(保存按钮监听);
        }
        AsyncStart().Forget();
    }
    async UniTaskVoid AsyncStart()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        if (string.IsNullOrEmpty(上个面板名称) == false)
        {
            上个面板pfb = await model.LoadPfb(上个面板名称);
        }
        if (string.IsNullOrEmpty(下个面板名称) == false)
        {
            下个面板pfb = await model.LoadPfb(下个面板名称);
        }
        if (string.IsNullOrEmpty(取消跳转界面名称) == false)
        {
            取消跳转界面pfb = await model.LoadPfb(取消跳转界面名称);
        }
    }
    void 返回按钮监听()
    {
        返回按钮监听Virtual();
    }
    protected virtual void 返回按钮监听Virtual()
    {
        Instantiate(上个面板pfb, transform.parent);
        Destroy(gameObject);
    }
    void 下一步按钮监听()
    {
        下一步按钮监听Virtual();
    }
    protected virtual void 下一步按钮监听Virtual()
    {
        Instantiate(下个面板pfb, transform.parent);
        Destroy(gameObject);
    }
    void 取消按钮监听()
    {
        取消按钮监听Virtual();
    }
    protected virtual void 取消按钮监听Virtual()
    {
        Instantiate(取消跳转界面pfb, transform.parent);
        Destroy(gameObject);
    }
    void 保存按钮监听()
    {
        保存按钮监听Virtual();
    }
    protected virtual void 保存按钮监听Virtual()
    {

    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
