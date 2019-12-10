using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public abstract class VariableSerializable<T>
{
    public string gameObjectName;
    public T value;

    public T GetValue()
    {
        return value;
    }

    public void SetValue(T input)
    {
        value = input;
    }
}

[System.Serializable]
public struct BoxColliderData
{
    public bool isTrigger;
    public Vector3 center;
    public Vector3 size;

    public BoxColliderData(BoxCollider collider)
    {
        isTrigger = collider.isTrigger;
        center = collider.center;
        size = collider.size;
    }

    public BoxColliderData Interpolate(BoxColliderData other, float param)
    {
        BoxColliderData returnData = new BoxColliderData();

        returnData.isTrigger = Convert.ToBoolean(Mathf.Round(Mathf.Lerp(Convert.ToInt32(isTrigger), Convert.ToInt32(other.isTrigger), param)));
        returnData.center = Vector3.Lerp(center, other.center, param);
        returnData.size = Vector3.Lerp(size, other.size, param);

        return returnData;
    }
}

[System.Serializable]
public class BoxColliderSerializable : VariableSerializable<BoxColliderData>
{
    public BoxColliderData Interpolate(BoxColliderSerializable other, float param)
    {
        return value.Interpolate(other.value, param);
    }
}

[Serializable]
public class BoxColliderKeyframe
{
    public List<BoxColliderSerializable> keyframes;
    public float sampleTime;

    public float Count { get { return keyframes.Count; } }

    public BoxColliderKeyframe()
    {
        keyframes = new List<BoxColliderSerializable>();
    }

    public BoxColliderSerializable this[int key]
    {
        get { return keyframes[key]; }
        set { keyframes[key] = value; }
    }

    public void Add(BoxColliderSerializable item)
    {
        keyframes.Add(item);
    }
}