using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Cysharp.Threading.Tasks;



public class YooAssetPfbModel : AbstractModel
{
    public Dictionary<string, UnityEngine.Object> pfbDict;
    private HashSet<string> uiAssetNames = new HashSet<string>(); // 存储UI组的资源名称

    protected override void OnInit()
    {
        pfbDict = new Dictionary<string, UnityEngine.Object>();
        uiAssetNames = LoadYooAssetsTool.GetAssetInfosByTag("UI");

    }

    public bool CheckPfbNameInUIAssets(string pfbName)
    {
        foreach (var assetName in uiAssetNames)
        {
            if (assetName.Contains(pfbName))
            {
                return true;
            }
        }
        return false;
    }

    private string GetPlatformSuffix(bool 取反 = false)
    {

#if UNITY_EDITOR
        // 在编辑器中根据选择的平台返回后缀
        if (UnityEditor.EditorPrefs.GetInt("TestPlatform", 0) == 0)
        {
            if (取反)
            {
                return "_Mobile";
            }
            else
            {
                return "_PC";
            }
        }
        else
        {
            if (取反)
            {
                return "_PC";
            }
            else
            {
                return "_Mobile";
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        if(取反)
        {
            return "_PC";
        }
        else
        {
            return "_Mobile";
        }
#else
        if(取反)
        {
            return "_PC";
        }
        else
        {
            return "_Mobile";
        }
#endif
    }

    public async UniTask<GameObject> LoadPfb(string pfbName)
    {
        string finalPath = pfbName;

        if (CheckPfbNameInUIAssets(pfbName))
        {
            // 移除可能已存在的平台后缀
            finalPath = pfbName.Replace("_PC", "").Replace("_Mobile", "");
            var fileTemp = finalPath + GetPlatformSuffix();
            if (LoadYooAssetsTool.CheckAssetExist(fileTemp))
            {
                finalPath = fileTemp;
            }
            else
            {
                finalPath = pfbName + GetPlatformSuffix(true);
            }
        }

        if (pfbDict.ContainsKey(finalPath))
        {
            return pfbDict[finalPath] as GameObject;
        }

        var pfb = await LoadYooAssetsTool.LoadAsset<GameObject>(finalPath);
        pfbDict[finalPath] = pfb;
        return pfb;
    }

    public async UniTask<T> LoadConfig<T>(string configName, bool islocal = false) where T : Object
    {
        if (pfbDict.ContainsKey(configName))
        {
            return pfbDict[configName] as T;
        }
        var config = await LoadYooAssetsTool.LoadAsset<T>(configName, islocal);
        pfbDict[configName] = config;
        return config;
    }
}
