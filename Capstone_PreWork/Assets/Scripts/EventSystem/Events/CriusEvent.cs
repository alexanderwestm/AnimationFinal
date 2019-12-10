using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriusEvent : Event
{
    private char abilityUsed;
    public CriusEvent(int eventPriority, char attack) : base(EventType.CRIUS_EVENT, eventPriority)
    {
        abilityUsed = attack;
    }

    public char GetAbility()
    {
        return abilityUsed;
    }
}
