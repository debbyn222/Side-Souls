/*Purpose:
Implements the IInteractable interface for NPCs, allowing interaction with player
*/
//Last Edit: 25th of June, 2024 @3:21am PST
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    //Interact is called when player interacts with the NPC
    public void Interact()
    {
        Debug.Log("Hello There"); //log message to console
    }
}
