using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player: MonoBehaviour
{
    public string playerName;

    public float speed;
    public float rollSpeed;
    public float climbSpeed;
    public float jumpForce;
    public float healthPoints;
    public float maxStamina;

    private float originalSpeed;
    private float originalRollSpeed;
    private float originalClimbSpeed;
    private float originalJumpForce;
    private float originalHealthPoints;
    private float originalMaxStamina;

    private Armor equippedArmor;

    public Player() 
    {
        // default values
        playerName = "john";
        speed = 7f;
        rollSpeed = 6f;
        climbSpeed = 3f;
        jumpForce = 11f;
        healthPoints = 100f;
        maxStamina = 100f;

        originalSpeed = speed;
        originalRollSpeed = rollSpeed;
        originalClimbSpeed = climbSpeed;
        originalJumpForce = jumpForce;
        originalHealthPoints = healthPoints;
        originalMaxStamina = maxStamina;
}
     public Player(string name, float speed, float rollSpeed, float climbSpeed, float jumpForce, float healthPoints, float maxStamina)
    {
        this.playerName = name;
        this.speed = speed;
        this.rollSpeed = rollSpeed;
        this.climbSpeed = climbSpeed;
        this.jumpForce = jumpForce;
        this.healthPoints = healthPoints;
        this.maxStamina = maxStamina;

        originalSpeed = speed;
        originalRollSpeed = rollSpeed;
        originalClimbSpeed = climbSpeed;
        originalJumpForce = jumpForce;
        originalHealthPoints = healthPoints;
        originalMaxStamina = maxStamina;
    }

    public void EquipArmor(Armor armor)
    {

        if(equippedArmor != null)
        {
            //remove effects of currently equipped armor
            speed = originalSpeed;
            rollSpeed = originalRollSpeed;
            climbSpeed = originalClimbSpeed;
            jumpForce = originalJumpForce;
            healthPoints = originalHealthPoints;
            maxStamina = originalMaxStamina;
        }

        //equip new armor
        speed *= 1 + (armor.speedModifier / 100);
        rollSpeed *= 1 + (armor.rollSpeedModifier / 100);
        climbSpeed *= 1 + (armor.climbSpeedModifier / 100);
        jumpForce *= 1 + (armor.jumpForceModifier / 100);
        healthPoints *= 1 + (armor.healthPointsModifier / 100);
        maxStamina *= 1 + (armor.maxStaminaModifier / 100);

        equippedArmor = armor;
    }

}
