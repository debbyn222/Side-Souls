using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI_V2 : MonoBehaviour
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

    //Target information
    private GameObject target;
    private Vector2 targetDirection;
    private float distanceFromTarget;

    //Enemy components and states
    private Rigidbody2D rb;
    private Animator animator;
    private bool inDetectionArea;
    private bool alreadyAttacked;
    private bool isPatrolling;

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
            CheckLineOfSight();
            Debugger();

            if (hit.collider.CompareTag("Player"))
            {
                EnemyLogic();
                isPatrolling = false;
            }
            else
            {
                //Note: Can change later for an "AlertPatroling()" function from the player being close
                //Have the enemy have a different animation and speed, either moving faster to search for the player or moving slower and acting more cautious
                Patroling();
                isPatrolling = true;
            }
        }
        else
        {
            //If the player isnt in the detection area, start patroling
            if (!alreadyAttacked)
            {
                Patroling();
                isPatrolling = true;
            }
        }
        
        //When the player gets out of the enemy's "line of sight" or further than rayCastLength, player is no longer in detection area
        if (distanceFromTarget > rayCastLength)
        {
            inDetectionArea = false;
        }
        
    }

    void CheckLineOfSight()
    {
        targetDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        targetDirection.Normalize();
        distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
        hit = Physics2D.Raycast(rayCast.position, targetDirection, rayCastLength, ~rayCastMask);
    }
    
    void EnemyLogic()
    {            
        if (!alreadyAttacked)
        {
            if (distanceFromTarget > attackRange)
            {
                Move(speed, targetDirection.x);
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
        float patrolSpeed = speed/3;

        if (isPatrolling)
            animator.SetFloat("AnimRunSpeed", 0.3f);
        else
            animator.SetFloat("AnimRunSpeed", 1f);

        if (!walkPointSet)
            SearchWalkPoint();
        
        if (walkPointSet)
        {
            if (walkPoint.x < transform.position.x)
                Move(patrolSpeed, -1);
            else 
                Move(patrolSpeed, 1);
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
    
    void Move(float moveSpeed, float directionOfTravel)
    {
        animator.SetInteger("AnimState", 1);

        //Make the enemy turn to the direction that its moving
        if (rb.velocity.x < 0 && isFacingRight || rb.velocity.x > 0 && !isFacingRight)
        {
            Flip();
        }

        rb.velocity = new Vector2(moveSpeed * directionOfTravel, rb.velocity.y);
    }


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
        isFacingRight = !isFacingRight;

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
