using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : Event
{
    public GameObject deadObject { get; private set; }

    public DeathEvent(GameObject obj, int priority) : base(EventType.DEATH_EVENT, priority)
    {
        deadObject = obj;
    }
}
