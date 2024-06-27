using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor : MonoBehaviour, IInteractable
{
    //modifiers are assigned in percentage values (e.g healthPointsModifier = 10 means 10% increase in healthpoints)
    public string armorName;
    public int armorLevel; //each level of armor = 20 HP (change as neeeded)
    public float speedModifier;
    public float rollSpeedModifier;
    public float climbSpeedModifier;
    public float jumpForceModifier;
    private float healthPointsModifier;
    public float maxStaminaModifier;
    private int numberOfArmorSlots;
    private ArmorBar armorBar;
    public Player player;

    public string interactDescription;
    public string InteractDescription
    {
        get => interactDescription;
        set => interactDescription = value;
    }


    void Start()
    {
        armorBar = FindObjectOfType<ArmorBar>();
        // Check if ArmorBarController was found
        if (armorBar != null)
        {
            // Get the number of armor slots
            numberOfArmorSlots = armorBar.GetNumberOfArmorSlots();
        }
        else
        {
            Debug.LogError("ArmorBar not found in the scene.");
        }
    }
    public Armor(string armorName, float speedModifier, float rollSpeedModifier, float climbSpeedModifier, float jumpForceModifier, float maxStaminaModifier, int armorLevel)
    {
        this.armorName = armorName;
        this.speedModifier = speedModifier;
        this.rollSpeedModifier = rollSpeedModifier;
        this.climbSpeedModifier = climbSpeedModifier;
        this.jumpForceModifier = jumpForceModifier;
        //this.healthPointsModifier = healthPointsModifier;
        this.maxStaminaModifier = maxStaminaModifier;
        this.armorLevel = armorLevel <= 5 ? armorLevel : 5;
    }

    public void Interact()
    {
        Debug.Log("picked up " +  this.armorName);
        player.EquipArmor(this);
        armorBar.UpdateArmorBar(player.equippedArmor != null ? player.equippedArmor.armorLevel : 0);
        player.printStats();
        //Destroy(gameObject);
    }


}
