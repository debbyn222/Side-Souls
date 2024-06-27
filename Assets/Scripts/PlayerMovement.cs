using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    public Rigidbody2D body;
    public Animator animator;
    public AnimationClip rollAnimationClip;
    private BoxCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    public BoxCollider2D shieldCollider;
    public PhysicsMaterial2D frictionMaterial;
    public TextMeshProUGUI interactText;
    private int originaLayer;
    private float originalGravityScale;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isRolling = false;
    private float fallThroughDuration = 0.3f;
    private bool isOnPlatform;
    private bool isOnGround;
    private bool isOnStairs;
    private bool isOnLadder;
    private IInteractable currentInteractable;
    

    // Start is called before the first frame update
    void Start()
    {
        //player = new Player(); //sets default player properties
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponents<BoxCollider2D>()[1]; //second box collider on player
        interactText.gameObject.SetActive(false);
        originaLayer = gameObject.layer;
        originalGravityScale = body.gravityScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling)
        { //prevent movement input during roll
            Move();
        }
        CheckGrounded();
        CheckFalling();
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.S) && isOnPlatform)
        {
            StartCoroutine(FallThroughPlatform());
        }
        if(isOnLadder)
        {
            ClimbLadder();
        }
        else
        {
            body.gravityScale = originalGravityScale;
        }

        if(Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(); //calls interact method in IInteractable interface that is implemented on every interactable object
        }
    }

    //Utility Methods--------------------------------------------------------------------------------------
    void CheckGrounded()
    {
        isOnPlatform = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        isOnGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isOnStairs = feetCollider.IsTouchingLayers(LayerMask.GetMask("Stairs"));
        isOnLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (isOnGround || isOnPlatform || isOnStairs)
        {
            isGrounded = true;
            frictionMaterial.friction = isOnStairs ? 3 : 0; //increase friction when on stairs to stop sliding down
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("Grounded", isGrounded);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                interactText.gameObject.SetActive(true);

                
                SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                float spriteHeight = spriteRenderer.bounds.size.y;
                Vector3 spriteTopPosition = spriteRenderer.bounds.center + new Vector3(0, spriteHeight / 2, 0);//spriteheight/2 + center = top of sprite
                interactText.transform.position = spriteTopPosition + new Vector3(0, 0.3f, 0); //position text above object sprite
                interactText.SetText("[E] " + interactable.InteractDescription);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && currentInteractable == interactable)
            {
                currentInteractable = null;
                interactText.gameObject.SetActive(false);
            }
        }
    }


    //movement methods----------------------------------------------------------------------------------------
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * player.speed, body.velocity.y);


        //check if moving
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            animator.SetInteger("AnimState", 1);
            animator.speed = player.speed/7f; //7 is default
            // Debug.Log("AnimState " + animator.GetInteger("AnimState"));
        }
        else
        {
            animator.SetInteger("AnimState", 0);
            // Debug.Log("AnimState " + animator.GetInteger("AnimState"));

        }

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        }
        else if (moveInput < 0 && facingRight)
        {
            Flip();
        }
    }

    void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, player.jumpForce);
        animator.SetTrigger("Jump");
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
    void CheckFalling()
    {
        if (body.velocity.y < 0)
        {
            animator.SetFloat("AirSpeedY", body.velocity.y);
        }
        else
        {
            animator.SetFloat("AirSpeedY", body.velocity.y);
        }
    }

    void Roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        float rollDirection = facingRight ? 1 : -1;
        body.velocity = new Vector2(rollDirection * player.rollSpeed, body.velocity.y);
        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");//makes it possible to phase through enemeies when rolling
        StartCoroutine(EndRoll(rollAnimationClip.length));
    }
    IEnumerator EndRoll(float duration)
    {
        yield return new WaitForSeconds(duration); // Adjust duration based on roll animation length
        isRolling = false;
        gameObject.layer = originaLayer;
        // Ensure the player transitions back to the appropriate state
        animator.SetInteger("AnimState", 0);
    }


    IEnumerator FallThroughPlatform()
    {
        int platformLayer = LayerMask.NameToLayer("Platform");

        // Disable collisions with platforms temporarily for all relevant colliders
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
        shieldCollider.enabled = false;

        yield return new WaitForSeconds(fallThroughDuration);

        // Re-enable collisions with platforms
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
        //shieldCollider.enabled = true;
    }

    void ClimbLadder()
    {
        body.gravityScale = 0;
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("Vertical") != 0)
        {
            body.velocity = new Vector2(body.velocity.x, player.climbSpeed);
            //insert code to run climbing animation
        }
        else if (Input.GetKey(KeyCode.S))
        {
            body.velocity = new Vector2(body.velocity.x, -player.climbSpeed);
            //insert code to run climbing animation
        }
        else//stopped climing but still on ladder
        {
            body.velocity = new Vector2(body.velocity.x, 0); // Stop moving when no key is pressed
            //insert code to end climbing animation
        }
    }

}