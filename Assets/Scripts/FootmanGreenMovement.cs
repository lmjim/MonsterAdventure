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
    //private float footmanViewAngle = 80.0f;
    private bool dead = false;

    void Start()
    {
        footmanRigidbody = GetComponent<Rigidbody>();
        footmanAnimator = GetComponent<Animator>();
        footmanCollider = GetComponent<CapsuleCollider>();
        swordCollider = transform.GetChild(1).GetChild(2).transform.GetComponent<CapsuleCollider>(); // sword is a child of a child of the footman
        newPosition = transform.position;

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
                //Vector3 directionToTarget = transform.position - playerPosition;
                //float angle = Vector3.Angle(-transform.forward, directionToTarget);
                //if(Mathf.Abs(angle) < footmanViewAngle) // this lets you perform sneak attacks, but when the footman is hit from behind, his position glitches
                //{
                    footmanAnimator.SetBool("Battle", true); // have the footman look like he is ready to attack
                    Vector3 lookTowards = playerPosition;
                    lookTowards.y = transform.position.y;
                    transform.LookAt(lookTowards); // have the footman face the player during battle
                //}
            }
            else
            {
                footmanAnimator.SetBool("Battle", false); // if the player is not close enough, the footman will not be ready to attack
            }

            /*
            THIS MAKES THE FOOTMAN MUCH HARDER TO DEFEAT
            maybe use this on a higher level footman
            if (dist < 2)
            {
                footmanAnimator.SetBool("Attack", true); // swing sword
            }
            else
            {
                footmanAnimator.SetBool("Attack", false); // don't swing sword
            }*/
        }
        else // if the footman has died, he has fallen off the stage
        {
            Destroy(gameObject, 5); // don't let the footman fall forever, remove him from the scene
        }

        if (BoximonFieryMovement.levelOver)
        {
            footmanAnimator.SetBool("Attack", false); // stop swinging sword
        }
    }

    void FixedUpdate()
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
            footmanAnimator.SetBool("Attack", true); // swing sword
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            footmanAnimator.SetBool("Attack", false); // stop swinging sword
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Stage Cliff"))
        {
            footmanAnimator.SetBool("Attack", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 5; // decrease falling speed
            dead = true;
            footmanCollider.enabled = false; // allow to fall easily
            swordCollider.enabled = false; // make sure sword can't do damage as he falls
        }

        if(other.gameObject.CompareTag("Water"))
        {
            footmanAnimator.SetBool("Attack", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 10; // decrease drowning speed
            dead = true;
            footmanCollider.enabled = false; // allow to drown
            swordCollider.enabled = false; // make sure sword can't do damage as he dies
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Boximon Bite") && playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack 02")) // footman is in the vicinity of boximon and boximon attacking
        {
            footmanAnimator.SetBool("Attack", false); // stop attacking so damage can be taken
            footmanAnimator.SetTrigger("Take Damage"); // play the "knock back" animation

            newPosition = transform.position - transform.forward * movementDistance * Time.deltaTime; // this sets the spot the footman should be knocked back to
        }
    }
}