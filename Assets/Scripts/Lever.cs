using UnityEngine;

public class Lever : MonoBehaviour
{
    public Animator doorAnimator; // Reference to the Animator component on the Door GameObject
    private bool isPlayerInRange = false;

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
        // Optionally, play lever animation or sound effect here
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