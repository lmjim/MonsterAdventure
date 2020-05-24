using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            LevelTracker.level1Passed = true;
            LevelTracker.learnedSprint = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            LevelTracker.level2Passed = true;
            LevelTracker.learnedDoubleJump = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            LevelTracker.level3Passed = true;
            LevelTracker.learnedWallJump = true;
        }
        if (Input.GetKeyDown("r"))
        {
            SceneManager.LoadScene("Home", LoadSceneMode.Single); // reload home screen so level buttons update
        }
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
