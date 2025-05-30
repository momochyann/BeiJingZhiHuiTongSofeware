using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using Spire.Doc.Fields;
public class WorkSceneSystem : AbstractSystem
{
    public int WorkSceneIndex;
    public string 干预者 { 
        get { 
            if (string.IsNullOrEmpty(s_干预者)) 
                return 默认人员; 
            else 
                return s_干预者; 
        } 
        set { 
            s_干预者 = value; 
        } 
    }
    string 默认人员 = "未指定";
    string s_干预者;
    protected override void OnInit()
    {
        WorkSceneIndex = 0;
    }
}
