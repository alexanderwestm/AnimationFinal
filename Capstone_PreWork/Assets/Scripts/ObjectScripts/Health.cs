using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Health : EventListener
{
    public float maxHealth = 100;
    public float currentHealth;

    public float shieldAmount;

    Animator anim;
    [SerializeField] float invulPeriod;
    [SerializeField] ParticleSystem takeDamageParticles; 
    bool damaged;
    float timer;

    CharacterSkillSet skillSet;

    public bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        EventManager.GetInstance().AddListener(this, EventType.DAMAGE_EVENT);
        currentHealth = maxHealth;
        skillSet = GetComponent<CharacterSkillSet>();
    }

    void Update()
    {
        if (damaged)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                damaged = false;
            }
        }
    }

    public override void HandleEvent(Event incomingEvent)
    {
        if (!damaged)
        { 
        DamageEvent damageEvent = (DamageEvent)incomingEvent;
        float damageMod = skillSet.damageTakenModifier < 0 ? 0 : skillSet.damageTakenModifier;
            if (damageEvent.damagedObj == gameObject && !isDead)
            {
                if (shieldAmount <= 0)
                {
                    currentHealth -= damageEvent.damageAmount * damageMod;
                    if (takeDamageParticles)
                    {
                        takeDamageParticles.Play();
                    }
                }
                else
                {
                    shieldAmount -= damageEvent.damageAmount * damageMod;
                    if (shieldAmount <= 0)
                    {
                        currentHealth -= shieldAmount;
                        shieldAmount = 0;
                        takeDamageParticles.Play(); 
                    }
                }
                if (currentHealth <= 0)
                {
                    // dead
                    // this is different for player and for boss
                    // fire a dying event with the tag of the object that's dying, this can be read by another script to determine what to do on boss death or on player death
                    //takeDamageParticles.Play(); 
                    isDead = true;
                    anim.SetBool("Dead", true);
                    EventManager.GetInstance().QueueEvent(new DeathEvent(gameObject, 0));
                }
                else
                {
                    timer = invulPeriod;
                    damaged = true;
                }
            }
        }
    }

    public void Reset()
    {
        currentHealth = maxHealth;
        damaged = false;
        timer = 0;
        isDead = false;
    }

    public void SetHealth(float value)
    {
        currentHealth = value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
