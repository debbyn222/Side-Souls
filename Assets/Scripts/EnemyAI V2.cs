using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class EnemyAIv2 : MonoBehaviour
{
    //Used for line of sight checks 
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength; 
    private RaycastHit2D hit;

    //Enemy Attributes
    public float speed;
    public float jumpForce;
    public float attackRange;
    public float timeBetweenAttacks;
    private bool isFacingRight = false;
    private float directionOfTravel;

    //Target information
    private GameObject target;
    private Vector2 targetDirection;
    private float distanceFromTarget;

    //Enemy components and states
    private Rigidbody2D rb;
    private Animator animator;
    private bool inDetectionArea;
    private bool alreadyAttacked;

    //Patrolling
    public LayerMask isGround;
    private Vector2 walkPoint;
    private bool walkPointSet = false;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with detection area then it sets the player as the target
        //Note: we can have a string targetTag; to have the target be whatever we want
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inDetectionArea = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inDetectionArea)
        {
            //If the player entered detection area start raycast towards to player direction to see if player is in line of sight
            targetDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
            targetDirection.Normalize();
            distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);

            hit = Physics2D.Raycast(rayCast.position, targetDirection, rayCastLength, ~rayCastMask);
            Debugger();

            if (hit.collider.CompareTag("Player"))
            {
                EnemyLogic();
            }
            else
            {
                //Note: Can change later for an "AlertPatroling()" function from the player being close
                //Have the enemy have a different animation and speed, either moving faster to search for the player or moving slower and acting more cautious
                Patroling();
            }
        }
        else
        {
            //If the player isnt in the detection area, start patroling
            Patroling();
        }
        
        //When the player gets out of the enemy's "line of sight" or further than rayCastLength, player is no longer in detection area
        if (distanceFromTarget > rayCastLength)
        {
            inDetectionArea = false;
        }
        
    }
    
    void EnemyLogic()
    {            
        if (!alreadyAttacked)
        {
            if (distanceFromTarget > attackRange)
            {
                directionOfTravel = targetDirection.x;
                Move();
            }
            else if (distanceFromTarget <= attackRange)
            {
                Attack();
            }
        }
        else
        {
            animator.SetInteger("AnimState", 0);
        }
  
    }

    void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();       
        
        if (walkPointSet)
        {
            if (walkPoint.x < transform.position.x)
                directionOfTravel = -1;
            else 
                directionOfTravel = 1;
            Move();
        }

        Vector2 distanceToWalkPoint = (Vector2)transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    void SearchWalkPoint()
    {
        float walkPointRange = 10;
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector2(transform.position.x + randomX, transform.position.y);

        if (Physics2D.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true;

    }
    
    void Move()
    {
        animator.SetInteger("AnimState", 1);

        //Make the enemy turn to the direction that its moving
        if (rb.velocity.x < 0 && isFacingRight || rb.velocity.x > 0 && !isFacingRight)
        {
            Flip();
            isFacingRight = !isFacingRight;
        }

        rb.velocity = new Vector2(speed * directionOfTravel, rb.velocity.y);
    }

    //
    void Attack()
    {   
        //Make the enemy stop moving if ranged enemy
        //rb.velocity = new Vector2(0, rb.velocity.y);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void Flip()
    {
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void Debugger()
    {
        if (inDetectionArea && distanceFromTarget > attackRange)
            Debug.DrawRay(rayCast.position, targetDirection * rayCastLength, Color.red);
        else if (inDetectionArea && distanceFromTarget <= attackRange)
            Debug.DrawRay(rayCast.position, targetDirection * rayCastLength, Color.green);
    }

}
