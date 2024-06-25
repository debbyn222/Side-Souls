using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetCollision : MonoBehaviour
{
    private Animator animator;
    private GameData gameData;
    private BoxCollider2D feetCollider;
    private BoxCollider2D bodyCollider;
    private BoxCollider2D shieldCollider;
    [NonSerialized] public bool isGrounded;
    [NonSerialized] public bool isOnPlatform;
    [NonSerialized] public bool isOnGround;
    [NonSerialized] public bool isOnStairs;
    [NonSerialized] public bool isOnLadder;
    public float platformFallThroughDuration = 0.3f;


    private void Start()
    {
        feetCollider = GetComponent<BoxCollider2D>();
        bodyCollider = transform.parent.Find("BodyCollider").GetComponent<BoxCollider2D>();
        shieldCollider = transform.parent.Find("ShieldCollider").GetComponent<BoxCollider2D>();
        animator = GetComponentInParent<Animator>();
        gameData = GetComponentInParent<PlayerMovement>().gameData;
        gameData.Initialize();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            gameData.player.speed = gameData.player.originalSpeed * 0.5f;
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
