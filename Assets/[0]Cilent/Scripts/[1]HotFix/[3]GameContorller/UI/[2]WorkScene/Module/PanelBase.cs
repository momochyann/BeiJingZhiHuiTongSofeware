using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class PanelBase : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 返回按钮;
    [SerializeField] Button 下一步按钮;
    [SerializeField] Button 取消按钮;
    [SerializeField] Button 保存按钮;

    void Start()
    {
        if (返回按钮 != null)
        {
            返回按钮.onClick.AddListener(返回按钮监听);
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
    }

    void 返回按钮监听()
    {
        返回按钮监听Virtual();
    }
    protected virtual void 返回按钮监听Virtual()
    {

    }
    void 下一步按钮监听()
    {
        下一步按钮监听Virtual();
    }
    protected virtual void 下一步按钮监听Virtual()
    {

    }
    void 取消按钮监听()
    {
        取消按钮监听Virtual();
    }
    protected virtual void 取消按钮监听Virtual()
    {

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
