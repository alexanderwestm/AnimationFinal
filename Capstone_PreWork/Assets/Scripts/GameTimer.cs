using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer GlobalTimer { get; private set; }

    public float time;

    private void Awake()
    {
        if(GlobalTimer == null)
        {
            GlobalTimer = this;
        }
        time = 0;
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        EventManager.GetInstance().DischargeQueue();
    }

    public void ResetTimer()
    {
        time = 0;
    }
}
