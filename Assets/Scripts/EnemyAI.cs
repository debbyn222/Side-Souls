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
    public float timer; //Timer for cooldown between attacks

    private RaycastHit2D hit;
    private GameObject target;
    private Animator animator;
    private float distance; //distance between enemy and player
    private bool attackMode;
    private bool inRange; //chcek if player is in range
    private bool attackCooldown; //Check is enemy is still cooling down
    private float intTimer;


    void Awake()
    {
        intTimer = timer; //store intial value of timer
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            hit = Physics2D.Raycast(rayCast.position, Vector2.left, rayCastLength, rayCastMask);
            RaycastDebugger();
        }

        //If the player is detected
        if(hit.collider != null)
        {
            EnemyLogic();
        } else if (hit.collider == null)
        {
            inRange = false;
        }

        if(inRange == false)
        {
            animator.SetBool("canWalk", false);
            StopAttack(); 
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with triggerarea then player is in range
        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            inRange = true;
            Debug.Log("Player in range");
        }
    }

    void EnemyLogic()
    {
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
        animator.SetBool("canWalk", true);
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Attack"))
        {
            Vector2 targetPosition = new Vector2(target.transform.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
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
        if (distance > attackDistance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.red);
        } else if (attackDistance > distance)
        {
            Debug.DrawRay(rayCast.position, Vector2.left * rayCastLength, Color.green);
        }
    }

}
