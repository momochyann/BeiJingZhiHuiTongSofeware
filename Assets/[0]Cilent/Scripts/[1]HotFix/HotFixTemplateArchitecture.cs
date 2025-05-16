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
        RegisterSystem(new InterventionSystem());

        RegisterModel(new PersonalPersonnelCrisisEventMessageModel());
        RegisterModel(new YooAssetPfbModel());
        RegisterModel(new GroupCrisisIncidentModel());
        RegisterModel(new GroupPersonnelCrisisEventMessageModel());
        RegisterModel(new ObjectiveAssessmentModel());
        RegisterModel(new ObjectiveAssessmentArchiveModel());
        RegisterModel(new SubjectiveAssessmentArchiveModel());
        RegisterModel(new IndividualInterventionArchiveModel());

        RegisterUtility(new Storage());
        RegisterUtility(new ExcelReader());

    }

}
