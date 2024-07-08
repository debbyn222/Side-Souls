using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController_Ranged : MonoBehaviour 
{
    EnemyDetector enemyDetector;

    //TODO: Replace Enemy Attributes with a separate script
    [Header("Enemy Attributes")]
    public float speed;
    public float jumpForce;
    public float attackRange;
    public float timeBetweenAttacks;
    public bool isFacingRight = false;

    //Enemy components and states
    [Header("Enemy Components")]
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] private bool alreadyAttacked;
    public GameObject projectile;
    public Transform projectileLaunchPoint;

    //Patrolling
    public LayerMask isGround;
    private Vector2 walkPoint;
    private bool walkPointSet = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        enemyDetector = GetComponentInChildren<EnemyDetector>();
    }

    void Update()
    {
        if (enemyDetector.inDetectionArea)
        {
            //Starts by checking if the enemy has line of sight
            if (enemyDetector.PlayerInSight())
            {
                EnemyLogic();
            }
            else
            {
                //Note: Can change later for an "AlertPatroling()" function from the player being close
                //Have the enemy have a different animation and speed, either moving faster to search for the player or moving slower and acting more cautious
                Patroling();
            }
            Debugger();
        }
        else
        {
            //If the player isnt in the detection area, start patroling
            if (!alreadyAttacked)
            {
                Patroling();
            }
        }
    }

    void EnemyLogic()
    {
        animator.SetFloat("AnimRunSpeed", 1f);

        if (!alreadyAttacked)
        {
            if (enemyDetector.distanceFromTarget > attackRange)
            {
                Move(speed, enemyDetector.targetDirection.x);
            }
            else if (enemyDetector.distanceFromTarget <= attackRange)
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
        float patrolSpeed = speed / 3;
        animator.SetFloat("AnimRunSpeed", 0.3f);

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
        if (!alreadyAttacked)
        {
            animator.SetTrigger("Attack");
            LookAtTarget();
            alreadyAttacked = true;

            projectileLaunchPoint.transform.right = enemyDetector.targetDirection;

            //TODO: Should making projectile spawn here during this attack funciton 
            Invoke(nameof(SpawnProjectile), 0.9f);

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void SpawnProjectile()
    {
        //Note: For now just making this an event trigger on the wizard attack animation
        Instantiate(projectile, projectileLaunchPoint.position, projectileLaunchPoint.rotation);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    void LookAtTarget()
    {
        if (enemyDetector.target.transform.position.x < transform.position.x && isFacingRight || enemyDetector.target.transform.position.x > transform.position.x && !isFacingRight)
            Flip();
    }

    void Debugger()
    {
        if (enemyDetector.inDetectionArea && enemyDetector.distanceFromTarget > attackRange)
            Debug.DrawRay(enemyDetector.rayCast.position, enemyDetector.targetDirection * enemyDetector.rayCastLength, Color.red);
        else if (enemyDetector.inDetectionArea && enemyDetector.distanceFromTarget <= attackRange)
            Debug.DrawRay(enemyDetector.rayCast.position, enemyDetector.targetDirection * enemyDetector.rayCastLength, Color.green);
    }
}
