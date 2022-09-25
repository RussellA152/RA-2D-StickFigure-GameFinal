using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player must pay with their money to unlock the locked object (usually an Item)
public class PaywallUnlocker : Unlocker
{
    [SerializeField] private int priceOfItem;
    private bool fetchedPriceOfItem = false; // has this locker grabbed a reference to the item's price?

    public override void CheckIfConditionIsFullfilled()
    {
        // TEMPORARY (REMOVE LATER) *** 
        conditionFullfilled = true;

        // check if this locker hasn't grabbed a reference to the item's price already so we don't have to GetComponent more than once
        if (!fetchedPriceOfItem)
            CheckPriceOfItem();

        if (!conditionFullfilled)
        {
            Debug.Log("Check if player has enough money");
        }
        
        else
        {
            // if the player had sufficient funds for this item, then remove the lock
            //itemToUnlock.RemoveLock();
            UnlockItem();
        }
        

        // start a small cooldown after interacting with lock
        StartCooldown();
    }

    private void CheckPriceOfItem()
    {
        // fetch the price of this locked gameobject
        priceOfItem = gameObjectToUnlock.GetComponent<IPriceable>().GetPrice();

        fetchedPriceOfItem = true;

    }
}