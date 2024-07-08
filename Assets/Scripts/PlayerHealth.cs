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
    public int healthPoints = 10; //Initial health points of player
    //public DeathSceneManager deathSceneManager; // Reference to the DeathSceneManager script
    private EnemyAI enemyAI;

    public void Start()
    {
        enemyAI = GetComponent<EnemyAI>();
    }
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
            Debug.Log("Player is dead.");
            if (enemyAI != null && enemyAI.EnemyCheck())
            {
                // Additional logic for enemies, if needed
            }
            Destroy(gameObject); // Destroy player GameObject when health drops to zero
            //deathSceneManager.ShowDeathUI(); // Show death UI upon player death
        }
    }
}

/* Notes:
    - Should be merged with HealthBar.cs
    - Or turned into proper component scripts\

    * Eventually remove Debug.Log statements as they will be replaced with UI updates.

    * Introduces new variable (HealthPoints) which does the same as other already existing variables (health) in HealthBar.cs

New Edits July 7, 2024:
    * Commented out the Destroy(gameObject) line. This will be replaced with the animations later down the line.
        * We don't want the player to just disappear.
    * Added the UI line right underneath that commented out line
        * Makes it so that the death screen UI shows up.
    * Edited "Health" in class name to "PlayerHealth" so as to match the script name.
*/
