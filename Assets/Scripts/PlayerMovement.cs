using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Player player;
    private Rigidbody2D body;
    private Animator animator;
    public GameData gameData;
    private FeetCollision feetCollision;
    private int originaLayer;
    private float originalGravityScale;
    private bool facingRight = true;
    private bool isRolling = false;
    private IInteractable currentInteractable;
    

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        feetCollision = GetComponentInChildren<FeetCollision>();
        originaLayer = gameObject.layer;
        originalGravityScale = body.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        CheckFalling();
        feetCollision.CheckGrounded();

    }

    //Utility Methods--------------------------------------------------------------------------------------
    void HandleInput()
    {
        if (!isRolling)
        { //prevent movement input during roll
            Move();
        }
        if (feetCollision.isGrounded  && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.S) && feetCollision.isOnPlatform)
        {
            StartCoroutine(feetCollision.FallThroughPlatform());
        }

        if (feetCollision.isOnLadder) //feetCollision.isOnLadder checks if body is on ladder too
        {
            ClimbLadder();
        }
        else
        {
            body.gravityScale = originalGravityScale;
        }

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

    void Roll()
    {
        isRolling = true;
        animator.SetTrigger("Roll");
        float rollDirection = facingRight ? 1 : -1;
        body.velocity = new Vector2(rollDirection * player.rollSpeed, body.velocity.y);
        gameObject.layer = LayerMask.NameToLayer("RollingPlayer");//makes it possible to phase through enemeies when rolling
        Debug.Log(gameData.rollAnimationClip.length);
        StartCoroutine(EndRoll(gameData.rollAnimationClip.length));
    }
    IEnumerator EndRoll(float duration)
    {
        yield return new WaitForSeconds(duration); // Adjust duration based on roll animation length
        isRolling = false;
        gameObject.layer = originaLayer;
        // Ensure the player transitions back to the appropriate state
        animator.SetInteger("AnimState", 0);
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