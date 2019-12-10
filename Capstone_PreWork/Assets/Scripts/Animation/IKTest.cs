using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public float effectorLength;
    // 0: base, 1: one we're solving for
    public List<Transform> jointTransforms;
    public Transform endEffector;
    public Transform constraintLocator;
    float totalLength;

    private void Awake()
    {
        totalLength = jointTransforms.Count * effectorLength;
    }

    private void Update()
    {
        // this is for a pair of joints with a plane constraint and an end effector
        // if I want to expand this then I would need to make it so that it works for any joint and then solves the joints backwards

        // d vector
        Vector3 distanceBaseEnd = endEffector.position - jointTransforms[0].position;
        float baseEndLength = distanceBaseEnd.magnitude;

        Vector3 distanceBaseConstraint = constraintLocator.position - jointTransforms[0].position;
        // n vector
        Vector3 normalPlane = Vector3.Cross(distanceBaseEnd, distanceBaseConstraint);
        // dhat
        Vector3 normalizedDistanceBaseEnd = distanceBaseEnd.normalized;
        normalPlane.Normalize();
        normalPlane = -normalPlane; // blue axis


        // red axis
        // use this one for right
        Vector3 baseJointTangent = (jointTransforms[1].position - jointTransforms[0].position).normalized;
        // use this one for right
        Vector3 jointEndTangent = (endEffector.position - jointTransforms[1].position).normalized;



        if (baseEndLength <= totalLength)
        {
            Debug.Log("In Range");
            // solving location
            // c vector

            Vector3 hHat = Vector3.Cross(normalizedDistanceBaseEnd, normalPlane);

            // heron's formula
            float s = .5f * (effectorLength + effectorLength + baseEndLength);
            float area = Mathf.Sqrt(s * (s - baseEndLength) * (s - effectorLength) * (s - effectorLength));

            float height = area * 2 / baseEndLength;
            float D = Mathf.Sqrt(effectorLength * effectorLength - height * height);

            jointTransforms[1].position = jointTransforms[0].position + (D * normalizedDistanceBaseEnd) + (height * hHat);

            //// green axis
            //baseJointBiNormal = Vector3.Cross(baseJointTangent, normalPlane);
            //Vector3 jointEndBiNormal = Vector3.Cross(jointEndTangent, normalPlane);
        }
        else
        {
            jointTransforms[1].position = jointTransforms[0].position + normalizedDistanceBaseEnd * effectorLength;
        }

        
        jointTransforms[0].right = baseJointTangent;
        jointTransforms[1].right = jointEndTangent;

    }

    //Vector3 baseJointTangent, baseJointBiNormal, n;

    private void OnDrawGizmos()
    {
        foreach (Transform transform in jointTransforms)
        {
            Gizmos.DrawLine(transform.position, transform.position + effectorLength * transform.right);
        }
    }
}
