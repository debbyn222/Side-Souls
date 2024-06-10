using System.Collections;
using UnityEngine;

public class PlayerStamina : MonoBehaviour
{
    public float maxStamina = 15f;  // Maximum stamina value
    public float currentStamina;    // Current stamina value
    public float staminaRegenRate = 1f; // Stamina points regenerated per second

    public float attackCost = 1f;
    public float dodgeCost = 2f;
    public float rollCost = 3f;
    public float parryReward = 3f;  // Stamina refunded on successful parry

    void Start()
    {
        // Initialize current stamina to max stamina
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Regenerate stamina over time
        RegenerateStamina();
    }

    void RegenerateStamina()
    {
        // Regenerate stamina if it's below the maximum
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
        }
    }

    public bool UseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            return true;  // Stamina was successfully used
        }
        else
        {
            return false; // Not enough stamina
        }
    }

    public void RefundStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
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