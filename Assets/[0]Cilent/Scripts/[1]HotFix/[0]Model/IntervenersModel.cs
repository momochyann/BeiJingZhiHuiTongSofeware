using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intervener : ICan2List
{
    public string name;
    public string 性别;
    public string 电话;
    public string 邮箱;
    public string 简介;
    public string 用户名;
    public string 密码;
}
public class IntervenersModel : CrisisIncidentBaseModel<Intervener>
{
    public List<Intervener> intervenerList => dataList;
    protected override string GetStorageKey()
    {
        return "Interveners";
    }
    protected override void OnInit()
    {
        base.OnInit();
        foreach (var item in intervenerList)
        {
            // Debug.Log("item.name: " + item.ScoreSituation);
        }
    }
    protected override List<int> OnSearchByName(string keyword)
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].name.Contains(keyword))
            {
                indexList.Add(i);
            }
        }
        return indexList;
    }
}
