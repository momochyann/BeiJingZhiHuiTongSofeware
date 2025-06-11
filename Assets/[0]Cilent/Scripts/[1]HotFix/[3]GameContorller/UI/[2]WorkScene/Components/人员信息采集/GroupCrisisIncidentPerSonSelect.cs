using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
public class GroupCrisisIncidentPerSonSelect : MonoBehaviour, IController
{
    // Start is called before the first frame update
    GameObject 团体危机显示面板;
    GameObject 团体危机人员显示面板;
    Transform 动画底线;
    bool 是否为事件显示面板 = true;
    Button 团体危机显示按钮;
    Button 团体危机人员显示按钮;
    [SerializeField]
    Color 选中颜色;
    void Start()
    {
        InitAsync().Forget();
    }
    async UniTask InitAsync()
    {
        团体危机显示按钮 = transform.Find("团体危机显示按钮").GetComponent<Button>();
        团体危机人员显示按钮 = transform.Find("团体危机人员显示按钮").GetComponent<Button>();
        动画底线 = transform.Find("底线");
        团体危机显示按钮.onClick.AddListener(团体危机显示按钮点击);
        团体危机人员显示按钮.onClick.AddListener(团体危机人员显示按钮点击);
        团体危机显示面板 = await this.GetModel<YooAssetPfbModel>().LoadPfb("团体危机显示面板");
        团体危机人员显示面板 = await this.GetModel<YooAssetPfbModel>().LoadPfb("团体危机人员显示面板");
    }
    // Update is called once per frame
    void 团体危机显示按钮点击()
    {
        if (是否为事件显示面板)
            return;
        动画底线.DOMoveX(团体危机显示按钮.transform.position.x, 0.3f);
        动画底线.DOScale(new Vector3(1f, 1, 1f), 0.3f);
        团体危机显示按钮.GetComponentInChildren<Text>().DOColor(选中颜色, 0.3f); 

        是否为事件显示面板 = true;
        Destroy(transform.parent.GetComponentInChildren<EntryDisPanelNew>().gameObject);
        Instantiate(团体危机显示面板, transform.parent);

    }
    void 团体危机人员显示按钮点击()
    {
        if (!是否为事件显示面板)
            return;
        动画底线.DOMoveX(团体危机人员显示按钮.transform.position.x, 0.3f);
        动画底线.DOScale(new Vector3(0.7f, 1, 1f), 0.3f);
        //团体危机显示按钮.GetComponentInChildren<TMP_Text>().Color(Color.black, 0.3f);
        团体危机显示按钮.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Normal;
        //Debug.Log("团体危机显示按钮颜色:"+团体危机显示按钮.GetComponentInChildren<Text>().color);
        团体危机人员显示按钮.GetComponentInChildren<TMP_Text>().DOColor(选中颜色, 0.3f);
        团体危机人员显示按钮.GetComponentInChildren<TMP_Text>().fontStyle = FontStyles.Bold;

        是否为事件显示面板 = false;
        Destroy(transform.parent.GetComponentInChildren<EntryDisPanelNew>().gameObject);
        Instantiate(团体危机人员显示面板, transform.parent);
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
