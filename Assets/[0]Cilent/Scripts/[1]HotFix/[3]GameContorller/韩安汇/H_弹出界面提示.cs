using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class H_弹出界面提示 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text 提示文本;
    private CanvasGroup canvasGroup; // 用于控制整个预制体的透明度
    
    void Awake()
    {
        // 获取或添加CanvasGroup组件到根对象
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // 初始设置为透明
        canvasGroup.alpha = 0f;
    }
    
    public void 显示提示(string 提示文本内容)
    {
        this.提示文本.text = 提示文本内容;
        
        // 整个预制体淡入效果
        canvasGroup.DOFade(1f, 1f).SetEase(Ease.OutQuad);
        
        // 3秒后整个预制体淡出并销毁
        StartCoroutine(淡出并销毁());
    }
    
    private IEnumerator 淡出并销毁()
    {
        yield return new WaitForSeconds(2f); // 等待2秒后开始淡出
        
        // 整个预制体淡出效果
        canvasGroup.DOFade(0f, 1f).SetEase(Ease.InQuad).OnComplete(() => {
            Destroy(gameObject);
        });
    }
}
