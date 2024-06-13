using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public Animator animator;
    public AnimationClip rollAnimationClip;
    public float speed = 5f;
    public float jumpForce = 2f;
    public float rollSpeed = 3f;
    private bool isGrounded;
    private bool facingRight = true;
    private bool isRolling = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling)
        { //prevent mocement input during roll
            Move();
        }
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Grounded", isGrounded);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("Grounded", isGrounded);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Ensure the player is grounded if staying on a surface
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("Grounded", isGrounded);
        }
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

}
