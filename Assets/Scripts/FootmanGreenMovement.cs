/*
 * References:
 * - Rotate footman to look at boximon: https://www.youtube.com/watch?v=dp3lZUDij6Y 
 */
 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootmanGreenMovement : MonoBehaviour
{
    public GameObject player;
    private Animator playerAnimation;

    private Rigidbody footmanRigidbody;
    private Animator footmanAnimator;
    private CapsuleCollider footmanCollider;
    private CapsuleCollider swordCollider;
    private Vector3 newPosition;

    private float movementDistance = 30.0f;
    private float attackDistance = 3.5f;
    private float footmanViewAngle = 80.0f;
    private bool dead = false;

    void Start()
    {
        footmanRigidbody = GetComponent<Rigidbody>();
        footmanAnimator = GetComponent<Animator>();
        footmanCollider = GetComponent<CapsuleCollider>();
        swordCollider = transform.GetChild(1).GetChild(2).transform.GetComponent<CapsuleCollider>(); // Sword is a child of a child of the footman
        newPosition = footmanRigidbody.position;

        playerAnimation = player.GetComponent<Animator>();
    }

    void Update()
    {
        // Rotate footman to look at boximon during battle when not dead
        if (!dead)
        {
            Vector3 playerPosition = player.transform.position;
            float dist = Vector3.Distance(transform.position, playerPosition);
            if (dist < attackDistance)
            {
                Vector3 directionToTarget = transform.position - playerPosition;
                float angle = Vector3.Angle(-transform.forward, directionToTarget);
                if(Mathf.Abs(angle) < footmanViewAngle)
                {
                    footmanAnimator.SetBool("Battle", true); // have the footman look like he is ready to attack
                    Vector3 lookTowards = playerPosition;
                    lookTowards.y = transform.position.y;
                    transform.LookAt(lookTowards); // have the footman face the player during battle
                }
            }
            else
            {
                footmanAnimator.SetBool("Battle", false); // if the player is not close enough, the footman will not be ready to attack
            }
        }
    }

    void OnAnimatorMove()
    {
        if(!dead)
        {
            footmanRigidbody.MovePosition(newPosition); // newPosition gets set when the footman is knocked backward
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            footmanAnimator.SetBool("Attack", true); // Swing sword
        }
    }

    void OnCollisionExit(Collision collision)
    {
        footmanAnimator.SetBool("Attack", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Stage Cliff"))
        {
            footmanAnimator.SetTrigger("Die");
            dead = true; // mark footman as dead so certain things stop running 
            /* TODO 
            after player watches footman fall, Unity should stop calculating his position 
            right now he falls forever, let's figure out how to stop that
            basically we need to "uncheck" the footman so he is no longer part of the screen
             */
            footmanCollider.enabled = false; // allow to fall easily
            swordCollider.enabled = false; // make sure sword can't do damage as he falls
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Boximon Bite") && playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack 02")) // Footman in vicinity of boximon and boximon attacking
        {
            footmanAnimator.SetBool("Attack", false); // stop attacking so damage can be taken
            footmanAnimator.SetTrigger("Take Damage"); // play the "knock back" animation

            newPosition = footmanRigidbody.position - transform.forward * movementDistance * Time.deltaTime; // this sets the spot the footman should be knocked back to
        }
    }
}