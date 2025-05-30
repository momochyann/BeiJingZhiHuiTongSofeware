using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
public class DllLink : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TMP_Asset tMP_Asset;
    [SerializeField] TMP_Text tMP_Text;
    [SerializeField] CanvasGroup canvasGroup;
    void Start()
    {

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
    private void EnsureTMPAssemblyReference()
    {
        // 强制引用TextMeshPro程序集，确保它被包含在热更新环境中
        var tmp = typeof(TMPro.TextMeshProUGUI);
        var tmpSettings = typeof(TMPro.TMP_Settings);
        Debug.Log($"确保TMP程序集引用: {tmp.Assembly.FullName}");
    }
    // Update is called once per frame
    void Update()
    {

    }
}
