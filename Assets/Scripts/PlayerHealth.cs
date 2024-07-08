using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    Animator myAnimator;
    public float health;
    public float maxHealth;

    public Image emptyHB;


    private void Start()
    {

        myAnimator = GetComponent<Animator>();
        myAnimator.enabled = true;

        health = maxHealth;
    }

    public void ManageHealth(float healthAmount)
    {
        health += healthAmount;
    }


    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        //  Debug.Log("Health reduced by " + damageAmount + ". Current health: " + health);
    }

    public void Update()
    {
         if (health <= 0)
        {
            //  Debug.Log("Dead");
            emptyHB.enabled = false;
            myAnimator.SetBool("IsDead", true);
            Destroy(gameObject, 2f);
        }
    }
}

