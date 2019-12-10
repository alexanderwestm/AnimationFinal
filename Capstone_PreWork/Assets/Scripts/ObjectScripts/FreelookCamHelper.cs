using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FreelookCamHelper : MonoBehaviour
{
    GameObject lookAtPoint;
    bool set = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!set)
        {
            if (lookAtPoint == null)
            {
                lookAtPoint = GameObject.FindGameObjectWithTag("Player");
                foreach (Transform child in lookAtPoint.transform)
                {
                    if (child.name.ToLower() == "lookatpoint")
                    {
                        lookAtPoint = child.gameObject;
                        break;
                    }
                }
            }
            if(lookAtPoint != null)
            {
                CinemachineFreeLook freeLookCam = gameObject.GetComponent<CinemachineFreeLook>();
                freeLookCam.Follow = lookAtPoint.transform;
                freeLookCam.LookAt = lookAtPoint.transform;
            }
        }
    }
}
