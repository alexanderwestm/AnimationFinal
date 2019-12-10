using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSounds : MonoBehaviour
{

    [SerializeField] private AudioClip[] roars;
    [SerializeField] private AudioClip[] freezingBreaths;
    [SerializeField] private AudioClip[] breaths; 
    private AudioSource audioSource; 



    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

  
    private void Roar()
    {
        AudioClip clip = GetRandomRoarClip();
        audioSource.PlayOneShot(clip);

    }


    private AudioClip GetRandomRoarClip()
    {
        return roars[UnityEngine.Random.Range(0, roars.Length)]; 

    }

    private void FreezingBreath()
    {
        AudioClip clip = GetRandomFreezingBreathClip();
        audioSource.PlayOneShot(clip);

    }

    private AudioClip GetRandomFreezingBreathClip()
    {
        return freezingBreaths[UnityEngine.Random.Range(0, freezingBreaths.Length)];

    }

    private void Breath()
    {
        AudioClip clip = GetRandomBreathClip();
        audioSource.PlayOneShot(clip); 

    }

    private AudioClip GetRandomBreathClip()
    {
        return breaths[UnityEngine.Random.Range(0, breaths.Length)]; 

    }
}
