using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : Item
{
    public override void ItemAction(GameObject player)
    {
        // can only use potion if player has full health
        // and this item has enough charges
        if (!PlayerStats.instance.IsHealthFull() && ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        usageCooldown = myScriptableObject.usageCooldown;

        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;

        maxAmountOfCharge = myScriptableObject.maxAmountOfCharge;
        amountOfCharge = myScriptableObject.maxAmountOfCharge;

    }
}
