using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePotion : Item
{
    public override void ItemAction(GameObject player)
    {
        // this potion will increase player's damage
        // but will lower their current health
        if(ShouldActivate())
        {
            PlayerStats.instance.ModifyDamageMultiplier(myScriptableObject.damageMultiplierModifier);
            PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);

            PlayItemSound(itemActionSound);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;

        usageCooldown = myScriptableObject.usageCooldown;

        maxAmountOfCharge = myScriptableObject.maxAmountOfCharge;
        amountOfCharge = myScriptableObject.maxAmountOfCharge;

        itemActionSound = myScriptableObject.itemActionSound;

    }
}
