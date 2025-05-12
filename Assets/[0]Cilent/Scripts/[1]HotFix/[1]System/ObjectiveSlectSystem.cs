using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class ObjectiveSelectSystem : AbstractSystem
{
    public int 当前题序;
    public ObjectiveAssessment 当前量表 { get; set; }
    public List<int> 当前量表得分;
    public PersonalPersonnelCrisisEventMessage 当前人员 { get; set; }
    public string 当前干预人员;
    public SubjectiveAssessmentArchive 当前主观评估 { get; set; }
    protected override void OnInit()
    {
        当前题序 = 0;
        当前量表 = this.GetModel<ObjectiveAssessmentModel>().GetObjectiveAssessments()[0];
        当前量表得分 = new List<int>();
        当前主观评估 = new SubjectiveAssessmentArchive();
    }

    
}
