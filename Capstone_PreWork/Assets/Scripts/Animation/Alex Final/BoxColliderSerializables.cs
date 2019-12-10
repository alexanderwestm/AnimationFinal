using System.Collections.Generic;
using UnityEngine;

public class BoxColliderSerializables : ScriptableObject
{
    public List<BoxColliderKeyframe> boxColliders;

    public int Count { get { return boxColliders.Count; } }

    public BoxColliderKeyframe this[int key]
    {
        get { return boxColliders[key]; }
        set { boxColliders[key] = value; }
    }

    public void Insert(BoxColliderKeyframe item)
    {
        if (boxColliders == null)
        {
            boxColliders = new List<BoxColliderKeyframe>();
        }
        if (boxColliders.Count == 0)
        {
            boxColliders.Add(item);
        }
        else
        {
            for (int i = boxColliders.Count - 1; i >= 0; --i)
            {
                if (boxColliders[i].sampleTime < item.sampleTime)
                {
                    boxColliders.Insert(i + 1, item);
                    return;
                }
            }
        }
    }
}