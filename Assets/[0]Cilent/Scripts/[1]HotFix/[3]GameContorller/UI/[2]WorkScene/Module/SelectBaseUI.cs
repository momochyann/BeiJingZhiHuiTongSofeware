using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;

public class SelectBaseUI : MonoBehaviour, IController
{
    // Start is called before the first frame update
    public Button button1;
    public Button button2;
    public Image selectImageBack;
    protected GameObject panel1Pfb;
    protected GameObject panel2Pfb;
    public String panel1Name;
    public String panel2Name;
    GameObject currentPanel;
    [SerializeField] bool isOurSelf = false;
    void Start()
    {
        Init().Forget();
    }
    async protected virtual UniTaskVoid Init()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        panel1Pfb = await model.LoadPfb(panel1Name);
        panel2Pfb = await model.LoadPfb(panel2Name);
        button1.onClick.AddListener(OnButton1Click);
        button2.onClick.AddListener(OnButton2Click);
        if (!isOurSelf)
        {
            currentPanel = Instantiate(panel1Pfb, transform.parent);
        }
        else
        {
            currentPanel = transform.parent.parent.gameObject;
        }
    }

    private void OnButton2Click()
    {
        if (currentPanel != null)
            Destroy(currentPanel);
        Transform parent = isOurSelf ? FindObjectOfType<Canvas>().transform : transform.parent;
        currentPanel = Instantiate(panel2Pfb, parent);
        Animation(2).Forget();
    }

    private void OnButton1Click()
    {
        if (currentPanel != null)
            Destroy(currentPanel);

        Transform parent = isOurSelf ? FindObjectOfType<Canvas>().transform : transform.parent;
        currentPanel = Instantiate(panel1Pfb, parent);
        Animation(1).Forget();
    }

    async protected virtual UniTaskVoid Animation(int index)
    {
        await UniTask.Yield();
        button1.GetComponentInChildren<Text>().DOColor(index == 1 ? Color.white : Color.gray, 0.3f);
        button2.GetComponentInChildren<Text>().DOColor(index == 2 ? Color.white : Color.gray, 0.3f);

        selectImageBack.transform.DOMoveX(index == 1 ? button1.transform.position.x : button2.transform.position.x, 0.3f);
    }
    // Update is called once per frame

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
