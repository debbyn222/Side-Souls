/*Purpose:
Manages combat actions.
Includes: Attacking, Rolling, stamina consumption for actions
*/
//Last Edited: 25th of June, 2024 @12:12am
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] public float attackCost = 1f; // Stamina cost for an attack
    [SerializeField] private float rollCost = 3f;  // Stamina cost for rolling
    public GameObject hookPrefab;
    public Transform hookSpawnPoint;
    private GameObject currentHook;
    public HookWeapon hookWeapon;

    private Stamina stamina;
    public AudioSource rollSound; // Reference to the roll sound AudioSource component
    public AudioSource attackSound; // Reference to the attack sound AudioSource component

    void Start()
    {
        stamina = GetComponent<Stamina>(); // Get stamina component attached to game object

        // Ensure the hookPrefab has a HookWeapon component
        if (hookPrefab != null)
        {
            hookWeapon = hookPrefab.GetComponent<HookWeapon>();
            if (hookWeapon == null)
            {
                Debug.LogError("Hook prefab does not have a HookWeapon component.");
            }
        }
        else
        {
            Debug.LogError("Hook prefab is not assigned.");
        }
    }

    void Update()
    {
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        // Handle roll input
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.F)) // Fire the Hook with F key
        {
            ShootHook();
        }
    }

    public void Attack()
    {
        if (stamina.StartAction(attackCost))
        {
            attackSound.Play(); // Play attack sound
            Debug.Log("Attack performed! Stamina deducted.");
            // Implement attack action here
            stamina.EndAction();
        }
        else
        {
            Debug.Log("Not enough stamina to perform the attack!");
            stamina.EndAction(); // Make sure to end action if not enough stamina
        }
    }

    public void Roll()
    {
        if (stamina.StartAction(rollCost))
        {
            rollSound.Play(); // Play roll sound
            Debug.Log("Roll performed! Stamina deducted.");
            // Implement roll action here
            stamina.EndAction();
        }
        else
        {
            Debug.Log("Not enough stamina to roll!");
            stamina.EndAction(); // Make sure to end action if not enough stamina
        }
    }

    void ShootHook()
    {
        if (currentHook == null)
        {
            Debug.Log("Creating new hook instance.");
            Vector3 spawnPosition = transform.position + (transform.localScale.x > 0 ? Vector3.right : Vector3.left) * 2; // Offset to spawn in front of the player
            currentHook = Instantiate(hookPrefab, spawnPosition, Quaternion.identity);
        }
        else if (!currentHook.activeInHierarchy)
        {
            Debug.Log("Reactivating hook.");
            Vector3 spawnPosition = transform.position + (transform.localScale.x > 0 ? Vector3.right : Vector3.left) * 2; // Offset to spawn in front of the player
            currentHook.transform.position = spawnPosition;
            currentHook.SetActive(true);
        }

        // Set hook direction based on player facing direction
        Vector3 launchDirection = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
        currentHook.GetComponent<HookWeapon>().LaunchHook(launchDirection);
    }


}



//Notes:

//Finish removing placeholders (technically done, but need to neatly integrate instead of reusing the same code. It's ugly and unnecessary).
//  - Placeholders are:
//      - Roll trigger (already matches real roll buttons and now deducts stamina)
//      - Attack trigger (already matches real attack button and deducts stamina)
//      - Jump? Never was cemented whether or not it would cause stamina loss, prob not tho

/*
 * Again, just to reiterate, this script COULD be left alone and it'd work perfectly fine.
 * This is because of the reused code from other scripts. 
 * Ideally, it should CALL the other methods, or else there's no point in those other methods.
 * When time is available, rewrite to CALL methods instead of just using same code from other scripts/methods.
*/