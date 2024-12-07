using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;
using Cysharp.Threading.Tasks;

public class LoadYooAssetsTool
{
    public static ResourcePackage package = YooAssets.GetPackage("DefaultPackage");
    public static ResourcePackage RawFilePackage = YooAssets.GetPackage("RawFilePackage");

    public static async UniTask<T> LoadAsset<T>(string AssetNames) where T : UnityEngine.Object
    {
        var handle = package.LoadAssetAsync<T>(AssetNames);
        await handle.ToUniTask();
        return handle.AssetObject as T;
    }
    public static async UniTask<byte[]> LoadRawFile_DP(string AssetNames)
    {
        var handle = RawFilePackage.LoadRawFileAsync(AssetNames);
        await handle.ToUniTask();
        return handle.GetRawFileData();
    }
    public static async UniTaskVoid LoadSceneAysnc(string AssetNames)
    {
        var sceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single;
        bool suspendLoad = false;
        SceneHandle handle = package.LoadSceneAsync(AssetNames, sceneMode, suspendLoad);
        await handle.ToUniTask();
        // return handle.InstantiateSync();
    }
}


