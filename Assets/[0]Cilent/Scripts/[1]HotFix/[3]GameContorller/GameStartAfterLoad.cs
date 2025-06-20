using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using QFramework;
public class GameStartAfterLoad : MonoBehaviour, IController
{
    // Start is called before the first frame update

    void Start()
    {
        gameObject.GetComponent<SupplementaryMetadata>().onSupplementaryMetadata += GameLoadedInit;
    }
    async void GameLoadedInit()
    {
        Application.targetFrameRate = 120;
        Debug.Log("GameHotedInit4");
        await WaitLoadAnimation(this.GetCancellationTokenOnDestroy());
        Debug.Log("加载数据完成22");
        this.GetModel<PersonalPersonnelCrisisEventMessageModel>();
       // await 加载UI下所有预制体();
        await UniTask.Delay(100);
        // var HotUI = await LoadYooAssetsTool.LoadAsset<GameObject>("LoadNewGameVesion");
        // Instantiate(HotUI, FindObjectOfType<Canvas>().transform);
        LoadYooAssetsTool.LoadSceneAsync("StartLoginScene").Forget();
    }
    async UniTask WaitLoadAnimation(CancellationToken cancellationToken)
    {
        var progress = FindObjectOfType<HotFixAssetsProgress>();
        while (progress._tweenerQueue.Count > 0)
        {
            await UniTask.Delay(100, cancellationToken: cancellationToken);
        }
    }
    async UniTask 加载UI下所有预制体()
    {
      //  yoo
      var uiAssetNames = LoadYooAssetsTool.GetAssetInfosByTag("UI");
      foreach (var assetName in uiAssetNames)
      {
        var finalPath = assetName.Replace("_PC", "").Replace("_Mobile", "");
        await this.GetModel<YooAssetPfbModel>().LoadPfb(finalPath);
      }
    }
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
