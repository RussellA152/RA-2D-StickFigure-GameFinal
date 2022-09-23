using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaywallLock : ItemLocker
{
    private int priceOfItem;
    private bool fetchedPriceOfItem = false; // has this locker grabbed a reference to the item's price?

    public override void CheckIfConditionIsFullfilled()
    {
        // check if this locker hasn't grabbed a reference to the item's price already so we don't have to GetComponent more than once
        if (!fetchedPriceOfItem)
            CheckPriceOfItem();

        Debug.Log("Check if player has enough money");


        if (conditionFullfilled)
        {
            // if the player had sufficient funds for this item, then remove the lock
            itemToUnlock.RemoveLock();

            unlockedObject = true;
        }
        

        // start a small cooldown after interacting with lock
        StartCooldown();
    }

    private void CheckPriceOfItem()
    {
        //priceOfItem = objectToUnlock.gameObject.GetComponent<ItemScriptableObject>().itemPrice;

        fetchedPriceOfItem = true;

    }
}