using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    
    [SerializeField] private float speed = 20f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //transform.position += -transform.right * Time.deltaTime * speed;
        rb.velocity = transform.transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
