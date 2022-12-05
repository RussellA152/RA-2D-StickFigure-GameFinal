using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikyWallet : Item
{
    // give player some money, but take away some of their current health
    public override void ItemAction(GameObject player)
    {
        // decrease player's gravity (floatier gameplay)
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerMoney(myScriptableObject.amountOfMoneyGiven);
            PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);

            PlayItemSound(itemActionSound);
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

        itemActionSound = myScriptableObject.itemActionSound;

    }
}
