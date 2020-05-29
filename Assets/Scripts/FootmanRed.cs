/*
 * References:
 * - Rotate footman to look at boximon: https://www.youtube.com/watch?v=dp3lZUDij6Y 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FootmanRed : MonoBehaviour
{
    public GameObject players;
    private GameObject player;
    private Animator playerAnimation;

    private Rigidbody footmanRigidbody;
    private Animator footmanAnimator;
    private CapsuleCollider footmanCollider;
    private Vector3 newPosition;

    private float movementDistance = 30.0f;
    private float shootDistance = 8f;
    private float attackDistance = 4f;
    private bool dead = false;

    public GameObject _Fireball;
    private bool shooting = false;

    AudioSource audioSource;
    public AudioClip attack;
    public AudioClip shot;
    private bool keepAttacking = false;

    void Start()
    {
        player = PlayerSwitch.DefinePlayer(players);
        audioSource = GetComponent<AudioSource>();


        footmanRigidbody = GetComponent<Rigidbody>();
        footmanAnimator = GetComponent<Animator>();
        footmanCollider = GetComponent<CapsuleCollider>();
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
                footmanAnimator.SetBool("Shoot", false); // stop shooting and return to battle stance
                Vector3 lookTowards = playerPosition;
                lookTowards.y = transform.position.y;
                transform.LookAt(lookTowards); // have the footman face the player during battle
            }
            else if (dist < shootDistance)
            {
                footmanAnimator.SetBool("Battle", true);
                footmanAnimator.SetBool("Attack", false);
                footmanAnimator.SetBool("Shoot", true);
                Vector3 lookTowards = playerPosition;
                lookTowards.y = transform.position.y;
                transform.LookAt(lookTowards); // have the footman face the player during battle
                
                // Have the footman shoot the player
                if (!shooting)
                {
                    if (LevelTracker.levelOver)
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
                // if the player is not close enough, the footman will not be ready to attack
                footmanAnimator.SetBool("Shoot", false);
                footmanAnimator.SetBool("Attack", false);
                footmanAnimator.SetBool("Battle", false);
            }

        }
        else // if the footman has died, he has fallen off the stage
        {
            Destroy(gameObject, 5); // don't let the footman fall forever, remove him from the scene
        }

        if (LevelTracker.levelOver)
        {
            footmanAnimator.SetBool("Shoot", false); // stop shooting
            footmanAnimator.SetBool("Attack", false);
            keepAttacking = false;
        }
    }

    void FixedUpdate()
    {
        if (!dead)
        {
            footmanRigidbody.MovePosition(newPosition); // newPosition gets set when the footman is knocked backward
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead)
        {
            footmanAnimator.SetBool("Attack", true); // swing sword
            keepAttacking = true;
            //StartCoroutine(PlayAttackSound());
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dead)
        {
            footmanAnimator.SetBool("Attack", false); // stop swinging sword
            keepAttacking = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stage Cliff") && !dead)
        {
            footmanAnimator.SetBool("Shoot", false);
            footmanAnimator.SetBool("Attack", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 5; // decrease falling speed
            dead = true;
            keepAttacking = false;
            footmanCollider.enabled = false; // allow to fall easily
            LevelTracker.enemiesDefeated++;
        }

        if ((other.gameObject.CompareTag("Water") || other.gameObject.CompareTag("Lava")) && !dead)
        {
            footmanAnimator.SetBool("Shoot", false);
            footmanAnimator.SetBool("Attack", false);
            footmanAnimator.SetTrigger("Die");
            footmanRigidbody.drag = 10; // decrease drowning speed
            dead = true;
            keepAttacking = false;
            footmanCollider.enabled = false; // allow to drown
            LevelTracker.enemiesDefeated++;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Boximon Bite") && playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack 02") && !dead) // footman is in the vicinity of boximon and boximon attacking
        {
            footmanAnimator.SetBool("Attack", false); // stop attacking so damage can be taken
            footmanAnimator.SetTrigger("Take Damage"); // play the "knock back" animation

            newPosition = transform.position - transform.forward * movementDistance * Time.deltaTime; // this sets the spot the footman should be knocked back to
        }
    }

    private IEnumerator Shoot()
    {
        shooting = true;

        // Shoot the iceball, can adapt fire rate with this wait for seconds
        _Fireball = Instantiate(GameObject.Find("Fireball")) as GameObject;
        _Fireball.transform.position = transform.TransformPoint((Vector3.forward * 1.5f) + (Vector3.up * .65f));
        _Fireball.transform.rotation = transform.rotation;
        _Fireball.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 2);
        audioSource.PlayOneShot(shot); // Shooting sound fx
        yield return new WaitForSeconds(1f);
        shooting = false;
    }

    IEnumerator PlayAttackSound() // Coroutine to play attacking sound fx every second
    {
        while (keepAttacking)
        {
            audioSource.volume = 0.7f;
            audioSource.PlayOneShot(attack);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
