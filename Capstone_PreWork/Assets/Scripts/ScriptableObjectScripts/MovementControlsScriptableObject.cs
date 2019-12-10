using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ControlVariables
{
    public float speedPerFrame, jumpStrength, jumpMultiplier, fallMultiplier, airSpeed, maxAirSpeed, dashSpeed, dashDuration;
}

[CreateAssetMenu(fileName = "MovementControls", menuName ="ScriptableObjects/MovementControls")]
public class MovementControlsScriptableObject : ScriptableObject
{
    public ControlVariables controlVariables;
}
