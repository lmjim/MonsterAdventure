using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public GameObject players;
    private GameObject player;
    private PlayerController controller;
    
    // the same script can be attached to slimes that unlock different abilities
    public bool unlocksSprinting;
    public bool unlocksDoubleJumping;
    public bool unlocksWallJumping;

    private AudioSource audioSource;
    public AudioClip unlocker;
    public bool hasPlayed1 = false;
    public bool hasPlayed2 = false;
    public bool hasPlayed3 = false;

    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);
        controller = player.GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            if (unlocksSprinting)
            {
                if (!hasPlayed1)
                {
                 
                    StartCoroutine(PlayAndStop());
                    //audioSource.PlayOneShot(unlocker);
                
                    hasPlayed1 = true;
                }

                controller.canSprint = true;
                
            }
            if (unlocksDoubleJumping)
            {
                if (!hasPlayed2)
                {
                    
                    StartCoroutine(PlayAndStop());
                    //audioSource.PlayOneShot(unlocker);
                    hasPlayed2 = true;
                }
               
                controller.canDoubleJump = true;
            }
            if (unlocksWallJumping)
            {
                if (!hasPlayed3)
                {
                    StartCoroutine(PlayAndStop());
                    //audioSource.PlayOneShot(unlocker);
                    hasPlayed3 = true;
                }
                controller.canWallJump = true;
            }
        }
    }

    IEnumerator PlayAndStop() 
    {
        controller.CreateEffect();
        yield return new WaitForSeconds(0.8f);
        audioSource.PlayOneShot(unlocker);
        controller.StopEffect();
        
    }
    
}
