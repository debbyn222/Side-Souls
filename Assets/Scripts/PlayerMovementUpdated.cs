/* Purpose:
Controls player movement and interactions in the game
*/
//Last edited: 24th of June, 2024 @11:17pm PST

using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovementUpdated : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public AnimationClip rollAnimationClip;
    private BoxCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    public BoxCollider2D shieldCollider;
    public PhysicsMaterial2D frictionMaterial;
    public TextMeshProUGUI interactText;

    private int originalLayer; //stores original layer of character
    private float originalGravityScale; //stores original grav scale
    public float speed = 7f;
    public float jumpForce = 5f;
    public float rollSpeed = 4f;
    public float climbSpeed = 3f;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isRolling = false;
    public float fallThroughDuration = 0.5f;
    private bool isOnPlatform;
    private bool isOnGround;
    private bool isOnStairs;
    private bool isOnLadder;
    private IInteractable currentInteractable;

    public AudioSource walkSound; //Reference to the AudioSource component for walking
    public AudioSource rollingSound;
    private bool isWalking = false;

    void Start()
    {
        //initialize references to components and original settings
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponents<BoxCollider2D>()[1]; // second box collider on player
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
        //setting original layer and grav
        originalLayer = gameObject.layer;
        originalGravityScale = body.gravityScale; 
    }

    void Update()
    {
        //Handles player input and checks for ground or platform contacts
        HandleInput();
        CheckGrounded();
        CheckFalling();
        checkIfWalking();
    }

    void HandleInput()
    {
        if (!isRolling) //ONLY allow movement if player is not rolling
        {
            Move(); //(most likely will be updated to include more restrictions)
        }

        if (isGrounded && Input.GetButtonDown("Jump"))//only activate jump if player is grounded
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))//roll when either shift key pressed
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.S) && isOnPlatform)//drop platforms if on platform and key pressed
        {
            StartCoroutine(FallThroughPlatform());//should ideally add drop with down arrow as well
        }

        if (isOnLadder)//no requirement to climb ladder. just climb when interacting
        {
            ClimbLadder();
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null) //if obj is interactable, press E to interact
        {
            currentInteractable.Interact();
        }
    }

    //check if player is on ground, platform, stairs, or ladder
    void CheckGrounded()
    {
        isOnPlatform = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        isOnGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isOnStairs = feetCollider.IsTouchingLayers(LayerMask.GetMask("Stairs"));
        isOnLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));

        if (isOnGround || isOnPlatform || isOnStairs)
        {
            isGrounded = true;
            if (frictionMaterial != null) //adjust friction is friction material is provided
            {
                frictionMaterial.friction = isOnStairs ? 3 : 0; // increase friction when on stairs to stop sliding down
            }
        }
        else
        {
            isGrounded = false;
        }

        animator.SetBool("Grounded", isGrounded); //update animator with grounded state
    }

    //Handle trigger collisions with interactable objects
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                if (interactText != null)
                {
                    interactText.gameObject.SetActive(true);
                    interactText.transform.position = other.transform.position + new Vector3(0, 1.5f, 0);
                }
            }
        }
    }

    //Handle exit trigger collisions with interactable objects
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && currentInteractable == interactable)
            {
                currentInteractable = null;
                if (interactText != null)
                {
                    interactText.gameObject.SetActive(false);
                }
            }
        }
    }

    //Handle player movement
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); //gets horizontal input (A/D, Left/Right arrow)
        body.velocity = new Vector2(moveInput * speed, body.velocity.y); //player velocity update

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            isWalking = true;
            animator.SetInteger("AnimState", 1);//update animator state to walking
        }
        else
        {
            isWalking = false;
            animator.SetInteger("AnimState", 0);//update animator state to idle
        }

        //player faces the way that it is recieving user input (faces right when going right & vice versa)
        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void checkIfWalking()
    {
        if (!isWalking)
        {
            walkSound.Play(); // Play walking sound
        }
    }

    //trigger jump force & animation
    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    //Flipping player's direction 
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    //update animator with player's vertical speed
    void CheckFalling()
    {
        animator.SetFloat("AirSpeedY", body.velocity.y);
    }

    //Handle player roll action
    void Roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        float rollDirection = facingRight ? 1 : -1;
        body.velocity = new Vector2(rollDirection * rollSpeed, body.velocity.y);
        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");
        StartCoroutine(EndRoll(rollAnimationClip.length));//starts coroutine to end
        rollingSound.Play();
    }

    //coroutine to end roll after set duration
    IEnumerator EndRoll(float duration)
    {
        yield return new WaitForSeconds(duration);
        isRolling = false;
        gameObject.layer = originalLayer;
        animator.SetInteger("AnimState", 0);
    }

    //coroutine to fall through platforms (can be tweaked)
    IEnumerator FallThroughPlatform()
    {
        int platformLayer = LayerMask.NameToLayer("Platform");
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
        shieldCollider.enabled = false;

        yield return new WaitForSeconds(fallThroughDuration); //ideally not time based but rather checks if in a floor or not

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
        shieldCollider.enabled = true;
    }

    void ClimbLadder()
    {
        body.gravityScale = 0; //disables gravity while climbing
        float verticalInput = Input.GetAxis("Vertical"); //gets vertical input (W/S, Up/Down arrow keys)

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            body.velocity = new Vector2(body.velocity.x, verticalInput * climbSpeed); //if vertical input, apply vertical velocity 
        }
        else
        {
            body.velocity = new Vector2(body.velocity.x, 0);
        }
    }

    //resets gravity to original value when not on ladder
    void ResetGravity()
    {
        if (!isOnLadder)
        {
            body.gravityScale = originalGravityScale;
        }
    }
}
/* Notes:
 * Huge change to original PlayerMovement script
 * Calls methods into Update rather than having them all in Update.
        - Honestly would like to do this will all other scripts (makes it a lot easier to read what's happening.

//else
        {
            walkSound.Stop(); // Stop walking sound
        }
 */