using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RangedAttack : Ability
{
    [SerializeField] float arrowSpeed;
     ParticleSystem rangedParticles;
    ParticleSystem rangedParticlesHit; 

    public void Start ()
    { 
        rangedParticles = GameObject.Find("Spearman/AttackObjects/ps_arrow_shoot").GetComponent<ParticleSystem>();
        rangedParticlesHit = GameObject.Find("Spearman/AttackObjects/DoDamageParticles").GetComponent<ParticleSystem>(); 
    }


    public RangedAttack(GameObject obj, float timeRan) : base(obj, timeRan)
    {
       
    }

    public RangedAttack(RangedAttack other) : base(other)
    {
        arrowSpeed = other.arrowSpeed;
        rangedParticles = other.rangedParticles; 
    }

    public override void Execute()
    {
        base.Execute();
        Shoot();  
    }

    void Shoot()
    {
        holder.GetComponent<Animator>().SetTrigger("rangedAttack");
        Vector3 halfHeight = new Vector3(0, 2, 0);
        timeRan = GameTimer.GlobalTimer.time;
        GameObject projectile = Object.Instantiate(hitBox, holder.transform.position + holder.transform.forward + halfHeight, holder.transform.rotation);
        projectile.GetComponent<HitboxScript>().Reset(damage);
        projectile.GetComponent<HitboxScript>().SetOwner(holder.gameObject);
        projectile.GetComponent<Rigidbody>().AddForce(((SpearmanAttack)holder).rangedDirection * arrowSpeed);
        ((SpearmanState)state).SetState(CharacterState.CharacterStates.IDLE);
        if (rangedParticles)
        {
            rangedParticles.Play();
        } 
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Boss")
        {
            rangedParticlesHit.Play(); 
        }
    }

}
