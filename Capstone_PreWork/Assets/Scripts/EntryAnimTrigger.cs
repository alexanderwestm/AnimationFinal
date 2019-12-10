using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryAnimTrigger : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>();
        anim.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        anim.gameObject.SetActive(true);
        if (other.tag == "Player")
        {
            anim.SetTrigger("crius_angry");
        }
    }
}
