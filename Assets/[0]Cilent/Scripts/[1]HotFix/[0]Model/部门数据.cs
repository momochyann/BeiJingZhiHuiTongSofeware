using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 部门 : ICan2List
{
    public string 部门名称;
    public string 部门编号;
    public string 部门级别;
    public string 部门描述;
    //public List<string> 部门成员;
    
}

public class 部门数据Model : CrisisIncidentBaseModel<部门>
{
    public List<部门> 部门列表 => dataList;
    
    protected override string GetStorageKey()
    {
        return "DepartmentData";
    }
    
    protected override void OnInit()
    {
        base.OnInit();
        foreach (var item in 部门列表)
        {
            Debug.Log("部门名称: " + item.部门名称);
        }
    }
    
    // 根据部门名称搜索
    protected override List<int> OnSearchByName(string keyword)
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].部门名称.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
    
    
    
    // 根据部门编号查找部门
    public 部门 根据编号查找部门(string 编号)
    {
        return 部门列表.Find(dept => dept.部门编号 == 编号);
    }
}

