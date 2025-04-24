using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using DG.Tweening;
using UnityEngine.UI;
public class P_AddGroupCrisisIncident_2 : PopPanelBase
{
    // Start is called before the first frame update
    public GroupCrisisIncident groupCrisisIncident;
    protected override void Awake()
    {
        base.Awake();
        //   OpenPanel();
    }
    public void 设置数据并打开面板(GroupCrisisIncident groupCrisisIncident)
    {
        this.groupCrisisIncident = groupCrisisIncident;
        OpenPanel();
    }
    protected override void OpenPanel()
    {
        弹出页面.GetComponent<CanvasGroup>().DOFade(1, 0.3f).From(0);
        关闭按钮.onClick.AddListener(ClosePanel);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
