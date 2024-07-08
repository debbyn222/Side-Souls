/*Purpose:
Handle damage inflicted on the player upon collision
*/
//Last Edit: 25th of June, 2024 @3:11am PST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public Health pHealth;
    public float damage = 2;
     
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pHealth.TakeDamage(damage);
        }
        //check for collision with player and apply damage & decrease player's health
    }
}

/* Notes:
    - Adds a new variable (pHealth) that references the UI helath bar, although this already exists within another script. (HealthBar.cs)
    - This only really checks if the player is colliding with the UI element for damage on screen (purple ball)
        - Figure out how, or if, this can be applied instead more universally to any incoming projectile. 
            - Will most likely have to be made into a component script for enemy projectiles and applied to them.
            - Also can be applied to damage objects like traps or (dangerous) water etc.
 */