/*Purpose:
Defines an interface for interactable objects
*/
//Last Edited: 25th of June, 2024 @1:42pm
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    
    public string InteractDescription { get; set; }//along with this, you must declare a public variable interactDescription when using this interface
    void Interact();
}