 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriusState : CharacterState
{
    public enum CriusStates
    {
        IDLE,
        CHAIN_SWING,
        SWING_HIGH,
        FROST_BEAM,
        ICICLE_THROW
    }

    CriusStates currentState;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out skillSet);
    }

    // Update is called once per frame
    void Update()
    {
        if (!skillSet)
        {
            TryGetComponent(out skillSet);
        }
    }

    public void SetState(CriusStates newState)
    {
        currentState = newState;
    }

    public CriusStates GetState()
    {
        return currentState;
    }
    public override CharacterStates GetSuperState()
    {
        if (currentState != CriusStates.IDLE)
        {
           return CharacterStates.ATTACKING;
        }
         return CharacterStates.IDLE;
    }
}
