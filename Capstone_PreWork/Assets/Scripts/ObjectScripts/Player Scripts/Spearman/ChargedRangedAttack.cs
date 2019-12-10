using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChargedRangedAttack : Ability
{
    [SerializeField] float arrowSpeed;
    public ChargedRangedAttack(GameObject obj, float timeRan) : base(obj, timeRan)
    {

    }

    public ChargedRangedAttack(ChargedRangedAttack other) : base(other)
    {
        arrowSpeed = other.arrowSpeed;
    }

    public override void Execute()
    {
        base.Execute();
        Shoot();
    }

    void Shoot()
    {
        timeRan = GameTimer.GlobalTimer.time;
        GameObject projectile = Object.Instantiate(hitBox, holder.transform.position + holder.transform.forward * 1.25f, holder.transform.rotation);
        projectile.GetComponent<HitboxScript>().Reset(damage);
        projectile.GetComponent<Rigidbody>().AddForce(holder.transform.forward * arrowSpeed);
        ((SpearmanState)state).SetState(CharacterState.CharacterStates.IDLE);
    }
}
