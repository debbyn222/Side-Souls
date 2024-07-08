using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollision : MonoBehaviour
{
    private Animator animator;
    private GameData gameData;
    [NonSerialized] public BoxCollider2D feetCollider;
    private BoxCollider2D bodyCollider;
    private BoxCollider2D shieldCollider;
    private Player player;
    [NonSerialized] public bool isGrounded;
    [NonSerialized] public bool isOnPlatform;
    [NonSerialized] public bool isOnGround;
    [NonSerialized] public bool isOnStairs;
    [NonSerialized] public bool isOnLadder;
    private bool isOnSpikes = false; //bool to prevent multiple coroutine calls
    public float platformFallThroughDuration = 0.3f;
   
    

    private void Start()
    {
        feetCollider = GetComponent<BoxCollider2D>();
        bodyCollider = transform.parent.Find("BodyCollider").GetComponent<BoxCollider2D>();
        shieldCollider = transform.parent.Find("ShieldCollider").GetComponent<BoxCollider2D>();
        //animator = GetComponentInParent<Animator>();
        animator = transform.parent.Find("PlayerSprite").GetComponent<Animator>();
        gameData = GetComponentInParent<PlayerMovement>().gameData;
        gameData.Initialize();
        player = gameData.player;
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
           player.speed = player.originalSpeed * 0.6f;
        }

    }

    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes") && !isOnSpikes)
        {
            StartCoroutine(TakeDamageOverTime());
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            gameData.player.speed = gameData.player.originalSpeed;
        }
    }


    public void CheckGrounded()
    {
        isOnPlatform = feetCollider.IsTouchingLayers(LayerMask.GetMask("Platform"));
        isOnGround = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        isOnStairs = feetCollider.IsTouchingLayers(LayerMask.GetMask("Stairs"));
        isOnLadder = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ladder")) || bodyCollider.IsTouchingLayers(LayerMask.GetMask("Ladder"));
        if (isOnGround || isOnPlatform || isOnStairs)
        {
            isGrounded = true;
            gameData.noFrictionMaterial.friction = isOnStairs ? 3 : 0; //increase friction when on stairs to stop sliding down
            
        }
        else
        {
            isGrounded = false;
            
        }
        animator.SetBool("Grounded", isGrounded);
    }

    private IEnumerator TakeDamageOverTime()
    {
        isOnSpikes = true; 
        while (true) // Loop to continuously check for damage
        {

            player.healthPoints = player.healthPoints - 10; // Tweak if needed
            //player.printStats();

            yield return new WaitForSeconds(1.5f); // Delay between health reductions

            // Check if the player is still in contact with the spikes
            if (!feetCollider.IsTouching(GameObject.FindWithTag("Spikes").GetComponent<Collider2D>()))
            {
                isOnSpikes = false;
                yield break;
            }
        }
    }

    public IEnumerator FallThroughPlatform()
    {
        int platformLayer = LayerMask.NameToLayer("Platform");

        // Disable collisions with platforms temporarily for all relevant colliders
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, true);
        bodyCollider.enabled = false;
        feetCollider.enabled = false;
        shieldCollider.enabled = false;

        yield return new WaitForSeconds(platformFallThroughDuration);

        // Re-enable collisions with platforms
        Physics2D.IgnoreLayerCollision(gameObject.layer, platformLayer, false);
        bodyCollider.enabled = true;
        feetCollider.enabled = true;
    }

}
