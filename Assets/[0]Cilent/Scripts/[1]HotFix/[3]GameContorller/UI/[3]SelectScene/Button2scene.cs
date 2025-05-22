using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
using UnityEngine.Events;
public class Button2scene : MonoBehaviour,IController
{
    [SerializeField] private string sceneName;
    public int WorkSceneIndex;
    void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            LoadYooAssetsTool.LoadSceneAsync(sceneName).Forget();
            if (WorkSceneIndex != 0)
            {
               this.GetSystem<WorkSceneSystem>().WorkSceneIndex = WorkSceneIndex;
            }
        });
    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
