using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Steering
{
    Vector3 targetLoc;
    Transform owner;
    Rigidbody ownerRB;


    [SerializeField] float arriveStopRadius;
    [SerializeField] float arriveSlowRadius;
    [SerializeField] float maxSpeed;
    [SerializeField] float maxAccel;

    // Start is called before the first frame update


    public void SetOwner(Transform newOwner)
    {
        owner = newOwner;
        ownerRB = owner.GetComponent<Rigidbody>();
    }
    public void SetTarget(Vector3 position)
    {
        targetLoc = position;
    }

    public bool Move()
    {
        
        float tempMaxSpeed = maxSpeed;
        //arrive and face steering

       



        //get direction vector
        Vector3 diff = targetLoc - owner.position;
        Vector3 normalDiff = diff.normalized;
        
        //only steer on the ground
        diff = new Vector3(diff.x, 0, diff.z);

        //face target location
        owner.transform.rotation = Quaternion.LookRotation(new Vector3(diff.x, 0f, diff.z));

        //avoid square root 
        if (diff.sqrMagnitude < arriveSlowRadius * arriveSlowRadius)
        {
            //stop if we're close enough with a bit of fudging
            if (diff.sqrMagnitude < arriveStopRadius * arriveStopRadius)
            {
                ownerRB.velocity = Vector3.zero;
                return false;
            }

            //speed is modified by the ratio between the distance between rb and stop radius and slow radius and stop radius, with a small increase so that we don't never reach the end
            tempMaxSpeed *= (( arriveStopRadius * arriveStopRadius - diff.sqrMagnitude) / (arriveStopRadius * arriveStopRadius - arriveSlowRadius * arriveSlowRadius)) + 0.1f;

        }

        //find our target velocity
        Vector3 targetVelocity = normalDiff * tempMaxSpeed;

        //find the difference between the actual and target velocities
        Vector3 velDiff = targetVelocity - ownerRB.velocity;

        if (velDiff.sqrMagnitude < maxAccel * maxAccel)
        {
            //if we can reach our target velocity this frame, set to it
            ownerRB.velocity = targetVelocity;
            return true;
        }


        //otherwise accelerate towards our target velocity
        ownerRB.velocity += maxAccel * Time.deltaTime * velDiff.normalized;
        return true;
    }
}
