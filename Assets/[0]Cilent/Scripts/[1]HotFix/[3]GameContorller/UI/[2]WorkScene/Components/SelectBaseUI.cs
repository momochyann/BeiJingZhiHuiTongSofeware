using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.UI;
using System;
using System.Linq;
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
        Transform canvas = FindObjectsOfType<Canvas>().Where(c => c.gameObject.name == "Canvas").FirstOrDefault().transform;

        Debug.Log(canvas.gameObject.name + "parent" + currentPanel.name);
        var _InstalPanel = Instantiate(panel2Pfb, canvas);
        Animation(2).Forget();
        Debug.Log("OnButton2Click");
        if (currentPanel != null)
        {
            var _currentPanel = currentPanel;
            currentPanel = _InstalPanel;
            Destroy(_currentPanel);
        }
    }

    private void OnButton1Click()
    {
        Transform parent = isOurSelf ? FindObjectOfType<Canvas>().transform : transform.parent;
        var _InstalPanel = Instantiate(panel1Pfb, parent);
        Animation(1).Forget();
        if (currentPanel != null)
        {
            var _currentPanel = currentPanel;
            currentPanel = _InstalPanel;
            Destroy(_currentPanel);
        }

    }

    async protected virtual UniTaskVoid Animation(int index)
    {
        await UniTask.Yield(this.GetCancellationTokenOnDestroy());
        button1.GetComponentInChildren<Text>().DOColor(index == 1 ? Color.white : Color.gray, 0.3f);
        button2.GetComponentInChildren<Text>().DOColor(index == 2 ? Color.white : Color.gray, 0.3f);
        if (selectImageBack != null)
        {
            selectImageBack.transform.DOMoveX(index == 1 ? button1.transform.position.x : button2.transform.position.x, 0.3f);
        }
    }
    // Update is called once per frame

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
