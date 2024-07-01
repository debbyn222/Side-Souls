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

    private Stamina stamina;
    public AudioSource rollSound; //Reference to the AudioSource component
    public AudioSource attackSound; // Reference to the attack sound AudioSource component


    void Start()
    {
        stamina = GetComponent<Stamina>();//get stamina component attached to game obj
    }

    void Update()
    {
        //These methods (attack and roll) already exist elsewhere
        //Will most likely have to come back and go over this again
        // Handle attack input
        if (Input.GetKeyDown(KeyCode.K))
        {
            Attack();
        }

        // Handle roll input
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            Roll();
            //switched placeholder "M" key to actual roll keys allows for stamina to be deducted from roll
            //but looks ugly, could probably be cleaned up to look better.
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
            attackSound.Play(); //play attack sound
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
            rollSound.Play(); //plays roll sound
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
            Vector3 spawnPosition = hookSpawnPoint != null ? hookSpawnPoint.position : transform.position;
            currentHook = Instantiate(hookPrefab, spawnPosition, transform.rotation);
        }
        else if (!currentHook.activeInHierarchy)
        {
            Debug.Log("Reactivating hook.");
            currentHook.transform.position = hookSpawnPoint != null ? hookSpawnPoint.position : transform.position;
            currentHook.SetActive(true);
        }
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