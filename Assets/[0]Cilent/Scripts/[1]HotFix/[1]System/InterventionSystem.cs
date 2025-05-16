using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class InterventionSystem : AbstractSystem
{
    // Start is called before the first frame update
    public IndividualInterventionArchive 当前干预档案 { get; set; }
    public PersonalPersonnelCrisisEventMessage 当前人员 { get; set; }
    public List<string> interventionDescription { get; set; }

    protected override void OnInit()
    {
        当前干预档案 = new IndividualInterventionArchive();
        interventionDescription = new List<string>();
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
        interventionDescription.Add("");
    }


}
