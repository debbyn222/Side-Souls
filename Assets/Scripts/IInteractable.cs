using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    
    public string InteractDescription { get; set; }//along with this, you must declare a public variable interactDescription when using this interface
    void Interact();
}
