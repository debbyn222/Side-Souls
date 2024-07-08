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
    private string targetTag = "Player";
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
        if (collision.gameObject.tag == targetTag)
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
            //Starts by checking if the enemy has line of sight
            if (CheckLineOfSight())
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
        
        //When the player gets out of the enemy's "line of sight" or further than rayCastLength, player is no longer in detection area
        if (distanceFromTarget > rayCastLength)
        {
            inDetectionArea = false;
        }
        
    }

    //Makes a line of sight check from the enemy's rayCast child object towards the target and checks if the target is in sight
    bool CheckLineOfSight()
    {
        targetDirection = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y);
        targetDirection.Normalize();
        distanceFromTarget = Vector2.Distance(transform.position, target.transform.position);
        hit = Physics2D.Raycast(rayCast.position, targetDirection, rayCastLength, ~rayCastMask);

        if (hit.collider.CompareTag(targetTag))
            return true;
        else 
            return false;
    }
    
    void EnemyLogic()
    {
        animator.SetFloat("AnimRunSpeed", 1f);

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
