using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocker : MonoBehaviour
{
    // the same script can be attached to slimes that unlock different abilities
    public bool unlocksSprinting;
    public bool unlocksDoubleJumping;
    public bool unlocksWallJumping;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (unlocksSprinting)
            {
                PlayerController.canSprint = true;
            }
            if (unlocksDoubleJumping)
            {
                PlayerController.canDoubleJump = true;
            }
            if (unlocksWallJumping)
            {
                PlayerController.canWallJump = true;
            }
        }
    }
}
