/*
This is the script for the portal. 
Once the portal is activated, standing on it will end the level. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Portal : MonoBehaviour
{
    private ParticleSystem ps;
    public Text winText;
    
    void Start()
    {
        ps = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        winText.text = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on into the portal
        {
            if(ps.isEmitting)
            {
                //print("Level Complete! Stars collected: " + Star.starsCollected.ToString()); // TODO update this with end level
                winText.text = "Level Complete! Stars collected: " + Star.starsCollected.ToString();

            }
        }
    }
}
