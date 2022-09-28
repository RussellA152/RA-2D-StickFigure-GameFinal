using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This abstract class is used to unlock a locked Interactable gameobject (usually an Item)
public abstract class Unlocker : Interactable
{

    [HideInInspector]
    public GameObject gameObjectToUnlock;

    private ILockable lockable;

    [HideInInspector]
    private bool unlockedObject = false; // has this Unlocker already unlocked its given gameObject?

    //[HideInInspector]
    //public bool conditionFulfilled; // has the Player fullfilled this unlocker's needs to unlock the gameobject

    private void Update()
    {
        // if this unlocker hasn't unlocked its given gameObject yet, and the player is in the trigger
        // then check for interaction
        if (!unlockedObject && inTrigger)
            CheckInteraction();
    }

    public override void InteractableAction()
    {
        CheckIfConditionIsFulfilled();
    }

    public abstract void CheckIfConditionIsFulfilled();


    // set "lockedItem" equal to item passed through
    public void SetGameObjectToUnlock(GameObject lockedGameObject)
    {
        gameObjectToUnlock = lockedGameObject;
        lockable = gameObjectToUnlock.GetComponent<ILockable>();

    }

    public void UnlockItem()
    {
        lockable.RemoveLock();

        Debug.Log("Unlocked gameobject");

        unlockedObject = true;
    }
}
