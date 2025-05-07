using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "ScaleScoringConfig", menuName = "IncidentConfig/ScaleScoringConfig")]
public class ScaleScoringConfig : ScriptableObject
{
    public List<记分规则> 记分规则;
}


[System.Serializable]
public class 记分规则 
{
    public string 量表名称;
    public List<报告类别> 报告;
}
[System.Serializable]
public class 报告类别
{
    public List<结果类> 结果类别;
    public List<int> 记分题目;
}
[System.Serializable]
public class 结果类
{
    public int 数始;
    public int 数至;
    public string 结果文案;
    public string 建议;
}
