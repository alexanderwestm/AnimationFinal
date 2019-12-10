using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : EventListener
{
    public float damageBack;
    public float breakDamageBack;
    public float health;

    bool damaged = false;

    Dictionary<GameObject, float> gameObjectDamageReduc;
    private void Awake()
    {
        gameObjectDamageReduc = new Dictionary<GameObject, float>();
    }

    private void FixedUpdate()
    {
        damaged = false;
    }

    public override void HandleEvent(Event incomingEvent)
    {
        DamageEvent damageEvent = (DamageEvent)incomingEvent;
        if (!damaged)
        {
            if (damageEvent.attackingObj.transform.root.tag == "Boss" && damageEvent.damagedObj == gameObject)
            {
                health -= damageEvent.damageAmount;
                if (health <= 0)
                {
                    if (breakDamageBack > 0)
                    {
                        EventManager.GetInstance().QueueEvent(new DamageEvent(gameObject, damageEvent.attackingObj.transform.root.gameObject, breakDamageBack, 1));
                    }
                    Destroy(gameObject);
                }
                else if (damageBack > 0)
                {
                    EventManager.GetInstance().QueueEvent(new DamageEvent(gameObject, damageEvent.attackingObj.transform.root.gameObject, damageBack, 1));
                }
                damaged = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger");
        if((other.tag == "Player" || other.tag == "PlayerClone") && !gameObjectDamageReduc.ContainsKey(other.gameObject))
        {
            CharacterSkillSet skillSet = other.GetComponent<CharacterSkillSet>();
            float damageReduc = skillSet.damageTakenModifier;
            gameObjectDamageReduc.Add(other.gameObject, damageReduc);
            skillSet.damageTakenModifier -= damageReduc;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Player" || other.tag == "PlayerClone") && gameObjectDamageReduc.ContainsKey(other.gameObject))
        {
            CharacterSkillSet skillSet = other.GetComponent<CharacterSkillSet>();
            skillSet.damageTakenModifier += gameObjectDamageReduc[other.gameObject];
            gameObjectDamageReduc.Remove(other.gameObject);
        }
    }

    private void OnDestroy()
    {
        // reset the damage taken when the bubble dies
        CharacterSkillSet skillSet;
        foreach (KeyValuePair<GameObject, float> pair in gameObjectDamageReduc)
        {
            skillSet = pair.Key.GetComponent<CharacterSkillSet>();
            skillSet.damageTakenModifier += pair.Value;
        }
        EventManager.GetInstance().RemoveListener(this, EventType.DAMAGE_EVENT);
    }
}
