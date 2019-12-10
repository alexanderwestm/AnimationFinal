using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class CriusPlayerAgent : Agent
{
    [SerializeField] float minTimeBetweenActions;
    float timeSinceLastAction;
    GameObject[] players;
    EventManager eventManager;
    public enum Action
    {
        INVALID_ACTION = -1,
        SWING_LOW,
        SWING_HIGH,
        FROST_BREATH,
        ICICLE_THROW,
        NUM_ACTIONS
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        eventManager = EventManager.GetInstance();
    }
    // Update is called once per frame
    void Update()
    {
        if (timeSinceLastAction >= minTimeBetweenActions)
        {
            RequestDecision();
        }
        else
        {
            timeSinceLastAction += Time.deltaTime;
        }
    }
    public override void CollectObservations()
    {
        foreach (GameObject g in players)
        {
            AddVectorObs(g.transform.position);
        }
    }

    public override void InitializeAgent()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public override void AgentReset()
    {
        base.AgentReset();
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        char action = '.';
        switch (vectorAction[0])
        { 
            case 1:
                {
                    Debug.Log("Took the L");
                    action = 'L';
                    break;
                }
            case 2:
                {
                    action = 'H';
                    break;
                }
            case 3:
                {
                    action = 'F';
                    break;
                }

        }

            CriusEvent newEvent = new CriusEvent(0, action);
            eventManager.QueueEvent(newEvent);

            timeSinceLastAction = 0;
    }
}
