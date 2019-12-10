using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : EventListener
{
    public float health;
    public float modifier;
    public float distance;
    public bool affectsClones = false;

    bool damaged = false;

    void Start()
    {
        EventManager.GetInstance().AddListener(this, EventType.DAMAGE_EVENT);
    }

    private void FixedUpdate()
    {
        damaged = false;
    }

    public override void HandleEvent(Event incomingEvent)
    {
        DamageEvent damageEvent = (DamageEvent)incomingEvent;
        if (!damaged)
        {
            if (damageEvent.damagedObj == gameObject)
            {
                Vector3 halfExtents = Vector3.one * .5f;
                halfExtents.x *= transform.localScale.x;
                halfExtents.y *= transform.localScale.y;
                halfExtents.z *= transform.localScale.z;
                int layerMask = (1 << 9); // player layer
                if (affectsClones) layerMask = layerMask | (1 << 10); // clone layer
                RaycastHit[] hits = Physics.BoxCastAll(transform.position, halfExtents, -transform.forward, Quaternion.identity, distance, layerMask);
                foreach (RaycastHit hit in hits)
                {
                    hit.collider.GetComponent<Health>().shieldAmount += damageEvent.damageAmount * modifier;
                }
                health -= damageEvent.damageAmount;
                if (health <= 0)
                {
                    EventManager.GetInstance().RemoveListener(this, EventType.DAMAGE_EVENT);
                    Destroy(gameObject);
                }
                damaged = true;
            }
        }
    }
}
