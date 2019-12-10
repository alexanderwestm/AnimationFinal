using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterState : MonoBehaviour
{
    public MovementControlsScriptableObject movementControls;
    public GameObject currentHitbox;

    public enum CharacterStates
    {
        STUNNED = -1,
        IDLE,
        ATTACKING,
        DASHING,
        CHARGING,
        CHARGING_RANGED,
        USING_ABILITY
    }


    //public enum SuperState
    //{
    //    ATTACKING,
    //    BLOCKING,
    //    ATTACK_AND_BLOCK,
    //    IDLE,
    //    UNABLE_TO_ACT
    //}

    [SerializeField] public CharacterStates currentState { get; protected set; } = CharacterStates.IDLE;

    public bool isGrounded = true, shouldJump = false, shouldDash = false;
    public bool isInvisible = false;
    public bool isBleeding = false;
    public CharacterSkillSet skillSet;

    protected float heightOffGround;

    public virtual void Update()
    {
        isGrounded = GetIsGrounded();
    }

    public virtual bool GetCanMove()
    {
        return currentState != CharacterStates.STUNNED && currentState != CharacterStates.USING_ABILITY && currentState != CharacterStates.DASHING && currentState != CharacterStates.CHARGING;
    }

    public virtual float GetSpeedMod()
    {
        return 1;
    }

    public bool GetIsGrounded()
    {
        Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.25f);
        
        if (hit.collider == null)
        {
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit testHit, 10f);
            heightOffGround = testHit.distance;
        }
        return hit.collider != null;
    }

    public void SetState(CharacterStates newState)
    {
        currentState = newState;
    }

    public virtual CharacterStates GetSuperState()
    {
        return currentState;
    }
}
