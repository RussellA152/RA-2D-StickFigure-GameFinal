using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemTest : Item
{
    public override void ItemAction(GameObject player)
    {
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyMaxHealth(200f);
            Debug.Log("Used potion!");
        }
        
    }

    public override void InitializeValues()
    {
        Debug.Log("Fetch Stats!");

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        amountOfCharge = myScriptableObject.amountOfCharge;
        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;
    }
}
