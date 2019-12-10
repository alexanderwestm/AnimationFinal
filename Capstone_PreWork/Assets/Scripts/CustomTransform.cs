using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTransform
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public float time;

    public CustomTransform()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        scale = Vector3.one;
    }

    public CustomTransform(Transform transform, float t)
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
        time = t;
    }

    public static CustomTransform Interpolate(CustomTransform a, CustomTransform b, float param)
    {
        CustomTransform returnTransform = new CustomTransform();

        returnTransform.position = Vector3.Lerp(a.position, b.position, param);
        returnTransform.rotation = Quaternion.Slerp(a.rotation, b.rotation, param);
        returnTransform.scale = Vector3.Lerp(a.scale, b.scale, param);

        return returnTransform;
    }
}