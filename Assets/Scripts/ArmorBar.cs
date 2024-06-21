using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmorBar : MonoBehaviour
{
    public Player player;
    public List<Image> armorSlots = new List<Image>(); // List to hold the armor slot images
    private Color filledColor = Color.white;
    private Color emptyColor = new Color(37f / 255f, 37f / 255f, 37f / 255f); //dark grayish
    //private Color emptyColor = Color.black;


    private void Start()
    {
        InitializeArmorSlots();

    }

    // Method to initialize the armorSlots list
    private void InitializeArmorSlots()
    {
        // Clear the list to avoid duplicates
        armorSlots.Clear();

        foreach (Transform child in transform)
        {
            Image armorSlot = child.GetComponent<Image>();
            if (armorSlot != null)
            {
                armorSlots.Add(armorSlot);
            }
        }

        Debug.Log("Number of armor slots: " + armorSlots.Count);
    }

    public void UpdateArmorBar(int armorLevel)
    {
        
        //clear previous armor level
        for (int i = 0; i < armorSlots.Count; i++)
        {
            armorSlots[i].color = emptyColor;
        }

        // Loop through the armor slots and update their visibility based on the armor level
        for (int i = 0; i < armorSlots.Count; i++)
        {
            if (i < armorLevel)
            {
                //armorSlots[i].enabled = true; 
                armorSlots[i].color = filledColor;// Show armor slot
            }
/*            else
            {
                //armorSlots[i].enabled = false; 
                armorSlots[i].color = emptyColor;// Hide armor slot
            }*/
        }
    }
    public int GetNumberOfArmorSlots()
    {
        return armorSlots.Count;
    }
}
