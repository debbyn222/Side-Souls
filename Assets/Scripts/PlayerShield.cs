using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public Collider2D shieldCollider;
    private Animator shieldAnimator;
    private bool isActive = false;
    
    void Start()
    {
        //shieldCollider = GetComponent<Collider2D>();
        shieldAnimator = GetComponent<Animator>();
        shieldCollider.enabled = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!isActive)
            {
                ActivateShield();
            }
        }
        else
        {
            if (isActive)
            {
                DeactivateShield();
            }
        }
    }

    void ActivateShield()
    {
        isActive = true;
        shieldCollider.enabled = true;
        shieldAnimator.SetTrigger("Block"); 
    }

    void DeactivateShield()
    {
        isActive = false;
        shieldCollider.enabled = false;
        shieldAnimator.ResetTrigger("Block");
    }
}