using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BasicEnemy : Health
{
    [SerializeField] Steering steering;

    // Start is called before the first frame update
    void Start()
    {
        steering.SetOwner(transform);
    }
}
