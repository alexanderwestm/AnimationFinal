using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dash : Ability
{
    [SerializeField] float duration;
    [SerializeField] float speed;
    [SerializeField] float strafeDurationMod;
    private Vector3 direction;

    [SerializeField] ParticleSystem dashParticles; 

    public Dash(GameObject obj, Vector2 direction, float timeRan) : base(obj, timeRan)
    {

    }

    public Dash(Dash other) : base(other)
    {
        duration = other.duration;
        speed = other.speed;
        strafeDurationMod = other.strafeDurationMod;
        dashParticles = other.dashParticles;
        direction = other.direction;
    }

    public override void Execute()
    {
        base.Execute();        
    }

    public override bool Cancel()
    {
        throw new System.NotImplementedException();
    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    IEnumerator DirectionalDash()
    {
        Debug.Log("Dash Duration: " + duration);
        Vector3 targetVel = direction;
        if(direction == Vector3.zero)
        {
            targetVel = holder.transform.forward;
        }

        targetVel *= speed * Time.fixedDeltaTime;
        rb.velocity += new Vector3(targetVel.x, 0, targetVel.z);
        yield return new WaitForSecondsRealtime(duration);
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
        ((SpearmanState)state).SetState(CharacterState.CharacterStates.IDLE);
    }
}
