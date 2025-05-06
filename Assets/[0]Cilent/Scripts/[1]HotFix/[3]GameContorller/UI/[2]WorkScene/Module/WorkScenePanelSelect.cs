using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine.EventSystems;
[Serializable]
public class 切换栏目
{
    public GameObject panelObj;
    public Button button;
    public string panelName;
}
public class WorkScenePanelSelect : MonoBehaviour, IController
{
    public List<切换栏目> 栏目列表;
    public Image selectImageBack;
    public GameObject 父节点;
    // Start is called before the first frame update
    void Start()
    {
        Init().Forget();
    }
    async protected virtual UniTaskVoid Init()
    {
        foreach (var item in 栏目列表)
        {
            item.button.onClick.AddListener(OnButtonClick);
            if (!string.IsNullOrEmpty(item.panelName))
            {
                item.panelObj = await this.GetModel<YooAssetPfbModel>().LoadPfb(item.panelName);
            }
        }
    }
    private void OnButtonClick()
    {
        var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        切换栏目 选中栏目 = null;
        List<GameObject> 已显示面板 = new List<GameObject>();
        foreach (var item in 栏目列表)
        {
            if (item.button == button)
            {
                选中栏目 = item;
            }
            var 查至面板 = 父节点.transform.Find(item.panelName + "(Clone)");
            if (查至面板 != null)
            {
                已显示面板.Add(查至面板.gameObject);
            }
        }
        if (已显示面板.Count > 0)
        {
            foreach (var item in 已显示面板)
            {
                Destroy(item);
            }
        }
        if (选中栏目 != null)
        {
            if (选中栏目.panelObj != null)
            {
                Instantiate(选中栏目.panelObj, 父节点.transform);
            }
            Animation(栏目列表.IndexOf(选中栏目)).Forget();
        }
    }
    async protected virtual UniTaskVoid Animation(int index)
    {
        selectImageBack.transform.DOMoveY(栏目列表[index].button.transform.position.y, 0.3f);
    }
    // Update is called once per frame
    void Update()
    {

    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
