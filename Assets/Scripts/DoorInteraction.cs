using UnityEngine;
using System.Collections;

public class DoorInteraction : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component on the Door GameObject
    private BoxCollider2D doorCollider; // Reference to the BoxCollider2D component on the Door GameObject
    private bool isPlayerInRange = false;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.O))
        {
            Interact();
        }
    }

    void Interact()
    {
        // Trigger the "Open" animation parameter in the doorAnimator
        doorAnimator.SetTrigger("Open");

        // Disable the collider briefly to allow the player to pass through
        doorCollider.enabled = false;

        // Optionally, play lever animation or sound effect here

        // Wait for a duration and then close the door automatically
        StartCoroutine(CloseDoorAfterDelay(2f)); // Adjust the delay as needed
    }

    IEnumerator CloseDoorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Trigger the "Close" animation parameter in the doorAnimator
        doorAnimator.SetTrigger("Close");

        // Enable the collider to block the passage again
        doorCollider.enabled = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show interaction UI, e.g., "Press O to interact"
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Hide interaction UI
        }
    }
}
