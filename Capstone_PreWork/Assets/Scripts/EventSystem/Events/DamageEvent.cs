using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : Event
{
    public GameObject attackingObj { get; private set; }
    public GameObject damagedObj { get; private set; }
    public float damageAmount { get; private set; }


    public DamageEvent(GameObject attacking, GameObject damaged, float value, int priority) : base(EventType.DAMAGE_EVENT, priority)
    {
        attackingObj = attacking;
        damagedObj = damaged;
        damageAmount = value;
    }
}
