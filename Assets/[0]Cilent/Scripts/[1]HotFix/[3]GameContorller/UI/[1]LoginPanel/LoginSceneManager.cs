using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;
public class LoginSceneManager : MonoBehaviour,IController
{
    // Start is called before the first frame update

    void Start()
    {
       LoadLoginPanel();
    }
    async void LoadLoginPanel()
    {
        var loginPanel= await this.GetModel<YooAssetPfbModel>().LoadPfb("LoginPanel");
        Instantiate(loginPanel,transform);
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
