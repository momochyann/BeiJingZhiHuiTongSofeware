using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using QFramework;
using Cysharp.Threading.Tasks;

public class H_读取名称 : MonoBehaviour, IController
{

    // Start is called before the first frame update
    // PlayerPrefs.SetString("systemInfo", "123");
    // PlayerPrefs.GetString("systemInfo");
    [SerializeField] TMP_InputField 单位名称输入框1;
    [SerializeField] TMP_InputField 单位名称输入框2;
    private Button 替换按钮;

    void Start()
    {
        单位名称输入框1.text = PlayerPrefs.GetString("单位名称");
        单位名称输入框2.text = PlayerPrefs.GetString("软件名称");

        // 设置输入框光标颜色为黑色
        单位名称输入框1.caretColor = Color.black;
        单位名称输入框2.caretColor = Color.black;

        // 禁用输入框获得焦点时全选文本
        单位名称输入框1.onFocusSelectAll = false;
        单位名称输入框2.onFocusSelectAll = false;

        // 添加输入框监听事件
        单位名称输入框1.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetString("单位名称", value);
            FindObjectOfType<H_显示名称>().setname();
        });

        单位名称输入框2.onValueChanged.AddListener((value) => {
            PlayerPrefs.SetString("软件名称", value);
            FindObjectOfType<H_显示名称>().setname();
        });

        // 保留替换按钮功能
        this.transform.Find("替换按钮").GetComponent<Button>().onClick.AddListener(() =>{
            // PlayerPrefs.SetString("单位名称", 单位名称输入框1.text);
            // PlayerPrefs.SetString("软件名称", 单位名称输入框2.text);
            // Debug.Log(单位名称输入框1.text);
            // Debug.Log(单位名称输入框2.text);
            // WorkSceneManager.Instance.加载通知("操作提示", "替换成功").Forget();
            // FindObjectOfType<H_显示名称>().setname();
        });
    }
    // Update is called once per frame
    void Update()
    {

    }

    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
