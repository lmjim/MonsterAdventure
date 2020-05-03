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
    public float m_Speed = 1.0f;
    private float movementDistance = 30.0f;
    //private int health;
    //public Text healthText;
    //private bool beingAttacked;
    private bool dead;

    public GameObject player;
    private Animator playerAnimation;

    Animator m_Animator;
    //GameObject target;
    GameObject boximon_attack;
    GameObject sword;
    Rigidbody m_RigidBody;
    CapsuleCollider m_Collider;
    //Vector3 m_Movement;
    private Vector3 newPosition;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_RigidBody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<CapsuleCollider>();

        //health = 50;
        //SetHealthText();

        //target = GameObject.Find("Boximon Fiery"); 
        playerAnimation = player.GetComponent<Animator>();
        boximon_attack = GameObject.Find("Rig");
        sword = GameObject.Find("Sword");

        newPosition = m_RigidBody.position;

        //beingAttacked = false;
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate footman to look at boximon while not dead
        if (!dead)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
            transform.LookAt(targetPosition);
        }

        /*if (Input.GetKeyDown("f")) // BUG: If you press f at start of game and go to footman will cause instant damage
        {
            beingAttacked = true;
        }*/
    }

    void OnAnimatorMove()
    {
        if(!dead)
        {
            m_RigidBody.MovePosition(newPosition);
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

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Stage Cliff"))
        {
            m_Animator.SetTrigger("Die");
            dead = true;
            m_Collider.enabled = false;
            sword.GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Boximon Bite") && playerAnimation.GetCurrentAnimatorStateInfo(0).IsName("Attack 02") /*TODO beingAttacked*/ ) // Footman in vicinity of boximon and boximon attacking
        {
            m_Animator.SetBool("Attack2", false);
            m_Animator.SetTrigger("Get Hit");
            //m_Animator.SetBool("Get Hit2", true);


            newPosition = m_RigidBody.position - transform.forward * movementDistance * Time.deltaTime;


            //boximon_attack.GetComponent<CapsuleCollider>().isTrigger = false; // Attempt to make trigger happen more than once -- doesn't work
            /*health--;
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
            }*/
        }
        //beingAttacked = false;
        //m_Animator.SetBool("Get Hit2", false);
    }

    void OnTriggerExit(Collider other)
    {
        //beingAttacked = false;
        // boximon_attack.GetComponent<CapsuleCollider>().isTrigger = true;
    }

    /*public void SetHealthText()
    {
        healthText.text = "Enemy Health: " + health.ToString();
    }*/
}