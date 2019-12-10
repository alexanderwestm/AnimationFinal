using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCommand : Command
{
    Quaternion rotation;
    public RotateCommand(GameObject obj, Quaternion rotation, float timeRan) : base(obj, timeRan)
    {
        this.rotation = rotation;
    }

    public override void Execute()
    {
        //Vector3 eulerAngle = rotation.eulerAngles;
        //gameObject.transform.rotation = Quaternion.Euler(0, eulerAngle.y, 0);
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}
