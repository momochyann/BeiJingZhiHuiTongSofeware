using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

//using NativeGalleryNamespace;
using QFramework;
public class DllLink : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Asset tMP_Asset;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] CanvasGroup canvasGroup;
    void Start()
    {
        PickImageFromGalleryAsync().Forget();
        // 添加AOT泛型预热
        AOTGenericWarmup();
    }
    public void Show()
    {
        tMP_Text.text = "正在加载资源...";
        tMP_Text.DOFade(1, 0.3f);
        canvasGroup.DOFade(1, 0.3f).From(0);
    }
    public void Hide()
    {
        canvasGroup.DOFade(0, 0.3f);

    }
    async UniTask<Texture2D> PickImageFromGalleryAsync()
    {
        await UniTask.Delay(1000);
        return default;
    }
    /// <summary>
    /// AOT泛型预热 - 确保热更新中用到的泛型类型在AOT中被生成
    /// </summary>
    private void AOTGenericWarmup()
    {
        // UniTask相关泛型预热
        _ = default(UniTask<Texture2D>);
        _ = default(UniTask<string>);
        _ = default(UniTask<bool>);
        _ = default(UniTask<byte[]>);

        // UniTaskCompletionSource预热 - 这个很重要！
        _ = default(UniTaskCompletionSource<Texture2D>);
        _ = default(UniTaskCompletionSource<string>);
        _ = default(UniTaskCompletionSource<bool>);

        // Task相关预热
        _ = default(System.Threading.Tasks.Task<Texture2D>);
        _ = default(System.Threading.Tasks.Task<string>);
        _ = default(System.Threading.Tasks.Task<bool>);

        // TaskCompletionSource预热
        _ = default(System.Threading.Tasks.TaskCompletionSource<Texture2D>);
        _ = default(System.Threading.Tasks.TaskCompletionSource<string>);

        // 常用Action/Func预热
        _ = default(System.Action<Texture2D>);
        _ = default(System.Action<string>);
        _ = default(System.Func<UniTask<Texture2D>>);
        _ = default(System.Func<UniTask<string>>);

        Debug.Log("AOT泛型预热完成");
    }
    private void EnsureTMPAssemblyReference()
    {
        // 强制引用TextMeshPro程序集，确保它被包含在热更新环境中
        var tmp = typeof(TMPro.TextMeshProUGUI);
        var tmpSettings = typeof(TMPro.TMP_Settings);
        Debug.Log($"确保TMP程序集引用: {tmp.Assembly.FullName}");
    }
    private void EnsureAOT()
    {
        var tcs = new UniTaskCompletionSource<Texture2D>();
        tcs.TrySetResult(null);
    }
    // Update is called once per frame
    void Update()
    {

    }


}
