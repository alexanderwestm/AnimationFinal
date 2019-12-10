using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] GameObject jumpLandingParticles;

    ParticleSystem particles; 

    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("Jump landed"); 
        if (col.gameObject.tag.Equals("Ground"))
        {
            GameObject g = Instantiate(jumpLandingParticles, transform.position, jumpLandingParticles.transform.rotation);

            Destroy(g, 5.0f);

        }
    }
}
