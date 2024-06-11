using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int healthPoints = 10;

    
    public void Damage(int damageAmount)
    {
        healthPoints -= damageAmount;
        Debug.Log("Health reduced by " + damageAmount + ". Current health: " + healthPoints);

        
        if (healthPoints <= 0)
        {
            
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }
}
