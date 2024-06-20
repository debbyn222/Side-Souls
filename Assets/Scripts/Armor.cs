using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IInteractable
{
    //modifiers are assigned in percentage values (e.g healthPointsModifier = 10 means 10% increase in healthpoints)
    public string armorName;
    public float speedModifier;
    public float rollSpeedModifier;
    public float climbSpeedModifier;
    public float jumpForceModifier;
    public float healthPointsModifier;
    public float maxStaminaModifier; 
    public Player player;

    public string interactDescription;
    public string InteractDescription
    {
        get => interactDescription;
        set => interactDescription = value;
    }


    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerMovement>().player;
    }

    public Armor(string armorName, float speedModifier, float rollSpeedModifier, float climbSpeedModifier, float jumpForceModifier, float healthPointsModifier, float maxStaminaModifier)
    {
        this.armorName = armorName;
        this.speedModifier = speedModifier;
        this.rollSpeedModifier = rollSpeedModifier;
        this.climbSpeedModifier = climbSpeedModifier;
        this.jumpForceModifier = jumpForceModifier;
        this.healthPointsModifier = healthPointsModifier;
        this.maxStaminaModifier = maxStaminaModifier;
    }

    public void Interact()
    {
        Debug.Log("picked up " +  this.armorName);
        player.EquipArmor(this);
        Destroy(gameObject);
    }


}
