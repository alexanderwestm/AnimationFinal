using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEvent : Event
{
    Collision collision;

    public CollisionEvent(EventType eventType, int eventPriority, Collision col) : base(eventType, eventPriority)
    {
        collision = col;
    }



    Collision GetCollision()
    {
        return collision;
    }
}
