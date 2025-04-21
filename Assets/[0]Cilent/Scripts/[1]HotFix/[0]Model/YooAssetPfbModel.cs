using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;
public class YooAssetPfbModel : AbstractModel
{
    public Dictionary<string, GameObject> pfbDict;
    protected override void OnInit()
    {
        pfbDict = new Dictionary<string, GameObject>();
    }
    
    public async UniTask<GameObject> LoadPfb(string pfbName)
    {
        if (pfbDict.ContainsKey(pfbName))
        {
            return pfbDict[pfbName];
        }
        var pfb = await LoadYooAssetsTool.LoadAsset<GameObject>(pfbName);
        pfbDict[pfbName] = pfb;
        return pfb;
    }
}
