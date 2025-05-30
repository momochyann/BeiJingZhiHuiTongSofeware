using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
public class PerSonGroupSelect : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField]
    private Button 团体按钮;
    [SerializeField]
    private Button 个人按钮;
    [SerializeField]
    private Image 选中图片;
    [SerializeField]
    private RectTransform[] 选中图片位置;
    string 团体界面名称 = "";
    string 个人界面名称 = "";
    GameObject 团体界面预制体;
    GameObject 个人界面预制体;
    GameObject 当前界面;
    void Start()
    {
        团体界面名称 = 团体按钮.gameObject.name;
        个人界面名称 = 个人按钮.gameObject.name;
        团体按钮.onClick.AddListener(OnGroupButtonClick);
        个人按钮.onClick.AddListener(OnPersonButtonClick);
        Init().Forget();
    }
    async UniTaskVoid Init()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        团体界面预制体 = await model.LoadPfb(团体界面名称);
        个人界面预制体 = await model.LoadPfb(个人界面名称);
        if (FindObjectOfType<EntryDisPanelNew>() != null)
        {
            var 显示界面 = FindObjectOfType<EntryDisPanelNew>().gameObject;
            当前界面 = 显示界面.name == "个人信息管理" ? 显示界面 : 显示界面.transform.parent.gameObject;
        }
    }

    void 切换动画(string 界面名称)
    {
        Color 团体文字颜色 = 界面名称 == 团体界面名称 ? Color.white : Color.gray;
        Color 个人文字颜色 = 界面名称 == 个人界面名称 ? Color.white : Color.gray;
        团体按钮.GetComponentInChildren<TMP_Text>().DOColor(团体文字颜色, 0.3f);
        个人按钮.GetComponentInChildren<TMP_Text>().DOColor(个人文字颜色, 0.3f);
        选中图片.transform.DOMove(选中图片位置[界面名称 == 团体界面名称 ? 1 : 0].position, 0.3f);
    }


    private void OnGroupButtonClick()
    {
        Debug.Log("团体按钮点击");
        if (当前界面 != null)
        {
            Destroy(当前界面);
        }
        当前界面 = Instantiate(团体界面预制体, transform.parent);
        切换动画(团体界面名称);
    }

    private void OnPersonButtonClick()
    {
        Debug.Log("个人按钮点击");
        if (当前界面 != null)
        {
            Destroy(当前界面);
        }
        当前界面 = Instantiate(个人界面预制体, transform.parent);
        切换动画(个人界面名称);
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
