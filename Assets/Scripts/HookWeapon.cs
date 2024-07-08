using UnityEngine;

public class HookWeapon : MonoBehaviour
{
    public float speed = 10f;
    public float maxDistance = 10f;
    private Transform playerTransform;
    private bool isReturning = false;
    private Transform hookedEnemy;
    private AudioSource hookLaunchSound;
    private Vector3 initialScale;
    private Vector3 launchDirection;

    void Awake()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        hookLaunchSound = GetComponent<AudioSource>();

        if (hookLaunchSound == null)
        {
            Debug.LogError("AudioSource component not found on HookWeapon GameObject.");
        }
    }

    void Start()
    {
        initialScale = transform.localScale;
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (!isReturning)
        {
            transform.Translate(launchDirection * speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, playerTransform.position) >= maxDistance)
            {
                isReturning = true;
            }
        }
        else
        {
            Vector3 returnPosition = playerTransform.position + launchDirection * 0.5f; // Adjust to stop before hitting the player
            transform.position = Vector3.MoveTowards(transform.position, returnPosition, speed * Time.deltaTime);

            if (hookedEnemy != null)
            {
                hookedEnemy.position = Vector3.MoveTowards(hookedEnemy.position, playerTransform.position, speed * Time.deltaTime);
            }

            // Adjusted logic to account for both sides
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                ResetHook();
            }
        }
    }

    void OnEnable()
    {
        if (hookLaunchSound != null)
        {
            hookLaunchSound.Play();
        }
        else
        {
            Debug.LogWarning("hookLaunchSound is null in OnEnable().");
        }

        launchDirection = playerTransform.localScale.x > 0 ? Vector3.right : Vector3.left;
        transform.position = playerTransform.position + launchDirection * 0.5f; // Adjusted spawn position
        isReturning = false;
        hookedEnemy = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && !isReturning)
        {
            isReturning = true;
            hookedEnemy = other.transform;
            Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.velocity = Vector2.zero; // Stop the enemy's velocity
                enemyRigidbody.bodyType = RigidbodyType2D.Kinematic;
            }
            other.transform.SetParent(transform);
        }
    }

    void ResetHook()
    {
        isReturning = false;
        transform.position = playerTransform.position;
        transform.SetParent(null);
        transform.localScale = initialScale;
        gameObject.SetActive(false);

        if (hookedEnemy != null)
        {
            Rigidbody2D enemyRigidbody = hookedEnemy.GetComponent<Rigidbody2D>();
            if (enemyRigidbody != null)
            {
                enemyRigidbody.bodyType = RigidbodyType2D.Dynamic; // Set back to Dynamic for normal physics behavior
            }
            hookedEnemy.SetParent(null);
            hookedEnemy = null;
        }
    }

    public void LaunchHook(Vector3 direction)
    {
        launchDirection = direction;
        gameObject.SetActive(true);

        if (hookLaunchSound != null)
        {
            hookLaunchSound.Play();
        }
        else
        {
            Debug.LogWarning("hookLaunchSound is null in LaunchHook().");
        }
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