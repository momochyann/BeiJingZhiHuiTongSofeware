using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using DG.Tweening;
public class TestUI : MonoBehaviour
{
    // Start is called before the first frame update
    Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
