using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "GroupCrisisIncidentOptionConfig", menuName = "IncidentConfig/GroupCrisisIncidentOptionConfig")]
public class GroupCrisisIncidentOptionConfig : ScriptableObject
{
    public List<GroupCrisisIncidentType> personalCrisisIncidentTypes;
    public List<GroupCrisisIncidentType> publicCrisisIncidentTypes;
}
