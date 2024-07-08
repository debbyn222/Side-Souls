using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("Line of Sight Parameters")]
    public Transform rayCast;
    public LayerMask rayCastMask;
    public float rayCastLength;
    public RaycastHit2D hit;
    public bool inDetectionArea;

    [Header("Target Information")]
    [SerializeField] private string targetTag = "Player";
    public GameObject target;
    public Vector2 targetDirection;
    public float distanceFromTarget; 

    void OnTriggerEnter2D(Collider2D collision)
    {
        //if player collides with detection area then it sets the player as the target
        if (collision.gameObject.tag == targetTag)
        {
            target = collision.gameObject;
            inDetectionArea = true;
        }
    }

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

    public bool PlayerInSight()
    {
        return CheckLineOfSight();
    }

    // Update is called once per frame
    void Update()
    {
        if (inDetectionArea)
        {
            CheckLineOfSight();
        }

        //When the player gets out of the enemy's "line of sight" or further than rayCastLength, player is no longer in detection area
        if (distanceFromTarget > rayCastLength)
        {
            inDetectionArea = false;
        }
    }
}
