/* Purpose:
Manage and display the health bar of a game object

Controlls: Current + Max Health and References health bar (UI)

What it does:
    - Start: Sets current health to max health
    - Update: Matches UI health bar to current health value, and checks if player HP > 0 (or else die)
*/
//Last Edited: 25th of June, 2024 @1:44am PST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float health; //current health
    public float maxHealth = 10; //initializes maxHealth variable and sets it to 10
    public Image healthBar; //refernece to UI healthbar
    public DeathSceneManager deathSceneManager; // Reference to the DeathSceneManager script
    public PlayerMovementUpdated playerMovement; // Reference to PlayerMovementUpdated script


    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth; //health is (re)set to maxHealth

        // Ensure playerMovement is assigned
        playerMovement = GetComponent<PlayerMovementUpdated>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovementUpdated component not found on the same GameObject as HealthBar.");
        }
    }
    /* should be the other way around tho imo? as in: "health = maxHealth;"
        - however I could see a potential flaw
            - Player takes damage, backs out of game, health is set equal to maxHealth
            - continue and repeat to cheese way through game
    */

    //I ended up just changing it myself. Original code was: "maxHealth = health;"
    //Original comment for the original code (above) was: //maxhealth is initialized to be equal to health at start

    // Update is called once per frame
    void Update()
    {
        //update fill amount of the health bar based on current health
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        if (health <= 0)
        {
            deathSceneManager.ShowDeathUI();
            playerMovement.ResetPlayerState(); // Reset player state when health reaches zero
        }
    }
}
/* Notes:
    - Should be merged with PlayerHealth.cs
    - Or turned into proper component scripts
*/