using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    INVALID_TYPE = -1,
    BALL_COLLISION,
    MOUSE_INPUT_EVENT,
    MOUSE_MOVE_EVENT,
    KEY_INPUT_EVENT,
    CRIUS_EVENT,
    DAMAGE_EVENT,
    DEATH_EVENT,
    NUM_TYPES
}

public class Event
{
    private EventType type;
    private int priority;

    public Event(EventType eventType, int eventPriority)
    {
        type = eventType;
        priority = eventPriority;
    }


    public void SetPriority(int newPriority)
    {
        priority = newPriority;
    }

    public int GetPriority()
    {
        return priority;
    }

    public EventType GetEventType()
    {
        return type;
    }
    
}
