/*
 * References:
 * - Rotate footman to look at boximon: https://www.youtube.com/watch?v=dp3lZUDij6Y 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootmanBlue : MonoBehaviour
{
    public GameObject players;
    private GameObject player;
    private Animator playerAnimation;

    private Rigidbody footmanRigidbody;
    private Animator footmanAnimator;
    private CapsuleCollider footmanCollider;
    private Vector3 newPosition;

    private float movementDistance = 30.0f;
    private float shootDistance = 10f;
    private float attackDistance = 4f; // Stefan - Kiana added this
    private bool dead = false;

    public GameObject _Iceball;
    private bool shooting = false;

    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);

        footmanRigidbody = GetComponent<Rigidbody>();
        footmanAnimator = GetComponent<Animator>();
        footmanCollider = GetComponent<CapsuleCollider>();
        newPosition = transform.position;

        playerAnimation = player.GetComponent<Animator>();

        footmanAnimator.SetBool("Battle", true);
    }

    void Update()
    {
        // Rotate footman to look at boximon during battle when not dead
        if (!dead)
        {
            Vector3 playerPosition = player.transform.position;
            float dist = Vector3.Distance(transform.position, playerPosition);

            // Stefan - Kiana Added this
            if (dist < attackDistance)
            {
                footmanAnimator.SetBool("Shoot", false);
                footmanAnimator.SetBool("Attack", true); // swing sword
            }
            else if (dist < shootDistance)
            {
                footmanAnimator.SetBool("Attack", false);
                footmanAnimator.SetBool("Shoot", true);
                Vector3 lookTowards = playerPosition;
                lookTowards.y = transform.position.y;
                transform.LookAt(lookTowards); // have the footman face the player during battle

                // Have the footman shoot the player
                if (!shooting)
                {
                    /* Stefan - Kiana added this to your script to stop the shooting if the player is dead */
                    if (LevelTracker.levelOver) // Lily changed this to check if level is over instead of player dead
                    {
                        footmanAnimator.SetBool("Shoot", false);
                        return;
                    }
                    else
                    {
                        StartCoroutine(Shoot());
                    }
                }

            }
            else
            {
                footmanAnimator.SetBool("Shoot", false); // if the player is not close enough, the footman will not be ready to attack
                footmanAnimator.SetBool("Attack", false);
            }

        }
        else // if the footman has died, he has fallen off the stage
        {
            Destroy(gameObject, 5); // don't let the footman fall forever, remove him from the scene
        }

        if (LevelTracker.levelOver)
        {
            footmanAnimator.SetBool("Shoot", false); // stop shooting
        }
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            footmanRigidbody.MovePosition(newPosition); // newPosition gets set when the footman is knocked backward
        }
    }

    /*
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("Should swing");
            footmanAnimator.SetBool("Shoot", true); // swing sword
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            footmanAnimator.SetBool("Shoot", false); // stop swinging sword
        }
    }
    */

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stage Cliff"))
        {
            footmanAnimator.SetBool("Shoot", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 5; // decrease falling speed
            dead = true;
            footmanCollider.enabled = false; // allow to fall easily
            LevelTracker.enemiesDefeated++;
        }

        if (other.gameObject.CompareTag("Water"))
        {
            footmanAnimator.SetBool("Shoot", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 10; // decrease drowning speed
            dead = true;
            footmanCollider.enabled = false; // allow to drown
            LevelTracker.enemiesDefeated++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boximon Bite") && playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack 02")) // footman is in the vicinity of boximon and boximon attacking
        {
            footmanAnimator.SetBool("Shoot", false); // stop attacking so damage can be taken
            footmanAnimator.SetTrigger("Take Damage"); // play the "knock back" animation

            newPosition = transform.position - transform.forward * movementDistance * Time.deltaTime; // this sets the spot the footman should be knocked back to
        }
    }

    private IEnumerator Shoot()
    {
        shooting = true;

        // Shoot the iceball, can adapt fire rate with this wait for seconds
        _Iceball = Instantiate(GameObject.Find("Iceball")) as GameObject;
        _Iceball.transform.position = transform.TransformPoint( (Vector3.forward * 1.5f) + (Vector3.up * .65f) );
        _Iceball.transform.rotation = transform.rotation;
        _Iceball.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 3);

        yield return new WaitForSeconds(1f);
        shooting = false;
    }
}
