using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentUI : MonoBehaviour
{
    AugmentDataScriptableObject data;
    // every thing has a cooldown
    // every frame the ability is recharged by some percentage of this cooldown
    // aka charge += % * dt <= cooldown

    bool adjustedValues = false;

    float currentCharge;

    CharacterState characterState;
    CharacterSkillSet skillSet;

    // Start is called before the first frame update
    void Start()
    {
        characterState = GetComponent<CharacterState>();
        skillSet = GetComponent<CharacterSkillSet>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        currentCharge += data.activeVariables["rechargeAmount"];
        if (currentCharge > data.activeVariables["cooldown"])
        {
            currentCharge = data.activeVariables["cooldown"];
        }
    }

    public void UseAbility(bool state)
    {
        switch (data.type)
        {
            case AugmentType.SOUL:
                {
                  

                    break;
                }
            case AugmentType.SOUL_METAL:
                {
                   
                    break;
                }
            case AugmentType.SOUL_MIST:
                {
                    
                    break;
                }
            case AugmentType.SOUL_BLOOD:
                {
                   
                    break;
                }
            case AugmentType.METAL:
                {
                   
                    break;
                }
            case AugmentType.METAL_METAL:
                {
                   
                    break;
                }
            case AugmentType.METAL_BLOOD:
                {
                   
                    break;
                }
            case AugmentType.MIST:
                {
                   
                    break;
                }
            case AugmentType.MIST_MIST:
                {
                
                    break;
                }
            case AugmentType.BLOOD:
                {
                    
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
