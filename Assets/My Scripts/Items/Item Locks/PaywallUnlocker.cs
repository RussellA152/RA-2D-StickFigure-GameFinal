using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player must pay with their money to unlock the locked object (usually an Item)
public class PaywallUnlocker : Unlocker
{
    [SerializeField] private int priceOfObject;
    private bool fetchedPriceOfItem = false; // has this locker grabbed a reference to the item's price?

    public override void CheckIfConditionIsFulfilled()
    {
        // TEMPORARY (REMOVE LATER) *** 
        //conditionFulfilled = true;

        // check if this locker hasn't grabbed a reference to the item's price already so we don't have to GetComponent more than once
        if (!fetchedPriceOfItem)
            CheckPriceOfItem();

        // if the player has enough money to unlock this gameobject
        if (PlayerStats.instance.GetPlayerMoney() >= priceOfObject)
        {
            // tell the locked gameobject to remove its lock
            UnlockItem();

            // subtract the player's money by priceOfObject amount
            PlayerStats.instance.ModifyPlayerMoney(-priceOfObject);
        }
        
        else
        {
            // Play a failure/unsuccessful purchase sound effect

            Debug.Log("Not enough money");
        }
        

        // start a small cooldown after interacting with lock
        StartCooldown();
    }

    private void CheckPriceOfItem()
    {
        // fetch the price of this locked gameobject
        priceOfObject = gameObjectToUnlock.GetComponent<IPriceable>().GetPrice();

        fetchedPriceOfItem = true;

    }
}