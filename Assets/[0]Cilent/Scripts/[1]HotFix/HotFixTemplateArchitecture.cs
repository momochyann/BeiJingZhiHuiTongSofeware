using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using System.Text;
public class HotFixTemplateArchitecture : Architecture<HotFixTemplateArchitecture>
{
    protected override void Init()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        RegisterSystem(new GetCan2ListModelByStringSystem());
        RegisterSystem(new SearchEntrySystem());
        RegisterSystem(new ObjectiveSelectSystem());
        RegisterSystem(new InterventionSystem());
        RegisterSystem(new WorkSceneSystem());

        RegisterModel(new PersonalPersonnelCrisisEventMessageModel());
        RegisterModel(new YooAssetPfbModel());
        RegisterModel(new GroupCrisisIncidentModel());
        RegisterModel(new GroupPersonnelCrisisEventMessageModel());
        RegisterModel(new ObjectiveAssessmentModel());
        RegisterModel(new ObjectiveAssessmentArchiveModel());
        RegisterModel(new SubjectiveAssessmentArchiveModel());
        RegisterModel(new IndividualInterventionArchiveModel());
        RegisterModel(new IntervenersModel());


        RegisterUtility(new Storage());
        RegisterUtility(new ExcelReader());

    }

}
