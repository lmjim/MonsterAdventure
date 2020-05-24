/*
This is the script for the star particle.
When the star appears, the player should be able to collect.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    private ParticleSystem ps;
    private bool collected = false;

    AudioSource audioSource;
    public AudioClip star;

    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            if(!collected & ps.isEmitting) // if the star hasn't already been collected and is visible
            {
                audioSource.PlayOneShot(star);
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // collect the star
                LevelTracker.starsCollected++; // keep track of the number of stars collected
                collected = true; // can't collect it again
            }
        }
    }

}
