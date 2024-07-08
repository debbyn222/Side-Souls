using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFloor : MonoBehaviour
{
    public int damagePerSecond = 50; // Amount of damage per second
    private bool isPlayerOnFloor = false;
    private GameObject player;
    private float damageTimer = 0f;

    void Update()
    {
        if (isPlayerOnFloor)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= 1f)
            {
                Health playerHealth = player.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.Damage(damagePerSecond);
                    damageTimer = 0f;
                }
                else
                {
                    Debug.LogError("Player GameObject does not have a Health script attached!");
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnFloor = true;
            player = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnFloor = false;
            player = null;
            damageTimer = 0f;
        }
    }
}