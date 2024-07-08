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
    private int originalLayer;
    private float originalGravityScale;
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

    void Start()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponents<BoxCollider2D>()[1]; // second box collider on player
        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
        originalLayer = gameObject.layer;
        originalGravityScale = body.gravityScale;
    }

    void Update()
    {
        HandleInput();
        CheckGrounded();
        CheckFalling();
    }

    void HandleInput()
    {
        if (!isRolling)
        {
            Move();
        }

        if ((isGrounded || isOnLadder) && Input.GetButtonDown("Jump"))
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

        if (isOnLadder)
        {
            ClimbLadder();
        }

        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact();
        }
    }

    void CheckGrounded()
    {
        isOnPlatform = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        isOnGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isOnStairs = feetCollider.IsTouchingLayers(LayerMask.GetMask("Stairs"));
        isOnLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));

        if (isOnGround || isOnPlatform || isOnStairs)
        {
            isGrounded = true;
            if (frictionMaterial != null)
            {
                frictionMaterial.friction = isOnStairs ? 3 : 0; // increase friction when on stairs to stop sliding down
            }
        }
        else
        {
            isGrounded = false;
        }

        if (!isOnLadder)
        {
            body.gravityScale = originalGravityScale; // Reset gravity if not on ladder
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
                if (interactText != null)
                {
                    interactText.gameObject.SetActive(true);
                    interactText.transform.position = other.transform.position + new Vector3(0, 1.5f, 0);
                }
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
                if (interactText != null)
                {
                    interactText.gameObject.SetActive(false);
                }
            }
        }
    }

    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);

        if (Mathf.Abs(moveInput) > 0.01f)
        {
            animator.SetInteger("AnimState", 1);
        }
        else
        {
            animator.SetInteger("AnimState", 0);
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
        if (isOnLadder)
        {
            isOnLadder = false; // Exit ladder state when jumping
            body.gravityScale = originalGravityScale; // Reset gravity
        }
        body.velocity = new Vector2(body.velocity.x, jumpForce);
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
        animator.SetFloat("AirSpeedY", body.velocity.y);
    }

    void Roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        float rollDirection = facingRight ? 1 : -1;
        body.velocity = new Vector2(rollDirection * rollSpeed, body.velocity.y);
        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");
        StartCoroutine(EndRoll(rollAnimationClip.length));
    }

    IEnumerator EndRoll(float duration)
    {
        yield return new WaitForSeconds(duration);
        isRolling = false;
        gameObject.layer = originalLayer;
        animator.SetInteger("AnimState", 0);
    }

    IEnumerator FallThroughPlatform()
    {
        int platformLayer = LayerMask.NameToLayer("Platform");
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
        shieldCollider.enabled = false;

        yield return new WaitForSeconds(fallThroughDuration);

        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
        shieldCollider.enabled = true;
    }

    void ClimbLadder()
    {
        body.gravityScale = 0;
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            body.velocity = new Vector2(body.velocity.x, verticalInput * climbSpeed);
        }
        else
        {
            body.velocity = new Vector2(body.velocity.x, 0);
        }

        // Allow jumping off the ladder
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void ResetGravity()
    {
        if (!isOnLadder)
        {
            body.gravityScale = originalGravityScale;
        }
    }
}