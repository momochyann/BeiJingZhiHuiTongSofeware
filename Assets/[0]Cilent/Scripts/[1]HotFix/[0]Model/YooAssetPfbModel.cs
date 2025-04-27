using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
public class YooAssetPfbModel : AbstractModel
{
    public Dictionary<string, UnityEngine.Object> pfbDict;
    protected override void OnInit()
    {
        pfbDict = new Dictionary<string, UnityEngine.Object>();
    }
    
    public async UniTask<GameObject> LoadPfb(string pfbName)
    {
        if (pfbDict.ContainsKey(pfbName))
        {
            return pfbDict[pfbName] as GameObject;
        }
        var pfb = await LoadYooAssetsTool.LoadAsset<GameObject>(pfbName);
        pfbDict[pfbName] = pfb;
        return pfb;
    }

    public async UniTask<T> LoadConfig<T>(string configName) where T : Object
    {
        if (pfbDict.ContainsKey(configName))
        {
            return pfbDict[configName] as T;
        }
        var config = await LoadYooAssetsTool.LoadAsset<T>(configName);
        pfbDict[configName] = config;
        return config;
    }
}
