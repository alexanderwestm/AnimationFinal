using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasicAttack : Ability
{
    [SerializeField] float duration;
    [SerializeField] ParticleSystem basicAttackParticles; 


    public BasicAttack(GameObject obj, float timeRan) : base(obj, timeRan)
    {
    }
    
    public BasicAttack(BasicAttack other) : base(other)
    {
        duration = other.duration;
        basicAttackParticles = other.basicAttackParticles; 
    }

    public override void Execute()
    {
        base.Execute();
        ((SpearmanState)state).SetState(CharacterState.CharacterStates.ATTACKING);
        action = holder.StartCoroutine(Swing());
    }

    IEnumerator Swing()
    {
        holder.gameObject.GetComponent<Animator>().SetBool("meleeAttack", true);
        hitBox.SetActive(true);
        timeRan = GameTimer.GlobalTimer.time;
        yield return new WaitForSecondsRealtime(duration);
        hitBox.SetActive(false);
        ((SpearmanState)state).SetState(CharacterState.CharacterStates.IDLE);
        action = null;
        holder.gameObject.GetComponent<Animator>().SetBool("meleeAttack", false);
    }

    public override bool Cancel()
    {
        base.Cancel();
        if(action != null)
        {
            holder.gameObject.GetComponent<Animator>().SetBool("meleeAttack", false);
            return true;
        }
        return false;
    }

}
