/*Purpose:
Manages stamina, but not necessarily player specific anymore.
Includes: Regeneration, expenditure, and UI updates.
*/
//Last Edited: 25th of June, 2024 @1:23am PST
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Slider staminaSlider; // Reference to the UI Slider for stamina
    [SerializeField] private float maxStamina = 15f;  // Maximum stamina value
    [SerializeField] private float startingStamina = 0f; // Starting stamina value
    public float currentStamina;    // Current stamina value
    [SerializeField] private float staminaRegenRate = 1f; // Stamina points regenerated per second
    [SerializeField] private float regenCooldownTimerMax = 5f; // Maximum timer for the regeneration cooldown

    public float currentRegenCooldownTimer = 0f; // Current timer for the regeneration cooldown
    private bool isInAction = false; // Flag to indicate if in action and pause regeneration

    void Start()
    {
        currentStamina = startingStamina; //initialize current stamina to starting value
        UpdateStaminaSlider(); //update slider UI
    }

    void Update()
    {
        // Regenerate stamina over time if not in action or cooldown
        if (!isInAction && currentRegenCooldownTimer <= 0)
        {
            RegenerateStamina();
        }

        // Update the regeneration cooldown timer only when an action is performed
        if (currentRegenCooldownTimer > 0 && !isInAction)
        {
            currentRegenCooldownTimer -= Time.deltaTime;
            if (currentRegenCooldownTimer <= 0) //checks everytime to see if current timer left is at or lower than 0.
            {
                currentRegenCooldownTimer = 0;//resets to 0
                Debug.Log("Stamina regeneration cooldown ended.");
            }
        }

        // Update the slider value
        UpdateStaminaSlider();
    }

    //Update stamina slider to reflect current value
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
            currentStamina += (staminaRegenRate * Time.deltaTime);
            currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
        }
    }

    //Start an action that costs stamina
    public bool StartAction(float cost)
    {
        if (!isInAction)//if not doing anything
        {
            if (currentStamina >= cost)
            {
                isInAction = true;
                currentStamina -= cost; // Deduct stamina for the action
                currentRegenCooldownTimer = regenCooldownTimerMax; // Reset cooldown timer to max when starting action
                Debug.Log("Started action with stamina cost: " + cost);
                return true;
            }
            else
            {
                Debug.Log("Not enough stamina to perform this action! Current Stamina: " + currentStamina);
                return false;
            }
        }
        else //if already in action, do not allow new action to be performed simultaneously
        {
            Debug.Log("Cannot start action: already in action.");
            return false;
        }
    }

    public void EndAction()
    {
        if (isInAction)
        {
            isInAction = false; //reset in action flag
        }
    }

    public void RefundStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Min(currentStamina, maxStamina); // Ensure it doesn't exceed max stamina
        Debug.Log("Stamina Refunded: " + amount + ". Current Stamina: " + currentStamina);
    }

    // Special method for parry action
    public void ParryAction()
    {
        // Implement parry action here
        Debug.Log("Parry Successful! No stamina cost.");
        // Optionally: Add visual or audio feedback for successful parry
    }
}

//Notes:

//last bug to fix is parrying activating the regen cooldown
//found bug: in Parry.cs file

/*Can be fixed in two ways:
 * Add another if else statement to StartAction method so as to exclude ParryAction from triggering regen timer
 *  - Ideal solution. Just add 1 nested if-else statement and EZ fix
 *      - If not parry, then currentRegenCooldownTimer = regenCooldownTimerMax;
 *      - Else (if parry), then don't update currentRegenCooldownTimer. 
 * Edit ParryAction so it doesn't call StartAction.
 *  - Not ideal, would have to rewrite the rest of the StartAction logic specifically for parrying
 *  - On top of this, a major point in StartAction is the "isInAction" state being turned off/on
 *  - By going this route, it'd muddy the waters a lot, especially down the line when new mechanics are added and need to all keep track of whether or not the player is commiting an action
 *  - Overall, BAD solution.
 */