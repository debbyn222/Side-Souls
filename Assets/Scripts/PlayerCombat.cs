using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float parryCooldown = 2f; // Cooldown time for parry
    public float parryDuration = 0.5f; // Duration of the parry action
    public float parryWindow = 0.2f; // Time window after imminent attack to trigger parry

    private bool canParry = true;
    private float parryWindowEnd; // End time of the parry window
    private PlayerStamina playerStamina;

    void Start()
    {
        playerStamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        // Simulate imminent attack input for testing (replace with actual enemy detection logic later)
        if (Input.GetKeyDown(KeyCode.P))
        {
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
        }
    }

    void Parry()
    {
        // Implement parry action here
        // For now, we'll just print a message and refund stamina
        Debug.Log("Parry!");
        playerStamina.Parry(); // Refund stamina
        StartCoroutine(ParryCooldown());
    }

    IEnumerator ParryCooldown()
    {
        canParry = false;
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
    }
}
