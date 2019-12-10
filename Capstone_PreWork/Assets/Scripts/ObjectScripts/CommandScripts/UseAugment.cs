using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAugment : Command
{
    bool state;
    public UseAugment(GameObject obj, float timeRan, bool state) : base(obj, timeRan)
    {
        this.state = state;
    }

    public override void Execute()
    {
        timeRan = GameTimer.GlobalTimer.time;
        gameObject.GetComponent<ActiveAugment>().UseAbility(state);
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
