/* Purpose:
Handles dealing damage to objects with a Health component when entering trigger area
*/
//Last Edited: 25th of June, 2024 @1:36am PST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
   private int damage = 3;//amount of damage to deal
   private void OnTriggerEnter2D(Collider2D collider)
   {
    if(collider.GetComponent<Health>() != null) //checks for Health component
    {
        Health health = collider.GetComponent<Health>(); //get reference to Health component of colliding object
        health.Damage(damage);//Deal damage to Health component
    }
   }
}
