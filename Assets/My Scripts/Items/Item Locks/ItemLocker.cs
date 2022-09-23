using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemLocker : Interactable
{
    [HideInInspector]
    public ItemGiver itemToUnlock; // the ILockable object that this PaywallLock will lock

    //private int itemLayerInt; // an integer representing the NameToLayer of "Item" layer

    //private bool hasLocked = false; // has this PaywallLock intance already locked something? If so, don't lock again

    [HideInInspector]
    public bool unlockedObject = false; // has this PaywallLock already unlocked its given gameObject?

    [HideInInspector]
    public bool conditionFullfilled; // has the Player fullfilled this locker's needs to unlock the gameobject

    //private bool hasSufficientFunds = false; // testing to see if player can grab this item if they didnt have enough money

    private void Update()
    {
        // if this lock hasn't unlocked its given gameObject yet, and the player is in the trigger
        // then check for interaction
        if (!unlockedObject && inTrigger)
            CheckInteraction();
    }

    public override void InteractableAction()
    {
        CheckIfConditionIsFullfilled();
    }

    public abstract void CheckIfConditionIsFullfilled();

    public void LockItems()
    {
        Debug.Log("Lock items!");

        // grab a ItemGiver reference from this gameobject's children
        // an Item will always be a child of this lock since the ItemManager sets it
        itemToUnlock = gameObject.transform.GetComponentInChildren<ItemGiver>();

        itemToUnlock.AddLock();


    }
}
