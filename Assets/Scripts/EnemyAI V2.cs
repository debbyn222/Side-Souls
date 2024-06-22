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

    //Target information
    private GameObject target;
    private Vector2 targetDirection;
    private float distanceFromTarget;

    //Enemy components and states
    private Rigidbody2D rb;
    private Animator animator;
    private bool inDetectionArea;
    private bool inSightRange; //Seeing if the player is in sight 
    private bool inAttackRange; //Seeing if the player is close enough to attack
    private bool alreadyAttacked;

    private float directionOfTravel;
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
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inDetectionArea = true;
        }
    }

    /*private void OnTriggerExit2D(Collider2D collision)
    {
        inDetectionArea = false;
    }*/

    // Update is called once per frame
    void Update()
    {
        //Creates a raycast with the direction of the player if they're in the detection area
        if (inDetectionArea)
        {
            targetDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
            targetDirection.Normalize();

            hit = Physics2D.Raycast(rayCast.position, targetDirection, rayCastLength, rayCastMask);
            Debugger();
        }
        //if (hit.collider.gameObject.tag == "Player")
        if (hit.collider != null)
        {
            inSightRange = true;
            EnemyLogic();
        } 
        //else if (hit.collider.gameObject.tag != "Player")
        else if (hit.collider == null)
        {
            inSightRange = false;
            inDetectionArea = false;
            Patroling();
        }
        
    }
    
    void EnemyLogic()
    {            
        distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
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
        animator.SetBool("canWalk", true);

        //Make the enemy turn to the direction that its moving
        if (rb.velocity.x < 0 && isFacingRight || rb.velocity.x > 0 && !isFacingRight)
        {
            Flip();
            isFacingRight = !isFacingRight;
        }

        rb.velocity = new Vector2(speed * directionOfTravel, rb.velocity.y);
    }

    void Attack()
    {
        animator.SetBool("Attack", true);
        animator.SetBool("canWalk", false);

        //Make the enemy stop moving
        rb.velocity = new Vector2(0, rb.velocity.y);

        if (!alreadyAttacked)
        {
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        animator.SetBool("Attack", false);
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
