/*
 * References:
 * Jump: https://answers.unity.com/questions/1020197/can-someone-help-me-make-a-simple-jump-script.html
 * iFrame: https://aleksandrhovhannisyan.github.io/blog/dev/invulnerability-frames-in-unity/
 * Double Jump: https://www.youtube.com/watch?v=OqTwWIoR75Y
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{   
    public Text healthText;
    public Text loseText;
    public int health = 20; // leave this public, it could change based on the level
    public float movementSpeed = 7f; // leave this public, it could change based on the level

    private Animator playerAnimator;
    private Rigidbody playerRB;
    private Vector3 playerMovement;
    private Quaternion playerRotation = Quaternion.identity;

    private GameObject[] slimes;
    private float turnSpeed = 20f;
    private float jumpForce = 4.0f;
    private float wallJumpForce = 2.0f;
    private bool isGrounded = true;
    private bool isInvincible = false;
    private float invincibilityDurationSeconds = 0.5f;
    private float wallJumpTime = 0.5f;
    private int maxJumps = 2;
    private int jumps = 0;
    private bool onWall = false;
    private bool isSprinting = false;
    private bool isWallJumping = false;
    private Vector3 moveDirection = Vector3.zero;

    public bool canSprint = false;
    public bool canDoubleJump = false;
    public bool canWallJump = false;
   
    
    

    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();

        slimes = GameObject.FindGameObjectsWithTag("Slime");

        SetHealthText();

        loseText.text = "";

        // check global tracker for previously learned abilities
        if (LevelTracker.learnedSprint)
        {
            canSprint = true;
        }
        if (LevelTracker.learnedDoubleJump)
        {
            canDoubleJump = true;
        }
        if (LevelTracker.learnedWallJump)
        {
            canWallJump = true; 
        }
    }

    void Update()
    {
        if(!LevelTracker.levelOver)
        {
            if (Input.GetKeyDown("space"))
            {
                if (canWallJump && onWall && !isWallJumping && !isGrounded)
                // we don't care about the # of jumps, player should always be able to wall jump (when unlocked)
                // when the player is on the ground (against a pillar/wall), they shouldn't kick off the wall
                // only kick off of the wall when already in the air (i.e. not grounded)
                {
                    Vector3 desiredForward = Vector3.RotateTowards(-transform.forward /*reverse direction*/, playerMovement, turnSpeed * Time.deltaTime, 0f);
                    playerRotation = Quaternion.LookRotation(desiredForward);

                    playerRB.AddForce(-transform.forward * wallJumpForce, ForceMode.Impulse); // push player in opposite direction
                    StartCoroutine(setIsWallJumping()); // momentarily stop arrow keys from moving player, so the walll kick can be seen/take effect

                    playerJump(); // this will increment jumps, but that's okay because you shouldn't "double jump" after a wall kick
                }
                else
                {
                    if (canDoubleJump && (isGrounded || maxJumps > jumps))
                    {
                        playerJump(); // this sets y-velocity to zero, so second jump will get a full jump force
                    }
                    else
                    {
                        // either player is out of jumps or double jump is not unlocked
                        // if grounded, then player can still jump once
                        if (isGrounded)
                        {
                            playerJump();
                        }
                    }
                }
            }

            if (Input.GetKeyDown("f"))
            {
                playerAnimator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to attack state
                playerAnimator.SetBool("Run Forward", false);
                playerAnimator.SetTrigger("Attack 02"); // Bite
            }

            if (Input.GetKeyDown("left shift")) // toggle sprint
            {
                if (canSprint)
                {
                    isSprinting = !isSprinting;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if(!LevelTracker.levelOver && !isWallJumping)
        {
            // Walking Functionality
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            playerMovement.Set(horizontal, 0f, vertical);
            playerMovement.Normalize();
            bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
            bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
            bool hasInput = hasHorizontalInput || hasVerticalInput;

            if (isSprinting)
            {
                playerAnimator.SetBool("Walk Forward", false);
                playerAnimator.SetBool("Run Forward", true && hasInput);
            }
            else
            {
                playerAnimator.SetBool("Run Forward", false);
                playerAnimator.SetBool("Walk Forward", hasInput);
            }
            
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, playerMovement, turnSpeed * Time.deltaTime, 0f);
            playerRotation = Quaternion.LookRotation(desiredForward);
        }

        if (transform.position.y < -1) // player fell off stage
        {
            Die();
        }
    }

    void OnAnimatorMove()
    {
        if(!onWall && !isWallJumping) // this allows the player to have upward momentum when against a wall
        {
            // This is really hacky but something about our movement sort of combining with animations made running way too fast, this helps that for now at least
            // We could adjust the speed with this in the future if we can fix the bug where when sprinting you can end your sprint to get a sudden huge burst before the animation changes
            if (isSprinting)
            {
                playerAnimator.SetBool("Walk Forward", false);
                playerRB.MovePosition(playerRB.position + playerMovement * playerAnimator.deltaPosition.magnitude * (movementSpeed * .25f));
            }
            else
            {
                playerAnimator.SetBool("Run Forward", false);
                playerRB.MovePosition(playerRB.position + playerMovement * playerAnimator.deltaPosition.magnitude * movementSpeed);
            }
        }

        playerRB.MoveRotation(playerRotation);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Spikes") || collision.gameObject.CompareTag("Water") || collision.gameObject.CompareTag("Lava")) // player runs into spikes or falls into water
        {
            Die();
        }

        if (collision.gameObject.CompareTag("Ground")) // player is not jumping
        {
 
            isGrounded = true;
            jumps = 0;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage")) // Player is hit by the sword
        {
            if (isInvincible) return;

            playerAnimator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to damage state
            playerAnimator.SetBool("Run Forward", false);
            playerAnimator.SetTrigger("Take Damage"); // very short animation, could also transform position backwards a little bit
            health--;
            SetHealthText();

            // If no more health left, die
            if (health < 1)
            {
                Die();

                // Getting rid of negative numbers
                health = 0;
                SetHealthText();
            }

            // iFrame to maintain lives
            StartCoroutine(BecomeTemporarilyInvincible());
        }

        if (other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true; // the buttons and portal should count as ground too
            jumps = 0;
        }

        if (other.gameObject.CompareTag("Iceball") || (other.gameObject.CompareTag("Fireball")))
        {
            if (isInvincible) return;

            playerAnimator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to damage state
            playerAnimator.SetBool("Run Forward", false);
            playerAnimator.SetTrigger("Take Damage"); // very short animation, could also transform position backwards a little bit
            health -= 2; // Iceballs do more damage that the sword
            SetHealthText();

            // If no more health left, die
            if (health < 1)
            {
                Die();

                // Getting rid of negative numbers
                health = 0;
                SetHealthText();
            }

            // iFrame to maintain lives
            StartCoroutine(BecomeTemporarilyInvincible());

            // TODO: Slow the player, if we decide to do that!

            // TODO: Add an icy particle the player when hit!
        }

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            onWall = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            onWall = false;
        }
        
    }

    // Makes boximon invinsible for a little so it doesn't lose hella health
    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDurationSeconds);
        isInvincible = false; // No longer invinsible
    }

    private IEnumerator setIsWallJumping()
    {
        // immediately after kicking off of a wall, we want to pause certain actions
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        isWallJumping = false;
    }

    void SetHealthText()
    {
        healthText.text = "Boximon Health: " + health.ToString();
    }

    void playerJump()
    {
        Vector3 v = playerRB.velocity;
        v.y = 0.0f;
        playerRB.velocity = v;
        playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
        jumps++;
    }

    public void FinishLevel() // this is called by the portal
    {
        playerAnimator.SetBool("Walk Forward", false); // stop the player from moving
        playerAnimator.SetBool("Run Forward", false);

        foreach (GameObject slime in slimes)
        {
            slime.GetComponent<Slime>().FinishLevel();
        }

        LevelTracker.EndLevel(true);
    }

    void Die()
    {
        playerAnimator.SetBool("Walk Forward", false); // switch back to idle state to be able to go to die state
        playerAnimator.SetBool("Run Forward", false);

        playerAnimator.SetTrigger("Die");
        loseText.text = "You died! Game over.\nPress BACKSPACE replay level\nPress TAB to return home";

        foreach (GameObject slime in slimes)
        {
            slime.GetComponent<Slime>().PlayerDied();
        }

        LevelTracker.EndLevel(false);
    }
}
