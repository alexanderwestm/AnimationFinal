using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ForceSelect : MonoBehaviour
{
    public Button selectButton;

    void OnEnable()
    {
        if (selectButton != null)
        {
            selectButton.Select();
            print("selected button");
        }
        else
        {
            Debug.Log("SelectButton was null");
        }
    }
}
