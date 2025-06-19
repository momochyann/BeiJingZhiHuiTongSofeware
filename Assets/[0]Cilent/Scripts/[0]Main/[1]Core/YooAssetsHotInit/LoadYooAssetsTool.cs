using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using Cysharp.Threading.Tasks;

public class LoadYooAssetsTool
{
    public static ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
    public static ResourcePackage LocalPackage = YooAssets.GetPackage("LocalDefaultPackage");

    public static ResourcePackage RawFilePackage = YooAssets.GetPackage("RawFilePackage");
    public static ResourcePackage LocalRawFilePackage = YooAssets.GetPackage("LocalRawFilePackage");
    /// <summary>
    /// 加载资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="AssetNames">资源名称</param>
    /// <param name="isLocal">是否本地加载</param>
    /// <returns></returns>

    public static async UniTask<T> LoadAsset<T>(string AssetNames, bool isLocal = false) where T : UnityEngine.Object
    {
        if (isLocal)
        {
            var handle = LocalPackage.LoadAssetAsync<T>(AssetNames);
            await handle.ToUniTask();
            return handle.AssetObject as T;
        }
        else
        {
            var handle = package.LoadAssetAsync<T>(AssetNames);
            await handle.ToUniTask();
            return handle.AssetObject as T;
        }
    }
    public static async UniTask<byte[]> LoadRawFile_DP(string AssetNames, bool isLocal = true)
    {
        if (isLocal)
        {
            var handle = LocalRawFilePackage.LoadRawFileAsync(AssetNames);
            await handle.ToUniTask();
            return handle.GetRawFileData();
        }
        else
        {
            var handle = RawFilePackage.LoadRawFileAsync(AssetNames);
            await handle.ToUniTask();
            return handle.GetRawFileData();
        }
    }
    public static async UniTaskVoid LoadSceneAsync(string AssetNames, bool isLocal = false)
    {
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        bool suspendLoad = false;
        if (isLocal)
        {
            SceneHandle handle = LocalPackage.LoadSceneAsync(AssetNames, sceneMode, suspendLoad);
            await handle.ToUniTask();
        }
        else
        {
            SceneHandle handle = package.LoadSceneAsync(AssetNames, sceneMode, suspendLoad);
            await handle.ToUniTask();
        }
        // return handle.InstantiateSync();
    }
    public static HashSet<string> GetAssetInfosByTag(string tag)
    {
        Debug.Log("GetAssetInfosByTag: " + tag);
        HashSet<string> uiAssetNames = new HashSet<string>(); 
        AssetInfo[] assetInfos = package.GetAssetInfos(tag);
        foreach (var assetInfo in assetInfos)
        {
            uiAssetNames.Add(assetInfo.Address);
           // Debug.Log("assetInfo.Address: " + assetInfo.Address);
        }
        return uiAssetNames;
    }
    public static bool CheckAssetExist(string assetName)
    {
        var assetInfo = package.GetAssetInfo(assetName);
        if(!string.IsNullOrEmpty(assetInfo.AssetPath))
        {
           // Debug.Log("assetName: " + assetName + " 存在");
            return true;
        }
        else
        {
         //   Debug.Log("assetName: " + assetName + " 不存在");
            return false;
        }
    }
}


