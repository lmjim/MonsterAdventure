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
            audioSource.PlayOneShot(unlocker);
            if (unlocksSprinting)
            {
                controller.canSprint = true;
            }
            if (unlocksDoubleJumping)
            {
                controller.canDoubleJump = true;
            }
            if (unlocksWallJumping)
            {
                controller.canWallJump = true;
            }
        }
    }
}
