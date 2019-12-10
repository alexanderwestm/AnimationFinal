using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAugment : MonoBehaviour
{

    //Take the string name the button press passes and switch the augment accordingly 
    public void SetAugment(string name)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        AugmentDataScriptableObject scriptableObject = (AugmentDataScriptableObject)Resources.Load("ScriptableObjects/Augments/" + name);
        player.GetComponent<PassiveAugment>().data = scriptableObject;
        player.GetComponent<ActiveAugment>().data = scriptableObject;
        player.GetComponent<ActiveAugment>().SetStartValues();
    }
}
