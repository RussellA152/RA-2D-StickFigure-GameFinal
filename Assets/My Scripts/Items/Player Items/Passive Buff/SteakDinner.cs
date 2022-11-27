using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteakDinner : Item
{
    public override void ItemAction(GameObject player)
    {
        // increase player's max health
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyMaxHealth(myScriptableObject.maxHealthModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
