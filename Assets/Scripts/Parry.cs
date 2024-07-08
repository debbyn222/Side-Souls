/*Purpose:
Handles the parry mechanic, but not necessarily player specific anymore. 
Includes: Cooldowns, parry windows, and stamina refunds
*/
//Last Edited: 24th of June, 2024 @11:53pm PST
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Parry : MonoBehaviour
{
    [SerializeField] private float parryCooldown = 2f; // Cooldown time for parry in seconds
    [SerializeField] private float parryWindow = 2f; // Time window after imminent attack to trigger parry in seconds
    [SerializeField] private float parryReward = 3f; // Stamina refunded on successful parry

    private bool canParry = true;
    private float parryWindowEnd;
    private Stamina stamina;
    public Text parryIndicatorText;

    void Start()
    {
        stamina = GetComponent<Stamina>();
        if (parryIndicatorText != null)
        {
            parryIndicatorText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Handle imminent attack input for testing (PLACEHOLDER)
        //Swap out for check on incoming attack (can be done with a bool indicator)
        if (Input.GetKeyDown(KeyCode.P)) //swap out
        {
            Debug.Log("Parry Window Open!");
            parryWindowEnd = Time.time + parryWindow;
        }

        // Handle parry input within the parry window (Semi PLACEHOLDER)
        //Add damage function (look at "ParryAction()" method). Should be different from attack button tho (K)
        if (Input.GetKeyDown(KeyCode.O) && Time.time < parryWindowEnd)
        {
            if (canParry)
            {
                ParryAction();
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

    //Perform the parry action
    /*
    Need to add actual parry logic: Currently only a simulated parry
        - Connect to damage scripts
        - Add animations
    */
    void ParryAction()
    {//this is where the bug is: Calls StartAction method
        stamina.StartAction(0); // 0 cost action (THIS SHOULD NOT BE CALLING StartAction)
        Debug.Log("Parry Successful!");
        stamina.RefundStamina(parryReward); // Refund stamina
        StartCoroutine(ParryCooldown());
        StartCoroutine(ShowParryIndicator());
        stamina.EndAction();
    }
    /*
     * The reason it should not be calling StartAction is because:
       - it (parrying) is then labeled as an action
       - within StartAction() method, ALL actions cause StaminaRegenCooldownTimer to reset
       - Parrying should NOT cause the timer to reset. why punish a parry?
     * OR, if you want to call StartAction()
       - Make it so that parrying doesnt cause the StaminaRegenCooldownTimer to reset
       - Could be down with an if-else statement and possibly a bool var if needed
     */

    //Handles parry cooldown
    IEnumerator ParryCooldown()
    {
        canParry = false;
        Debug.Log("Parry Cooldown Started.");
        yield return new WaitForSeconds(parryCooldown);
        canParry = true;
        Debug.Log("Parry Cooldown Ended.");
    }

    //Show the parry indicator text for a short duration
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

//Notes: (also check Stamina.cs):

//editing ParryAction() method seems to be the last thing needed
//  - Then, also removing debug statements and reviewing code (not of high importance, could be left alone)