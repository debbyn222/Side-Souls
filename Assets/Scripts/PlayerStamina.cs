//Author: Cesar R. Molina-Lopez
//Date: 6/08/2024
//Purpose: Placeholder stamina script which interacts with combat script
/*
Placeholders include:
    - 
*/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public Slider staminaSlider; // Reference to the UI Slider for stamina
    [SerializeField] private float maxStamina = 15f;  // Maximum stamina value
    public float currentStamina;    // Current stamina value
    [SerializeField] private float staminaRegenRate = 1f; // Stamina points regenerated per second (want to update so it regenerates 1 full point once per second
    //Right now, this updates 1 full point over the course of a second. Looks ugly.

    [SerializeField] private float attackCost = 1f; //only one that's added in PlayerCombat as placeholder
    [SerializeField] private float dodgeCost = 2f; //not added in PlayerCombat as placeholder
    [SerializeField] private float rollCost = 3f; //not added in PlayerCOmbat as placeholder
    [SerializeField] private float parryReward = 3f;  // Stamina refunded on successful parry

    void Start()
    {
        // Initialize current stamina to minimum stamina
        currentStamina = 0f;
        Debug.Log("Stamina Initialized: " + currentStamina);
    }

    void Update()
    {
        // Regenerate stamina over time
        RegenerateStamina();
        Debug.Log("Current Stamina: " + currentStamina); //would like to get it to ONLY show whole numbers, couldn't figure it out yet though.
        // Update the slider value
        UpdateStaminaSlider();
    }
    void UpdateStaminaSlider()
    {
        // Set the slider value to the current stamina
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    void RegenerateStamina()
    {
        // Regenerate stamina if it's below the maximum
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
            //Debug.Log("Stamina Regenerated: " + currentStamina); (Unnecessary right now, maybe bring this back as a UI element)
        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            Debug.Log("Stamina Used: " + amount + ". Current Stamina: " + currentStamina);
            return true;  // Stamina was successfully used
        }
        else
        {
            Debug.Log("Not Enough Stamina!");
            return false; // Not enough stamina
        }
    }

    public void RefundStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
        Debug.Log("Stamina Refunded: " + amount + ". Current Stamina: " + currentStamina);
    }

    // Methods to be called from other scripts
    public void Attack()
    {
        UseStamina(attackCost);
    }

    public void Dodge()
    {
        UseStamina(dodgeCost);
    }

    public void Roll()
    {
        UseStamina(rollCost);
    }

    public void Parry()
    {
        RefundStamina(parryReward);
    }
}