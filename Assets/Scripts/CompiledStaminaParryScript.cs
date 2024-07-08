/*Purpose:
Merge player combat mechanics, stamina system, and parry mechanic all into a single script
*/
//Last Edit: 25th of June, 2024 @11:50am PST
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CompiledStaminaParryScript : MonoBehaviour
{
    // Stamina related variables
    public Slider staminaSlider; // Reference to the UI Slider for stamina
    [SerializeField] private float maxStamina = 15f;  // Maximum stamina value
    public float currentStamina;    // Current stamina value
    [SerializeField] private float staminaRegenRate = 1f; // Stamina points regenerated per second

    [SerializeField] private float attackCost = 1f;
    [SerializeField] private float dodgeCost = 2f;
    [SerializeField] private float rollCost = 3f;
    [SerializeField] private float parryReward = 3f;

    // Combat related variables
    [SerializeField] private float parryCooldown = 2f; // Cooldown time for parry in seconds
    [SerializeField] private float parryWindow = 2f; // Time window after imminent attack to trigger parry in seconds

    private bool canParry = true;
    private float parryWindowEnd; // End time of the parry window

    public Text parryIndicatorText;

    void Start()
    {
        // Initialize current stamina to minimum stamina
        currentStamina = 0f;
        Debug.Log("Stamina Initialized: " + currentStamina);

        if (parryIndicatorText != null)
        {
            parryIndicatorText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Regenerate stamina over time
        RegenerateStamina();
        Debug.Log("Current Stamina: " + (int)currentStamina); // Showing only whole numbers

        // Update the slider value
        UpdateStaminaSlider();

        // Handle attack input
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        // Handle imminent attack input for testing
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Parry Window Open!");
            parryWindowEnd = Time.time + parryWindow;
        }

        // Handle parry input within the parry window
        if (Input.GetKeyDown(KeyCode.O) && Time.time < parryWindowEnd)
        {
            if (canParry)
            {
                Parry();
            }
            else
            {
                Debug.Log("Can't Parry! Cooldown Active.");
            }
        }

        // Log when parry window is over
        if (Time.time >= parryWindowEnd && parryWindowEnd != 0)
        {
            Debug.Log("Parry Window Over!");
            parryWindowEnd = 0; // Reset to avoid multiple logs
        }
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
        if (UseStamina(attackCost))
        {
            Debug.Log("Attack performed! Stamina deducted.");
        }
        else
        {
            Debug.Log("Not enough stamina to perform the attack!");
        }
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
        StartCoroutine(ParryCooldown());
        StartCoroutine(ShowParryIndicator());
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        Debug.Log("Parry Cooldown Started.");
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
        Debug.Log("Parry Cooldown Ended.");
    }

    IEnumerator ShowParryIndicator()
    {
        if (parryIndicatorText != null)
        {
            parryIndicatorText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Display for half a second
            parryIndicatorText.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Parry Indicator Text is not assigned.");
        }
    }
}

/* Notes:
 - This version of the compiled script falls behind debugging edits + improvements of the separate scripts (Parry.cs | Stamina.cs| Combat.cs)
    - This will only be updated if team chooses to throw away the component scripts in favor for compiled scripts
    - Otherwise this script will be discarded
*/