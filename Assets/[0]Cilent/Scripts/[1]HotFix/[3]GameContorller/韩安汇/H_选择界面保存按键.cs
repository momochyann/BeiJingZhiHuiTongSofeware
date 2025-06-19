using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;

public class H_选择界面保存按键 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button 保存按钮;
    [SerializeField] Transform toggleContainer; // 存放所有Toggle的容器

    void Start()
    {
        保存按钮.onClick.AddListener(保存按钮点击);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void 保存按钮点击()
    {
        // 获取所有被选中的Toggle
        var selectedToggles = toggleContainer.GetComponentsInChildren<Toggle>()
            .Where(t => t.isOn)
            .Select(t => t.GetComponentInChildren<Text>().text)
            .ToList();

        // 将选中的名字通过事件发送给增加团队面板
        var 增加团队面板 = FindObjectOfType<P_增加团队面板>();
        if (增加团队面板 != null)
        {
            增加团队面板.更新选中人员(selectedToggles);
        }

        // 关闭选择界面
        Destroy(gameObject);
    }
}
