using UnityEngine;

public class HookWeapon : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 10f; //could lead to a possible bug
    private Vector3 startPosition;
    private bool isReturning = false;
    private Transform playerTransform;  // Store the player's Transform
    private Vector3 initialScale;
    private Transform hookedEnemy; // Store the reference to the hooked enemy
    private Collider2D hookCollider; // Reference to the hook's Collider2D component

    public AudioSource hookLaunchSound; //Reference to the audioSource component
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        initialScale = transform.localScale;
        gameObject.SetActive(false);
        hookCollider = GetComponent<Collider2D>(); // Get the Collider2D component
        Debug.Log("Hook initialized and deactivated.");
    }

    void Update()
    {
        if (!isReturning)
        {
            // Move the hook forward
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            Debug.Log("Hook moving forward.");
            // Check if the hook has reached its maximum distance
            if (Vector3.Distance(startPosition, transform.position) >= maxDistance) //could be possible bug 
            {
                isReturning = true;
                Debug.Log("Hook reached max distance, returning.");
            }
        }
        else
        {
            // Move the hook back to the player
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            Debug.Log("Hook returning to player.");
            // Reset the hook if it has returned to the player

            if (hookedEnemy != null)
            {
                // Move the hooked enemy towards the player as well
                hookedEnemy.position = Vector3.MoveTowards(hookedEnemy.position, playerTransform.position, speed * Time.deltaTime);
                Debug.Log("Hook is reeling in the enemy.");
            }

            if ((Vector3.Distance(transform.position, playerTransform.position) < 0.1f) && isReturning) //might lead to bug (might have fixed it)
            {
                ResetHook();
                Debug.Log("Hook returned to player and reset.");
            }
        }
    }

    void OnEnable()
    {
        hookLaunchSound.Play();
        startPosition = playerTransform.position;
        isReturning = false;
        hookedEnemy = null; // Reset the hooked enemy
        Debug.Log("Hook enabled and starting at: " + startPosition);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isReturning)
        {
            // Attach the enemy to the hook
            isReturning = true;
            hookedEnemy = other.transform;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.SetParent(transform);
            Debug.Log("Hook hit an enemy, returning.");
        }
    }

    void ResetHook()
    {
        isReturning = false;
        transform.position = playerTransform.position;
        transform.SetParent(null);
        transform.localScale = initialScale;
        gameObject.SetActive(false);

        // Detach the enemy
        if (hookedEnemy != null)
        {
            hookedEnemy.SetParent(null);
            hookedEnemy = null;
        }

        Debug.Log("Hook reset.");
    }

}

/* Notes:
 * This is still bugged, takes around 3-4 button presses (F) to actually fire the hook. Not sure why that's happening.
 * Some logic things need to be worked out, for example
    - Do you want the hook to be able to grab onto enemy from any angle?
        - For example:
        - IMO, there should be KNOCK BACK to enemy when they are hit from the FRONT, but,
        - When the hook is reeling back, that is when it can actually hook onto the enemy and drag them in.
        - These are all just ideas, can be edited later.
    - Only goes towards the left side of the screen. 
        - Should be able to go forwards or backwards depending on which way the player is facing.

    - Interestingly enough, this is how the hook ended up working. (unintentionally)
        - It knocks back the enemy when launching out
        - It acts like hook when hitting the enemy from behind as it comes back to the player.
        Think of it (in it's current state) as more of just an unstoppable object
            - whatever is in its way will move with it basically.

    - However, I rethought about this and kind of concluded it's a bit redundant for the hook to cause knock back
        - IFF we want to add a shield bash ability.
            - but if we decide against a shield bash ability, then the hook is fine to have the knock back and hook effect.
 */