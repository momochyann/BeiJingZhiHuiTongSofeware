using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
public class DllLink : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CanvasGroup canvasGroup;
    void Start()
    {
      
    }
    public void Show()
    {
        canvasGroup.DOFade(1, 0.3f).From(0);
    }
    public void Hide()
    {
        canvasGroup.DOFade(0, 0.3f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
