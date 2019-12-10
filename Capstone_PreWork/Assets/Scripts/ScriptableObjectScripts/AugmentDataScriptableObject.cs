using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveData", menuName ="ScriptableObjects/Passive Augment Data")]
public class AugmentDataScriptableObject : ScriptableObject
{
    [Tooltip("Type of passive augment")]
    public AugmentType type;
    [Tooltip("Passive variables with their names: consult PassiveAugment.cs for names")]
    public CustomDictionary passiveVariables;
    [Tooltip("Active variables with their names: consult ActiveAugment.cs for names")]
    public CustomDictionary activeVariables;

    public string passiveDescription;
    public string activeDescription;
}
