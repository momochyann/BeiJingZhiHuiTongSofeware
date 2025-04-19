using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using QFramework;
public class EntryEditorSelect : MonoBehaviour, IController
{
    // Start is called before the first frame update
    GameObject 功能按钮预制体;
    [SerializeField] string 功能按钮;
    void Start()
    {
        Init().Forget();
    }
    async UniTaskVoid Init()
    {
        var model = this.GetModel<YooAssetPfbModel>();
        功能按钮预制体 = await model.LoadPfb("功能按钮");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public IArchitecture GetArchitecture()
    {
        return this.GetArchitecture();
    }
}
