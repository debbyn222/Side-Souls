using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    private IInteractable currentInteractable;
    public TextMeshProUGUI interactText;
    public BoxCollider2D bodyCollider;

    private void Start()
    {
        interactText.gameObject.SetActive(false);
        bodyCollider = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            currentInteractable.Interact(); //calls interact method in IInteractable interface that is implemented on every interactable object
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentInteractable = interactable;
                interactText.gameObject.SetActive(true);


                SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
                float spriteHeight = spriteRenderer.bounds.size.y;
                Vector3 spriteTopPosition = spriteRenderer.bounds.center + new Vector3(0, spriteHeight / 2, 0);//spriteheight/2 + center = top of sprite
                interactText.transform.position = spriteTopPosition + new Vector3(0, 0.3f, 0); //position text above object sprite
                interactText.SetText("[E] " + interactable.InteractDescription);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Interactable"))
        {
            IInteractable interactable = other.GetComponent<IInteractable>();
            if (interactable != null && currentInteractable == interactable)
            {
                currentInteractable = null;
                interactText.gameObject.SetActive(false);
            }
        }
    }
}
