using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;
using UnityEngine.Events;
using DG.Tweening;
public class PagePromptBox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Text pageIndexText;
    [SerializeField] ProceduralImage pageIndexBackgroundImage;
    Button pageIndexButton;
    [HideInInspector] public UnityEvent<int> OnPageIndexButtonClickEvent;
    float AnimationDuration = 0.3f;
    void Start()
    {
        pageIndexButton = GetComponent<Button>();
        pageIndexButton.onClick.AddListener(OnPageIndexButtonClick);
    }
    public void SetPageIndex(string index, bool isActive)
    {
        pageIndexText.text = index;
        pageIndexBackgroundImage.BorderWidth = isActive ? 0 : 1;
        pageIndexBackgroundImage.DOFade(1, AnimationDuration).From(isActive ? 0.5f : 1);
        // pageIndexText.color = isActive ? Color.white : Color.gray;
        pageIndexText.DOColor(isActive ? Color.white : Color.gray, AnimationDuration);
    }
    public void SetPageIndex(int index, bool isActive)
    {
        pageIndexText.text = index.ToString();
        pageIndexBackgroundImage.BorderWidth = isActive ? 0 : 1;
        pageIndexBackgroundImage.DOFade(1, AnimationDuration).From(isActive ? 0.5f : 1);
        // pageIndexText.color = isActive ? Color.white : Color.gray;
        pageIndexText.DOColor(isActive ? Color.white : Color.gray, AnimationDuration);
    }

    void OnPageIndexButtonClick()
    {
        if (int.TryParse(pageIndexText.text, out int index))
        {
            OnPageIndexButtonClickEvent?.Invoke(index);
        }
    }
    void OnDestroy()
    {
        OnPageIndexButtonClickEvent?.RemoveAllListeners();
    }
}
