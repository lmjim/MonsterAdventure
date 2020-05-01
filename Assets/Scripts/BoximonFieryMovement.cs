/*
 * References:
 * Jump: https://answers.unity.com/questions/1020197/can-someone-help-me-make-a-simple-jump-script.html
 * iFrame: https://aleksandrhovhannisyan.github.io/blog/dev/invulnerability-frames-in-unity/
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoximonFieryMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public float movementSpeed = 5f;
    private float jumpForce = 4.0f;
    public bool isGrounded;

    private int health;
    public Text healthText;
    public Text loseText;

    private bool isInvincble = false;
    //[SerializeField]
    private float invincibilityDurationSeconds = 0.5f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;


    //private int hitGround = 0; // Remove, this is for debugging

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();

        health = 5;
        SetHealthText();

        loseText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            m_Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown("f"))
        {
            m_Animator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to attack state
            //m_Animator.SetTrigger("Attack 01"); // Punch
            m_Animator.SetTrigger("Attack 02"); // Shove
        }
    }

    void FixedUpdate()
    {
            // Walking Functionaltiy
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            m_Movement.Set(horizontal, 0f, vertical);
            m_Movement.Normalize();
            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool walkForward = hasHorizontalInput || hasVerticalInput;
            m_Animator.SetBool("Walk Forward", walkForward);
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);

        // Lose Text if fall off stage
        if (transform.position.y < -1) // This threshold might change for other levels
        {
            loseText.text = "You died! Game over.";
        }
    }

    void OnAnimatorMove()
    {
        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * movementSpeed);
        m_Rigidbody.MoveRotation(m_Rotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        if(collision.gameObject.CompareTag("Spikes")) // Player runs into spikes
        {
            m_Animator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to die state
            m_Animator.SetTrigger("Die");
            loseText.text = "You died! Game over.";
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            //print("Hit Ground " + hitGround++); // Remove this, this is for debugging
        }

        if(collision.gameObject.CompareTag("Water"))
        {
            m_Animator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to die state
            // The arrow keys trigger the animation, so the player needs to be stopped
            // TODO Before triggering death, forcefully stop the player from being able to move
            m_Animator.SetTrigger("Die");
            loseText.text = "You died! Game over.";
        }
    }

    void OnCollisionExit()
    {
        //isGrounded = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Slime")) // Player goes near a slime
        {
            Slime theSlime = other.gameObject.GetComponent<Slime>();
            theSlime.ShowMessage();
        }

        if (other.gameObject.CompareTag("Damage")) // Player is hit by the sword
        {
            if (isInvincble) return;

            m_Animator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to damage state
            m_Animator.SetTrigger("Take Damage"); // very short animation, could also transform position backwards a little bit
            health--;
            SetHealthText();

            // If no more health left, die
            if (health < 1)
            {
                m_Animator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to die state
                m_Animator.SetTrigger("Die");
                loseText.text = "You died! Game over.";

                // Getting rid of negative numbers
                health = 0;
                SetHealthText();
            }

            // iFrame to maintain lives
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Slime")) // Player leaves a slime
        {
            Slime theSlime = other.gameObject.GetComponent<Slime>();
            theSlime.RemoveMessage();
        }
    }

    void SetHealthText()
    {
        healthText.text = "Boximon Health: " + health.ToString();
    }

    // Makes boximon invinsible for a little so it doesn't lose hella health
    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincble = true;
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        isInvincble = false; // No longer invinsible
    }
}
