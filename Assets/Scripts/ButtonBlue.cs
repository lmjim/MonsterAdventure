/*
This is the script for the blue button as well as part of the star. 
The star will not appear until after the player has activated the button.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBlue : MonoBehaviour
{
    public GameObject star;
    
    private ParticleSystem ps;
    private GameObject button;
    private Renderer buttonRender;
    private bool pressed = false;

    AudioSource audioSource;
    public AudioClip button_sound;

    void Start()
    {
        ps = star.transform.GetChild(0).GetComponent<ParticleSystem>();
        button = transform.GetChild(0).gameObject;
        buttonRender = button.GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps on the button
        {
            button.transform.position += Vector3.down * 2 * Time.deltaTime; // depress the button
            
            if(!pressed) // if the button hasn't already been pressed before
            {
                buttonRender.material.DisableKeyword("_EMISSION"); // turn off button light
                audioSource.PlayOneShot(button_sound); // click sound fx
                pressed = true; // can't press it again
                ps.Play(); // reveal the star
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player")) // when the player steps off the button
        {
            this.transform.GetChild(0).transform.position += Vector3.up * 2 * Time.deltaTime; // unpush the button
        }
    }
}
