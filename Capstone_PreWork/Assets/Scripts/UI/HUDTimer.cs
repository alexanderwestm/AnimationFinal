using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDTimer :  MonoBehaviour
{ 
    Text tx;
    [SerializeField] bool isPreviousTime;
    float previousTime;
    BossWall bossWall;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent(out tx);
        bossWall = FindObjectOfType<BossWall>();
    }



    // Update is called once per frame
    void Update()
    {
        if (bossWall.isInArena)
        {
            if (!isPreviousTime)
            {
                tx.text = GameTimer.GlobalTimer.time.ToString("0.00");
            }
            else
            {
                if (GameTimer.GlobalTimer.time > previousTime)
                {
                    previousTime = GameTimer.GlobalTimer.time;
                }
                tx.text = previousTime.ToString("0.00");
            }
        }
    }
}
