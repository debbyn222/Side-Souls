/* Purpose:
Manage player health and damage logic

Controlls: HealthPoints (why not just use health and maxHealth from HealthBar script?)
*/
//Last Edit: 25th of June, 2024 @3:35pm
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

/* Notes:
    - Should be merged with HealthBar.cs
    - Or turned into proper component scripts\

    * Eventually remove Debug.Log statements as they will be replaced with UI updates.

    * Introduces new variable (HealthPoints) which does the same as other already existing variables (health) in HealthBar.cs
*/
