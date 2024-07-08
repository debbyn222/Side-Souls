using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public bool UseItem()
    {
        if(statToChange == StatToChange.health)
        {
            Health pHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
            if(pHealth.health == pHealth.maxHealth)
            {
                return false;
            }
            else
            {
                pHealth.ManageHealth(amountToChangeStat);
                return true;
            }

        }
        return false;
    }

    public enum StatToChange
    {
        none,
        health,
     //   stamina,
    };
}
