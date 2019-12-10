using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * These go on weapons and attacks in order to deal damage (specifically the player)
 */
public class HitboxScript : MonoBehaviour
{
    Quaternion startRotation;
    float damage;
    bool dealtDamage = false;
    GameObject highLevelObj;
    // Start is called before the first frame update
    void Start()
    {
        startRotation = transform.rotation;
    }

    void OnTriggerEnter(Collider col)
    {
        //only hit once
        if (!dealtDamage)
        {
            if (col.TryGetComponent(out BasicEnemy enemy))
            {
                if(gameObject.transform.parent != null)
                {
                    highLevelObj = gameObject.transform.parent.gameObject; 
                }
                else if (highLevelObj == null)
                {
                    highLevelObj = gameObject;
                }
                float damageDeal = damage;
                if (transform.root.gameObject.GetComponent<CharacterSkillSet>() != null)
                {
                    damageDeal *= transform.root.gameObject.GetComponent<CharacterSkillSet>().damageModifier;
                }
                EventManager.GetInstance().QueueEvent(new DamageEvent(highLevelObj, col.gameObject, damageDeal, 0));
                //enemy.TakeDamage(damage);
                dealtDamage = true;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "Boss")
        {
            //only hit once
            if (!dealtDamage)
            {
                if (col.gameObject.TryGetComponent(out BasicEnemy enemy))
                {
                    if (gameObject.transform.parent != null)
                    {
                        highLevelObj = gameObject.transform.parent.gameObject;
                    }
                    else if (highLevelObj == null)
                    {
                        highLevelObj = gameObject;
                    }
                    float damageDeal = damage;
                    if (transform.root.gameObject.GetComponent<CharacterSkillSet>() != null)
                    {
                        damageDeal *= transform.root.gameObject.GetComponent<CharacterSkillSet>().damageModifier;
                    }
                    EventManager.GetInstance().QueueEvent(new DamageEvent(highLevelObj, col.gameObject, damageDeal, 0));
                    //enemy.TakeDamage(damage);
                    dealtDamage = true;
                    Destroy(gameObject);

                }
            }
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject.GetComponent<Collider>());
            transform.rotation = startRotation;
            transform.parent = col.transform;
            Destroy(gameObject, 5);
        }
    }

    public void Reset(float newDamage)
    {
        damage = newDamage;
        dealtDamage = false;

        for ( int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).TryGetComponent(out HitboxScript script))
            {
                script.Reset(newDamage);
            }
        }
      
    }
    public void SetOwner(GameObject g)
    {
        highLevelObj = g;
    }
}
