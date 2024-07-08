/*Purpose:
Control behavior of enemy AI
Includes: Movement, attacking, and detecting player
*/
//Last Edited: 25th of June, 2024 @2:24am PST
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public float attackDistance; //minimum distance to attack
    public float speed;
    public float jumpForce;
    public float timer; //Timer for cooldown between attacks

    private RaycastHit2D hit;
    private GameObject target;
    private Vector2 targetPosition;
    private Animator animator;
    private float distance; //distance between enemy and player
    private bool attackMode;
    private bool inRange; //chcek if player is in range
    private bool attackCooldown; //Check is enemy is still cooling down
    private float intTimer;

    private bool isFacingRight = false;
    //private Sensor_Bandit sensor;
    private Rigidbody2D rb;

    void Awake()
    {
        intTimer = timer; //store intial value of timer
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if player is in range and update raycast direction based on facing direction
        if (inRange && isFacingRight)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.right, rayCastLength, rayCastMask);
            RaycastDebugger();
        } 
        else if (inRange && !isFacingRight)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
            RaycastDebugger();
        }

        //If the player is detected
        if (hit.collider != null)
        {
            EnemyLogic();
        } 
        else if (hit.collider == null)
        {
            inRange = false;
        }

        //if player is out of range, stop attacking
        if(inRange == false)
        {
            animator.SetBool("canWalk", false);
            StopAttack(); 
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with trigger area then player is in range
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            inRange = true;
        }
        //Check the position of the player relative to the enemy, if player is left and enemy is facing right flip and vice versa
        if (targetPosition.x < transform.position.x && isFacingRight || targetPosition.x > transform.position.x && !isFacingRight)
        {
            Flip();
            isFacingRight = !isFacingRight;
        }
    }

    void EnemyLogic()
    {
        //Calculate distance between enemy player, and decide whether to move or attack
        distance = Vector2.Distance(transform.position, target.transform.position);
        if (distance > attackDistance)
        {
            Move();
            StopAttack();
        }
        else if (attackDistance >= distance && attackCooldown == false) 
        {
            Attack();
        }
        if (attackCooldown)
        {
            Cooldown();
            animator.SetBool("Attack", false);
        }
    }

    void Move()
    {
        //Move towards plauer if not attacking
        animator.SetBool("canWalk", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            targetPosition = new Vector2(target.transform.position.x, target.transform.position.y);
            //transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            float directionOfTravel;
            if (target.transform.position.x > transform.position.x)
            {
                directionOfTravel = 1;
            } else
            {
                directionOfTravel = -1;
            }
            /*if (targetPosition.y > transform.position.y + 2)
            {
                Jump();
            }*/
            rb.velocity = new Vector2(speed*directionOfTravel, rb.velocity.y);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        animator.SetTrigger("Jump");
    }

    void Attack ()
    {
        timer = intTimer; //Reset timer when player enter attack Range
        attackMode = true; //To check if Enemy can still attack or not

        animator.SetBool("canWalk", false);
        animator.SetBool("Attack", true);
    }

    void StopAttack ()
    {
        attackCooldown = false;
        attackMode = false;
        animator.SetBool("Attack", false);
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;

        if(timer <= 0 && attackCooldown && attackMode)
        {
            attackCooldown = false;
            timer = intTimer;
        }
    }

    public void TriggerCooldown()
    {
        attackCooldown = true;
    }

    void RaycastDebugger()
    {
        //visualize raycast behavior with debug rays
        if (distance > attackDistance)
        {
            if (isFacingRight) 
                Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength, Color.red);
            else
                Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        } else if (attackDistance > distance)
        {
            if (isFacingRight)
                Debug.DrawRay(rayCast.position, Vector2.right * rayCastLength, Color.green);
            else
                Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }

    void Flip()
    {
        //flip the enemy's facing direction
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}
