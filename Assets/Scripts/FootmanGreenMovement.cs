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
    private int health;
    public Text healthText;
    private bool beingAttacked;
    private bool dead;

    Animator m_Animator;
    GameObject target;
    GameObject boximon_attack;
    GameObject sword;
    //Rigidbody m_RigidBody;
    //Vector3 m_Movement;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        //m_RigidBody = GetComponent<Rigidbody>();

        health = 2;
        SetHealthText();

        target = GameObject.Find("Boximon Fiery"); 
        boximon_attack = GameObject.Find("Rig");
        sword = GameObject.Find("Sword");

        beingAttacked = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate footman to look at boximon while not dead
        if (!dead)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            transform.LookAt(targetPosition);
        }

        if (Input.GetKeyDown("f")) // BUG: If you press f at start of game and go to footman will cause instant damage
        {
            beingAttacked = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //m_Animator.SetTrigger("Attack"); // Continuously swings sword
            m_Animator.SetBool("Attack2", true); // Swings once
        }
    }

    void OnCollisionExit(Collision collision)
    {
        m_Animator.SetBool("Attack2", false);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Boximon Fist") && beingAttacked ) // Footman in vicinity of boximon and boximon attacking
        {
            m_Animator.SetTrigger("Get Hit");
            //m_Animator.SetBool("Get Hit2", true);


            //boximon_attack.GetComponent<CapsuleCollider>().isTrigger = false; // Attempt to make trigger happen more than once -- doesn't work
            health--;
            SetHealthText();
            
            if (health < 1)
            {
                m_Animator.SetTrigger("Die");
                health = 0;
                SetHealthText();

                dead = true;

                // Disable colldiers after footman is dead
                sword.GetComponent<CapsuleCollider>().enabled = false; // Don't want sword to cause damage after footman is dead
                boximon_attack.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
        beingAttacked = false;
        m_Animator.SetBool("Get Hit2", false);
    }

    void OnTriggerExit(Collider other)
    {
        beingAttacked = false;
        // boximon_attack.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    public void SetHealthText()
    {
        healthText.text = "Enemy Health: " + health.ToString();
    }
}