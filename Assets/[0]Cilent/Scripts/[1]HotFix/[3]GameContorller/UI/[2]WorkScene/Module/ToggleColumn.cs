using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ToggleColumn : MonoBehaviour
{
    // Start is called before the first frame update
    int _currentIndex = -1;
    public int currentIndex
    {
        get => _currentIndex; set
        {
            _currentIndex = value;
            toggleList[value].isOn = true;
        }
    }
    public UnityEvent<int> OnToggleChange;
    List<Toggle> toggleList = new List<Toggle>();
    private void Awake()
    {
        toggleList = GetComponentsInChildren<Toggle>().ToList();
    }
    void Start()
    {
        // 为每个Toggle添加监听，确保只有一个Toggle处于激活状态
        toggleList.ForEach(toggle =>
        {
            toggle.onValueChanged.AddListener(isOn =>
            {
                if (isOn)
                {
                    toggleList.Where(t => t != toggle && t.isOn).ToList().ForEach(t => t.isOn = false);
                    currentIndex = toggleList.IndexOf(toggle);
                    OnToggleChange?.Invoke(currentIndex);
                }
            });
        });
    }



}
