using UnityEngine;

public class BossInputInterpreter : EventListener
{
    Vector3 moveVector = Vector3.zero;
    CharacterState characterState;
    CommandLogger logger;
    private void Awake()
    {
        EventManager eventManager = EventManager.GetInstance();
        eventManager.AddListener(this, EventType.CRIUS_EVENT);
        characterState = gameObject.GetComponent<CharacterState>();
        logger = gameObject.GetComponent<CommandLogger>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (logger == null)
        {
            logger = gameObject.GetComponent<CommandLogger>();
        }
    }

    public override void HandleEvent(Event incomingEvent)
    {
        base.HandleEvent(incomingEvent);
        switch (incomingEvent.GetEventType())
        {
            case (EventType.CRIUS_EVENT):
            {
                    CriusEvent castedEvent = (CriusEvent)incomingEvent;
                    characterState.skillSet.HandleButtonEvent(castedEvent.GetAbility(), KeyState.NULL_STATE);
                    break;
            }
        }
    }
}
