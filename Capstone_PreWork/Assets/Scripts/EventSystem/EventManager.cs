using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EventManager
{
    private EventQueue queue;
    private int maxEventPerFrame = 500;
    //an array of lists
    private List<EventListener>[] eventListeners;

    private static EventManager instance;
    private EventManager()
    {
        if (queue == null)
        {
            queue = new EventQueue(500);
        }
        if (eventListeners == null)
        {
            //initalize array of lists
            eventListeners = new List<EventListener>[(int)EventType.NUM_TYPES];

            //initalize each list 
            for (int i = 0; i < (int)EventType.NUM_TYPES; ++i)
            {
                eventListeners[i] = new List<EventListener>();
            }
        }
    }

    /// <summary>
    /// This should only be called once a frame. Currently called in InputManager Update()
    /// </summary>
    public void DischargeQueue()
    {
        for (int i = 0; i < maxEventPerFrame; ++i)
        {
            Event toSend = queue.Pop();
            if (toSend == null)
            {
                break;
            }
            DispatchEvent(toSend);
        }
    }

    public void AddListener(EventListener toAdd, EventType listenFor)
    {
        if (eventListeners == null)
        {
            //initalize array of lists
            eventListeners = new List<EventListener>[(int)EventType.NUM_TYPES];


            //initalize each list 
            for (int i = 0; i < (int)EventType.NUM_TYPES; ++i)
            {
                eventListeners[i] = new List<EventListener>();
            }
        }
        eventListeners[(int)listenFor].Add(toAdd);
    }

    public void RemoveListener(EventListener toRemove, EventType removeFrom)
    {
        if(eventListeners != null)
        {
            eventListeners[(int)removeFrom].Remove(toRemove);
        }
    }

    public void QueueEvent(Event toAdd)
    {
        if (queue == null)
        {
            queue = new EventQueue(500);
        }

        

        if (!Application.isPlaying)
        {
            DispatchEvent(toAdd);
        }
        else
        {
            queue.Enqueue(toAdd);
        }

    }

    private void DispatchEvent(Event toSend)
    {

        if (toSend != null)
        {
            EventType sendType = toSend.GetEventType();
            //send the event to be handled by everything listening for that type of event
            for (int i = 0; i < eventListeners[(int)sendType].Count; ++i)
            {
                eventListeners[(int)sendType][i].HandleEvent(toSend);
            }
        }
        else
        {
            Debug.LogError("DISPATCHED NULL EVENT");
        }
    }

    public static EventManager GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManager();
        }
        return instance;
    }
}
