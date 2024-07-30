using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    
    [SerializeField] private float speed = 10f;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isMoving = true;
    //[SerializeField] private LayerMask layerMask;
    //[SerializeField] private LayerMask ignoredLayers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //transform.position += -transform.right * Time.deltaTime * speed;
        if (isMoving)
            rb.velocity = transform.transform.right * speed;
        else
            rb.velocity = new Vector2(0,0);
            transform.position = new Vector2(transform.position.x,transform.position.y);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        animator.SetTrigger("Explode");
        isMoving = false;
        Invoke(nameof(DeleteProjectile), 1f);
    }

    void DeleteProjectile()
    {
        Destroy(gameObject);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(layerMask))
        {
            animator.SetTrigger("Explode");
            Destroy(gameObject);
        }
    }*/
}
