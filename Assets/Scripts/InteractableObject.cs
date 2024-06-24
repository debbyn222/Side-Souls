using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    protected GameObject player;
    protected PlayerMovement playerMovement;
    public string InteractDescription { get; set; }

    protected virtual void Awake()
    {
        // Find the player object once when the game starts
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        else
        {
            Debug.LogWarning("Player object not found in the scene.");
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //layer = GameObject.FindGameObjectsWithTag("Player")[0];
            //player = other.gameObject;
            //pyerMovement = player.GetComponent<PlayerMovement>();

            playerMovement.interactText.gameObject.SetActive(true);
            SpriteRenderer spriteRenderer = other.GetComponent<SpriteRenderer>();
            float spriteHeight = spriteRenderer.bounds.size.y;
            Vector3 spriteTopPosition = spriteRenderer.bounds.center + new Vector3(0, spriteHeight / 2, 0);//spriteheight/2 + center = top of sprite
            playerMovement.interactText.transform.position = spriteTopPosition + new Vector3(0, 0.3f, 0); //position text above object sprite
            playerMovement.interactText.SetText("[E] " + InteractDescription);

            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    // Abstract method to be implemented by child classes
    public abstract void Interact();
}
