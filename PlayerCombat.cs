using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private float parryCooldown = 2f; // Cooldown time for parry in seconds
    //[SerializeField] private float parryDuration = 1f; // Duration of the parry action in seconds
    [SerializeField] private float parryWindow = 2f; // Time window after imminent attack to trigger parry in seconds
    [SerializeField] public float attackCost = 1f; // Stamina cost for an attack

    private bool canParry = true;
    private float parryWindowEnd; // End time of the parry window
    private PlayerStamina playerStamina;

    public Text parryIndicatorText;

    void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
        if (parryIndicatorText != null)
        {
            parryIndicatorText.gameObject.SetActive(false);
        }
    }

    void Update()
{
    // Simulate attack input using the "A" key
    if (Input.GetKeyDown(KeyCode.K))
    {
        // Call the method to perform the attack action
        Attack();
    }

    // Simulate imminent attack input for testing (replace with actual enemy detection logic later)
    if (Input.GetKeyDown(KeyCode.P))
    {
        Debug.Log("Parry Window Open!");
        // Simulate imminent attack
        parryWindowEnd = Time.time + parryWindow;
    }

    // Parry input using a different key (e.g., "O") within the parry window
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
    void Attack()
    {
        // Implement attack action here
        // For now, let's deduct stamina for the attack
        if (playerStamina.UseStamina(attackCost))
        {
            Debug.Log("Attack performed! Stamina deducted.");
        }
        else
        {
            Debug.Log("Not enough stamina to perform the attack!");
        }
    }

    void Parry()
    {
        // Implement parry action here
        // For now, we'll just print a message and refund stamina
        Debug.Log("Parry Successful!");
        playerStamina.Parry(); // Refund stamina
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
