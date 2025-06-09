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
        Debug.Log("GameHotedInit4");
        await WaitLoadAnimation(this.GetCancellationTokenOnDestroy());
        Debug.Log("加载数据完成22");
        // 初始化ExcelReader
        ExcelReader excelReader = this.GetUtility<ExcelReader>();
        excelReader.Init();
        excelReader.EnsureExcelDirectoryExists();
        // 如果需要，生成清单文件
        excelReader.GenerateExcelManifest();
        this.GetModel<PersonalPersonnelCrisisEventMessageModel>();
        await this.GetModel<YooAssetPfbModel>().LoadPfb("人员信息采集界面");
        await this.GetModel<YooAssetPfbModel>().LoadPfb("危机评估预警界面");
        await this.GetModel<YooAssetPfbModel>().LoadPfb("危机干预实施选择界面");
        await this.GetModel<YooAssetPfbModel>().LoadPfb("危机档案管理选择界面");
        await this.GetModel<YooAssetPfbModel>().LoadPfb("干预资源库面板");
       // await this.GetModel<YooAssetPfbModel>().LoadPfb("系统设置管理界面");
        //初始化录音工具
        //this.GetUtility<AudioRecorderUtility>().Init();

        await UniTask.Delay(1000);
      

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
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
