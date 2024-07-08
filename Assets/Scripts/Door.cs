using UnityEngine;

public class Door : MonoBehaviour
{
    private BoxCollider2D doorCollider;
    private Animator animator;

    void Start()
    {
        doorCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        doorCollider.enabled = false; // Disable the collider to open the passage
        if (animator != null)
        {
            animator.SetTrigger("Open"); // Play door opening animation
        }
        // Add sound effect here if needed
    }

    public void CloseDoor()
    {
        doorCollider.enabled = true; // Enable the collider to close the passage
        if (animator != null)
        {
            animator.SetTrigger("Close"); // Play door closing animation
        }
        // Add sound effect here if needed
    }
}