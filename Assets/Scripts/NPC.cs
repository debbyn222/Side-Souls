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
