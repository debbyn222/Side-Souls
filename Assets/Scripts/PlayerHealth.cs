/* Purpose:
Manage player health and damage logic

Controlls: HealthPoints (why not just use health and maxHealth from HealthBar script?)
*/
//Last Edit: 25th of June, 2024 @3:35pm
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints = 10; //Initial health points of player

    public void Damage(int damageAmount)
    {
        healthPoints -= damageAmount; //reduce health by specified amount
        Debug.Log("Health reduced by " + damageAmount + ". Current health: " + healthPoints);

        if (healthPoints <= 0)
        {
            Debug.Log("Dead");
            Destroy(gameObject);//destroys game obj if health drops to or below zero
        }
    }
}

/* Notes:
    - Should be merged with HealthBar.cs
    - Or turned into proper component scripts\

    * Eventually remove Debug.Log statements as they will be replaced with UI updates.

    * Introduces new variable (HealthPoints) which does the same as other already existing variables (health) in HealthBar.cs
*/