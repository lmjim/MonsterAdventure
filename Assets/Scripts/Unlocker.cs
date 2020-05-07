using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    public GameObject players;
    private GameObject player;
    private PlayerController controller;

    // There is a probably a cleaner way to do this but I think this is okay for now
    public bool unlocksSprinting;
    public bool unlocksDoubleJumping;
    public bool unlocksWallJumping;

    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);
        controller = player.GetComponent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)

    {
        if (other.gameObject.CompareTag("Player"))
        {
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
