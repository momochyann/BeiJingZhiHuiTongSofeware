using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class PopPanelBase : MonoBehaviour, IController
{
    // Start is called before the first frame update
    protected GameObject 弹出页面;
    Button 返回按钮;
    bool isClose = false;
    protected virtual void Awake()
    {
        弹出页面 = transform.Find("弹出页面").gameObject;
        返回按钮 = 弹出页面.transform.Find("返回按钮").GetComponent<Button>();
    }
    void Start()
    {

    }
    public virtual void 编辑条目(ICan2List ICan2List)
    {

    }
    protected virtual void OpenPanel()
    {
        弹出页面.transform.DOScale(1, 0.3f).From(0);
        返回按钮.onClick.AddListener(ClosePanel);
    }
    // Update is called once per frame
    public void ClosePanel()
    {
        if (isClose) return;
        isClose = true;
        弹出页面.transform.DOScale(0, 0.1f).From(1);
        Destroy(gameObject, 0.2f);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
