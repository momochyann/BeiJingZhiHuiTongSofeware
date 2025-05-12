using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI.ProceduralImage;
using Cysharp.Threading.Tasks;
using DG.Tweening;
public class ButtonColumn : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public string 组描述 = "";
    int _currentIndex = -1;
    public int currentIndex
    {
        get => _currentIndex; set
        {
            _currentIndex = value;
            buttonList.ForEach(t => { t.GetComponent<ProceduralImage>().BorderWidth = 1; t.GetComponentInChildren<Text>().color = Color.blue; });
            buttonList[_currentIndex].GetComponent<ProceduralImage>().BorderWidth = 0;
            buttonList[_currentIndex].GetComponentInChildren<Text>().color = Color.white;
            // buttonList[_currentIndex].onClick.Invoke();
        }
    }
    [HideInInspector] public UnityEvent<int> OnButtonSelectChange;
    [HideInInspector] public List<Button> buttonList = new List<Button>();
    private void Awake()
    {
        buttonList = GetComponentsInChildren<Button>().ToList();
    }
    void Start()
    {
        // 为每个按钮添加监听，确保只有一个按钮处于激活状态
        Debug.Log(组描述);
        foreach (var button in buttonList)
        {
            Debug.Log(button.name);
            button.onClick.AddListener(() =>
            {
                var _button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
                Debug.Log(button.name + " " + _button.name);
                if (_button == button)
                {
                    button.GetComponent<ProceduralImage>().BorderWidth = 0;
                    button.GetComponentInChildren<Text>().DOColor(Color.white, 0.3f);
                    buttonList.Where(t => t != button).ToList().ForEach(t =>
                    {
                        t.GetComponent<ProceduralImage>().BorderWidth = 1;
                        t.GetComponentInChildren<Text>().DOColor(Color.blue, 0.3f);
                    });
                    _currentIndex = buttonList.IndexOf(button);
                    OnButtonSelectChange?.Invoke(currentIndex);
                }
            });
        }
        currentIndex = 0;
    }

    // void OnButtonSelectChange(int index)


}
