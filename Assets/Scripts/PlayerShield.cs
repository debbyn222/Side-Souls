/*Purpose:
Manage player shield activation and deactivation
*/
//Last Edit: 25th of June, 2024 @3:26am PST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    public Collider2D shieldCollider; //collider component of shield
    private Animator shieldAnimator;
    private bool isActive = false;
    
    void Start() //Not sure why the first line is commented out. Edit that in a later revision.
    {
        //shieldCollider = GetComponent<Collider2D>();
        shieldAnimator = GetComponent<Animator>();
        shieldCollider.enabled = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) //activate shield on B press
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

/* Notes:
First line in Start() method is code that has been commented out. Either fix it or get rid of it.
    - (Was commented out originally, not a change done in editing phase.)
*/