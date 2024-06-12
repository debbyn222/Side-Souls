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
    public float speed = 5f;
    public float jumpForce = 5f;
    public float rollSpeed = 3f;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isRolling = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<BoxCollider2D>();
        feetCollider = GetComponents<BoxCollider2D>()[1]; //second box collider on player

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Facing Right: " + facingRight);
        if (!isRolling)
        { //prevent mocement input during roll
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
    }

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
        StartCoroutine(EndRoll(rollAnimationClip.length));
    }
    IEnumerator EndRoll(float duration)
    {
        yield return new WaitForSeconds(duration); // Adjust duration based on roll animation length
        isRolling = false;
        // Ensure the player transitions back to the appropriate state
        animator.SetInteger("AnimState", 0);
    }

    void CheckGrounded()
    {
        isGrounded = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        animator.SetBool("Grounded", isGrounded);
        Debug.Log("Grounded: " + isGrounded);
    }

}