/* Purpose:
Implements the IInteractable interface for NPCs, allowing interaction with player
*/
//Last Edit: 25th of June, 2024 @3:21am PST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    public string interactDescription;
    public string InteractDescription
    {
        get => interactDescription;
        set => interactDescription = value;
    }
    public void Interact()
    {
        Debug.Log("Hello There");
    }
}