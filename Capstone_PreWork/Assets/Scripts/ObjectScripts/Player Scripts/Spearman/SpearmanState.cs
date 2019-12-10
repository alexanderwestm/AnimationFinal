using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpearmanState : CharacterState
{
    Animator anim;
    //public enum SpearmanStates
    //{
    //    IDLE,
    //    ATTACKING,
    //    DASHING,
    //    CHARGING,
    //    CHARGING_RANGED
    //}
    //
    //[SerializeField] SpearmanStates currentState;
    void Awake()
    {
        TryGetComponent(out anim);
    }

    public override void Update()
    {
        base.Update();

        if (isGrounded || heightOffGround < 2f)
        {
            anim.SetBool("inAir", false);
        }
        else
        {
            anim.SetBool("inAir", true);
        }
    }
    public override float GetSpeedMod()
    {
        return 1;
    }
}
