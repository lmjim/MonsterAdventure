using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomeController : MonoBehaviour
{
    AudioSource playerAudio;

    private Animator playerAnimator;
    private Rigidbody playerRB;
    private Vector3 playerMovement;
    private Quaternion playerRotation = Quaternion.identity;
    private float turnSpeed = 20f;
    private float movementSpeed = 4f;

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Walking Functionality
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        playerMovement.Set(horizontal, 0f, vertical);
        playerMovement.Normalize();
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool walkForward = hasHorizontalInput || hasVerticalInput;
        playerAnimator.SetBool("Walk Forward", walkForward);
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, playerMovement, turnSpeed * Time.deltaTime, 0f);
        playerRotation = Quaternion.LookRotation(desiredForward);

        if (walkForward)
        {
            if (!playerAudio.isPlaying)
            {
                playerAudio.Play();
            }
        
        }
        else
        {
            playerAudio.Stop();
        }
    }

    void OnAnimatorMove()
    {
        playerRB.MovePosition(playerRB.position + playerMovement * playerAnimator.deltaPosition.magnitude * movementSpeed);
        playerRB.MoveRotation(playerRotation);
    }
}
