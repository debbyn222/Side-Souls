/*Purpose:
Handle player attacks and manage attack states
*/
//Last Edit: 25th of June, 2024 @3:20am PST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject attackArea; //reference to game obj representing attack area

    private bool attacking = false;

    private float timeToAttack = 0.25f;
    private float timer = 0f;

    
    void Start()
    {
       
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K)) //check for attack input
        {
            Attack();
        }

        if(attacking) //manage attack duration
        {
            timer += Time.deltaTime;

            if(timer >= timeToAttack) //deactivates attack state if after specified time
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }

        }
    }

    private void Attack() //activate attack state and attack area
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }
}

/*Notes:
This script needs to be attached to parrying related scripts.
*/