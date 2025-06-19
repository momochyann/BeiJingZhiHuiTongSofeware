using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Cysharp.Threading.Tasks;
using QFramework;

public class H_选择界面保存按键 : MonoBehaviour, IController
{
    // Start is called before the first frame update
    [SerializeField] Button 保存按钮;
    
    void Start()
    {
        保存按钮.onClick.AddListener(保存按钮点击);
    }

    // Update is called once per frame
    void 保存按钮点击()
    {
        // 查找EntryDisPanelNew来获取所有IEntry对象
        var entryDisPanel = FindObjectOfType<EntryDisPanelNew>();
        if (entryDisPanel != null)
        {
            // 获取所有IEntry对象
            IEntry[] entries = entryDisPanel.transform.GetComponentsInChildren<IEntry>();
            
            // 获取所有被选中的条目
            var selectedEntries = entries
                .Where(entry => entry.IsChoose && entry.can2ListValue is GroupPersonnelCrisisEventMessage)
                .Select(entry => entry.can2ListValue as GroupPersonnelCrisisEventMessage)
                .Where(person => person != null)
                .Select(person => person.name)
                .ToList();
            
            Debug.Log($"选中的姓名: {string.Join(", ", selectedEntries)}");
            
            // 将选中的名字通过事件发送给增加团队面板
            var 增加团队面板 = FindObjectOfType<P_增加团队面板>();
            if (增加团队面板 != null)
            {
                增加团队面板.更新选中人员(selectedEntries);
            }
        }
        else
        {
            Debug.LogError("未找到EntryDisPanelNew");
        }

        // 关闭选择界面
        Destroy(gameObject);
    }
    
    public IArchitecture GetArchitecture()
    {
        return HotFixTemplateArchitecture.Interface;
    }
}
