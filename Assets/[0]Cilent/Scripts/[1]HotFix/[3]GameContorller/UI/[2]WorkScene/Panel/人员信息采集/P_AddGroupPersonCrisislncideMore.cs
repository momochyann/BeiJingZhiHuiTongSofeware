using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;
using Michsky.MUIP;
public class P_AddGroupPersonCrisislncideMore : PopPanelBase
{
    // Start is called before the first frame update
    [SerializeField] ButtonManager 导出模板按钮;
    [SerializeField] ButtonManager 导入模板按钮;
    GroupPersonnelCrisisEventMessageModel groupPersonnelCrisisEventMessageModel;

    void Start()
    {
        OpenPanel();
        groupPersonnelCrisisEventMessageModel = this.GetModel<GroupPersonnelCrisisEventMessageModel>();
        导出模板按钮.onClick.AddListener(ExportTemplate);
        导入模板按钮.onClick.AddListener(ImportTemplate);
    }

    private void ImportTemplate()
    {
        var jsonDataUtility = this.GetUtility<JsonDataUtility>();
        groupPersonnelCrisisEventMessageModel.ImportFromJson(false);
    }

    private void ExportTemplate()
    {
        groupPersonnelCrisisEventMessageModel.ExportToJson();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
