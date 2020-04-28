/*
This is the script for the star particle.
When the star appears, the player should be able to collect.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public static int starsCollected = 0; // this is a "global" variable so other scripts can access it
    
    private ParticleSystem ps;
    private bool collected = false;

    void Start()
    {
        ps = transform.GetChild(0).GetComponent<ParticleSystem>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            if(!collected & ps.isEmitting) // if the star hasn't already been collected and is visible
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); // collect the star
                starsCollected++; // keep track of the number of stars collected
                collected = true; // can't collect it again
            }
        }
    }

}
