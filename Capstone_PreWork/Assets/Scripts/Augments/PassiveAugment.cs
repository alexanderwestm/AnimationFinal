using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

[System.Serializable]
public enum AugmentType
{
    INVALID_TYPE = -1,
    SOUL,           // yes
    //SOUL_SOUL,      // yes
    SOUL_METAL,     // yes
    SOUL_MIST,      // yes
    SOUL_BLOOD,     // yes
    METAL,          // yes
    METAL_METAL,    // yes
    METAL_MIST,     // yes
    METAL_BLOOD,    // yes
    MIST,           // yes
    MIST_MIST,      // yes
    MIST_BLOOD,     // yes
    BLOOD,          // yes
    //BLOOD_BLOOD     // yes
}

/*
 * Not every variable is going to be used for every passive
 * Cooldown: how long until you can use the ability again
 * Active Time: how long the ability is available
 * Threshhold: the amount needed to reach to activate
 * Percentage Mod: decimal value of how much to add
 *                  ex: (.25 = 25%, -.25 = -25%)
 *                  if taking away do -value
 *                  if adding do +value
 * Flat Value: value used for flat value passives (Thorns: damage dealt)
 */

[System.Serializable]
public class PassiveAugment : EventListener
{
    public AugmentDataScriptableObject data;

    [SerializeField] CharacterState holderState;
    [SerializeField] CharacterSkillSet holderSkillSet;

    // if the cooresponding value has already been augmented, rest on canceling
    [SerializeField] bool augmentedValues;

    [SerializeField] float timeRan;
    [SerializeField] bool canExecute { get { if (data == null) return false; return GameTimer.GlobalTimer.time - timeRan > data.passiveVariables["cooldown"]; } }

    public float damageDealt;

    public Sprite[] elementIcons;
        
    public Sprite[] effectIcons;

    [SerializeField] ParticleSystem healParticles;
    [SerializeField] ParticleSystem invisibleParticles;
    [SerializeField] GameObject barrier;
    [SerializeField] ParticleSystem increasedDamageParticles;
    [SerializeField] ParticleSystem intangibleParticles;
    [SerializeField] ParticleSystem lifeStealParticles; 

    private void Start()
    {
        holderState = GetComponent<CharacterState>();
        holderSkillSet = GetComponent<CharacterSkillSet>();
        EventManager.GetInstance().AddListener(this, EventType.DAMAGE_EVENT);
    }

    private void Update()
    {
        if (!augmentedValues && (data.type == AugmentType.METAL || data.type == AugmentType.METAL_METAL))
        {
            augmentedValues = true;
            holderSkillSet.damageTakenModifier += data.passiveVariables["modifier"];
        }
        if (canExecute)
        {
            switch (data.type)
            {
                case AugmentType.MIST_BLOOD:
                    {
                       
                        // check if character is stealthed, if stealthed increase their damage by %
                        if (holderState.isInvisible && !augmentedValues)
                        {
                            invisibleParticles.Play();
                            increasedDamageParticles.Play(); 
                            augmentedValues = true;
                            holderSkillSet.damageModifier += data.passiveVariables["modifier"];

                            //UI Display damage increase icon
                            
                            GameObject icon = GameObject.Find("damage_up");
                            icon.GetComponent<Image>().enabled = true;
                        }
                        else if (!holderState.isInvisible && augmentedValues)
                        {
                            invisibleParticles.Play();
                            increasedDamageParticles.Play(); 
                            augmentedValues = false;
                            holderSkillSet.damageModifier -= data.passiveVariables["modifier"];

                            GameObject icon = GameObject.Find("damage_up");
                            icon.GetComponent<Image>().enabled = false;
                        }
                        break;
                    }
            }
        }

        //Switches the icons in the UI, won't be in Update after the selection UI is hooked up
        CheckDataType();


    }

    public override void HandleEvent(Event incomingEvent)
    {
        DamageEvent damageEvent = (DamageEvent)incomingEvent;
        bool isAttacker = damageEvent.attackingObj == gameObject;
        bool playerDamaged = false;
        if (damageEvent.damagedObj)
        {
            playerDamaged = damageEvent.damagedObj.tag == "PlayerClone" || damageEvent.damagedObj.tag == "Player";
        }
        bool teamAttack = damageEvent.attackingObj.tag == "PlayerClone" || damageEvent.attackingObj.tag == "Player";

        if (canExecute)
        {
            if (isAttacker)
            {
                switch (data.type)
                {
                    case AugmentType.SOUL:
                    {
                        // heal a % of the damage dealt
                        Health health = damageEvent.attackingObj.GetComponent<Health>();
                        health.SetHealth(health.currentHealth + damageEvent.damageAmount * data.passiveVariables["modifier"]);
                        healParticles.Play();
                        lifeStealParticles.Play(); 
                        break;
                    }
                    case AugmentType.SOUL_METAL:
                    {
                        // gain a barrier based on the amount of damage dealt
                        if (!augmentedValues)
                        {
                            barrier.SetActive(true); 
                            StartCoroutine(Barrier(damageEvent));
                        }
                        break;
                    }
                    case AugmentType.SOUL_MIST:
                    {
                        // if damage is higher than a specific value then gain invisibility
                        if (damageEvent.damageAmount >= data.passiveVariables["threshold"])
                        {
                            invisibleParticles.Play(); 
                            StartCoroutine(Stealth(damageEvent.damageAmount * data.passiveVariables["modifier"]));
                        }
                        break;
                    }
                    case AugmentType.BLOOD:
                    {
                        // damaged unit takes % higher damage for x seconds
                        if (!damageEvent.damagedObj.GetComponent<CharacterState>().isBleeding)
                        {
                            increasedDamageParticles.Play(); 
                            StartCoroutine(Bleed(damageEvent));
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            else if (teamAttack)
            {
                StartCoroutine(DamageDealt(damageEvent.damageAmount));
                switch (data.type)
                {
                    // not 100% sure on how to do this will look into this
                    case AugmentType.SOUL_BLOOD:
                    {
                        // if total damage dealt is greater than X heal % of it
                        // gain a damage boost for X seconds
                        if (GetSumDamage() > data.passiveVariables["threshold"] && !augmentedValues)
                        {
                            Health health = damageEvent.attackingObj.GetComponent<Health>();
                            health.SetHealth(health.currentHealth + damageEvent.damageAmount * data.passiveVariables["modifier"]);
                            StartCoroutine(DamageModifier(data.passiveVariables["activeTime"]));
                                //UI Display damage increase icon
                            healParticles.Play();
                            increasedDamageParticles.Play();                     


                            }
                            break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
            else if (playerDamaged)
            {
                switch (data.type)
                {
                    case AugmentType.METAL_MIST:
                    {
                        // taking X damage deals % back
                        if (damageEvent.damageAmount > data.passiveVariables["threshold"])
                        {
                            EventManager.GetInstance().QueueEvent(new DamageEvent(damageEvent.damagedObj, damageEvent.attackingObj.transform.root.gameObject, damageEvent.damageAmount * data.passiveVariables["modifier"], 0));
                            healParticles.Play();

                            }
                            break;
                    }
                    case AugmentType.METAL_BLOOD:
                    {
                        // when damaged deal X points back
                        EventManager.GetInstance().QueueEvent(new DamageEvent(damageEvent.damagedObj, damageEvent.attackingObj.transform.root.gameObject, data.passiveVariables["value"], 0));
                         

                        break;
                    }
                    case AugmentType.MIST:
                    {
                        // taking X damage stealths for Y seconds
                        if (damageEvent.damageAmount > data.passiveVariables["threshold"])
                        {

                                invisibleParticles.Play(); 
                                StartCoroutine(Stealth(data.passiveVariables["activeTime"]));
                        }
                        break;
                    }
                    case AugmentType.MIST_MIST:
                    {
                        // taking X damage stealths for Y seconds
                        if (damageEvent.damageAmount > data.passiveVariables["threshold"])
                        {
                                invisibleParticles.Play(); 
                                StartCoroutine(Stealth(data.passiveVariables["activeTime"]));
                        }
                        break;
                    }
                    default:
                    {
                        break;
                    }
                }
            }
        }
    }

    IEnumerator Barrier(DamageEvent damageEvent)
    {
        damageEvent.attackingObj.GetComponent<Health>().shieldAmount += damageEvent.damageAmount * data.passiveVariables["modifier"];
        timeRan = GameTimer.GlobalTimer.time;
        augmentedValues = true;
        yield return new WaitForSecondsRealtime(data.passiveVariables["activeTime"]);
        augmentedValues = false;
        damageEvent.attackingObj.GetComponent<Health>().shieldAmount = 0;
    }

    IEnumerator Stealth(float time)
    {
        GameObject icon = GameObject.Find("invisible");
        icon.GetComponent<Image>().enabled = true;
        holderState.isInvisible = true;
        timeRan = GameTimer.GlobalTimer.time;
        // this needs to be based off the damage dealt
        yield return new WaitForSecondsRealtime(time);
        icon.GetComponent<Image>().enabled = false;
        holderState.isInvisible = false;
    }

    IEnumerator Bleed(DamageEvent damageEvent)
    {
        GameObject icon = GameObject.Find("bleed");
        icon.GetComponent<Image>().enabled = true;
        damageEvent.damagedObj.GetComponent<CharacterSkillSet>().damageTakenModifier += data.passiveVariables["modifier"];
        damageEvent.damagedObj.GetComponent<CharacterState>().isBleeding = true;
        yield return new WaitForSecondsRealtime(data.passiveVariables["activeTime"]);
        icon.GetComponent<Image>().enabled = false;
        damageEvent.damagedObj.GetComponent<CharacterSkillSet>().damageTakenModifier -= data.passiveVariables["modifier"];
        damageEvent.damagedObj.GetComponent<CharacterState>().isBleeding = false;
    }

    IEnumerator DamageDealt(float damageValue)
    {
        damageDealt += damageValue;
        yield return new WaitForSecondsRealtime(data.passiveVariables["damageTime"]);
        damageDealt -= damageValue;
    }

    IEnumerator DamageModifier(float time)
    {
        GameObject icon = GameObject.Find("damage_up");
        icon.GetComponent<Image>().enabled = true;
        holderSkillSet.damageModifier += data.passiveVariables["damageBonus"];
        augmentedValues = true;
        yield return new WaitForSecondsRealtime(time);
        augmentedValues = false;
        holderSkillSet.damageModifier -= data.passiveVariables["damageBonus"];
        icon.GetComponent<Image>().enabled = false;
    }

    float GetSumDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, data.passiveVariables["range"], 1 >> 10);
        float totalDamage = 0.0f;
        PassiveAugment augment;
        foreach (Collider col in hitColliders)
        {
            col.gameObject.TryGetComponent(out augment);
            totalDamage += augment.damageDealt;
        }
        return totalDamage;
    }


    //Sage's UI trash corner

    public void CheckDataType()
    {
        GameObject icon = GameObject.Find("equipped_slot_1");
        GameObject icon2 = GameObject.Find("equipped_slot_2");
        GameObject mod = GameObject.Find("intake_reduced");
        GameObject icon3 = GameObject.Find("thorns");
        if (icon != null && icon2 != null && mod != null && icon3 != null)
        {
            switch (data.type)
            {
                case AugmentType.SOUL:
                    {
                        //Directly grabs the image slot in the UI
                        //Assigns the sprite component of the image a sprite from an array 
                        icon.GetComponent<Image>().sprite = elementIcons[3];
                        icon2.GetComponent<Image>().sprite = elementIcons[3];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.SOUL_METAL:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[3];
                        icon2.GetComponent<Image>().sprite = elementIcons[1];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.SOUL_MIST:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[3];
                        icon2.GetComponent<Image>().sprite = elementIcons[2];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.SOUL_BLOOD:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[3];
                        icon2.GetComponent<Image>().sprite = elementIcons[0];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.METAL:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[1];
                        icon2.GetComponent<Image>().sprite = elementIcons[1];
                        mod.GetComponent<Image>().enabled = true;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.METAL_METAL:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[1];
                        icon2.GetComponent<Image>().sprite = elementIcons[1];
                        mod.GetComponent<Image>().enabled = true;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.METAL_BLOOD:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[1];
                        icon2.GetComponent<Image>().sprite = elementIcons[0];
                        icon3.GetComponent<Image>().enabled = true;
                        mod.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.MIST:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[2];
                        icon2.GetComponent<Image>().sprite = elementIcons[2];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.MIST_MIST:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[2];
                        icon2.GetComponent<Image>().sprite = elementIcons[2];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.BLOOD:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[0];
                        icon2.GetComponent<Image>().sprite = elementIcons[0];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.MIST_BLOOD:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[3];
                        icon2.GetComponent<Image>().sprite = elementIcons[0];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                case AugmentType.METAL_MIST:
                    {
                        icon.GetComponent<Image>().sprite = elementIcons[1];
                        icon2.GetComponent<Image>().sprite = elementIcons[2];
                        mod.GetComponent<Image>().enabled = false;
                        icon3.GetComponent<Image>().enabled = false;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }
}
