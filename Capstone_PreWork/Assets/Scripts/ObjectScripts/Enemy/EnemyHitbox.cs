using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Script attached to things that deal damage to the player
 */
public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] bool isBossContact;
    [SerializeField] float bossContactDamage;
    float damage = 0;
    bool dealtDamage = false;
    Coroutine resetter;
    ParticleSystem meleeParticles; 
    // Start is called before the first frame update
    void Start()
    {
        if (isBossContact)
        {
            damage = bossContactDamage;
        }
        if (GameObject.Find("Spearman/AttackObjects/ps_hit_land") != null)
        {
            //meleeParticles = GameObject.Find("Spearman/AttackObjects/ps_hit_land").GetComponent<ParticleSystem>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (!isBossContact)
        {
            float damageDeal = damage;
            if(transform.root.gameObject.GetComponent<CharacterSkillSet>() != null)
            {
                damageDeal *= transform.root.gameObject.GetComponent<CharacterSkillSet>().damageModifier;
            }
            EventManager.GetInstance().QueueEvent(new DamageEvent(gameObject, col.gameObject, damageDeal, 0));
          
        }
    }

    void OnCollisionEnter(Collision col)
    {
        float damageDeal = damage;
        if (transform.root.gameObject.GetComponent<CharacterSkillSet>() != null)
        {
            damageDeal *= transform.root.gameObject.GetComponent<CharacterSkillSet>().damageModifier;
        }
        EventManager.GetInstance().QueueEvent(new DamageEvent(gameObject, col.gameObject, damageDeal, 0));
       // if (meleeParticles != null)
        //{
           // meleeParticles.Play();
        //}
    }

    public void Reset(float newDamage)
    {
        damage = newDamage;

        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).TryGetComponent(out EnemyHitbox script))
            {
                script.Reset(newDamage);
            }
        }
    }


}
