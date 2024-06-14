using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public AnimationClip rollAnimationClip;
    private BoxCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    public BoxCollider2D shieldCollider;
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rollSpeed = 4f;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isRolling = false;
    private int originaLayer;
    private float fallThroughDuration = 0.5f;
    private bool isOnPlatform;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponents<BoxCollider2D>()[1]; //second box collider on player
        originaLayer = gameObject.layer;
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
    }


    //movement methods----------------------------------------------------------------------------------------
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(moveInput * speed, body.velocity.y);
        //check if moving
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            animator.SetInteger("AnimState", 1);
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
        body.velocity = new Vector2(rollDirection * rollSpeed, body.velocity.y);
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

    void CheckGrounded()
    {
        isOnPlatform = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        if (feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) || isOnPlatform) 
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        animator.SetBool("Grounded", isGrounded);
        //Debug.Log("Grounded: " + isGrounded);
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
        shieldCollider.enabled = true;
    }

}