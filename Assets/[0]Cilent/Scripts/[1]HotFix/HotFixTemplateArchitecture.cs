using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;

public class HotFixTemplateArchitecture : Architecture<HotFixTemplateArchitecture>
{
    protected override void Init()
    {
        RegisterSystem(new GetCan2ListModelByStringSystem());
        RegisterSystem(new SearchEntrySystem());
         RegisterSystem(new ObjectiveSelectSystem());
        RegisterUtility(new Storage());
        RegisterModel(new PersonalPersonnelCrisisEventMessageModel());
        RegisterModel(new YooAssetPfbModel());
        RegisterModel(new GroupCrisisIncidentModel());
        RegisterModel(new GroupPersonnelCrisisEventMessageModel());
        RegisterModel(new ObjectiveAssessmentModel());
        RegisterModel(new ObjectiveAssessmentArchiveModel());
        RegisterUtility(new ExcelReader());
       
    }

}
