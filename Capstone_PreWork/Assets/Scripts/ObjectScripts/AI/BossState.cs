using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : EventListener
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F3))
        {
            EventManager.GetInstance().QueueEvent(new DamageEvent(null, GameObject.FindGameObjectWithTag("Player"), 100, 0));
            //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().TakeDamage(100);
        }

    }
}
