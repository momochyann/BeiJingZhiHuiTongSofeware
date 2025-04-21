using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "PersonalCrisisEventPropertyConfig", menuName = "IncidentConfig/PersonalCrisisEventPropertyConfig")]
public class PersonalCrisisEventPropertyConfig : ScriptableObject
{
    public List<PersonalCrisisEventProperty> personalCrisisEventProperties;
}