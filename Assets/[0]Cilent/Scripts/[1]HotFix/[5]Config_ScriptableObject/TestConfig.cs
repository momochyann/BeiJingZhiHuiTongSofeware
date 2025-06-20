using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Objectivesssss
{
    public string ss ;
    public string ss1 ;
     public List<题目> 题目列表 ;
     public 记分规则 记分规则;
}


[CreateAssetMenu(fileName = "TestConfig", menuName = "IncidentConfig/TestConfig")]
public class TestConfig : ScriptableObject
{

    public List<Objectivesssss> assessmentList2;
    // public string ss;
    // public string ss1;
}
