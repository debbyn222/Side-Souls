using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    public Slider staminaSlider; // Reference to the UI Slider for stamina
    [SerializeField] private float maxStamina = 15f;  // Maximum stamina value
    public float currentStamina;    // Current stamina value
    [SerializeField] private float staminaRegenRate = 1f; // Stamina points regenerated per second

    [SerializeField] private float attackCost = 1f;
    [SerializeField] private float dodgeCost = 2f;
    [SerializeField] private float rollCost = 3f;
    [SerializeField] private float parryReward = 3f;  // Stamina refunded on successful parry

    void Start()
    {
        // Initialize current stamina to max stamina
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
            Debug.Log("Stamina Regenerated: " + currentStamina);
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