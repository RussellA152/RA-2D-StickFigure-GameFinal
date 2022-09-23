using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopLock : Interactable
{
    private ILockable objectToUnlock;

    public override void InteractableAction()
    {
        CheckSufficientFunds();
    }
    private void CheckSufficientFunds()
    {
        Debug.Log("Check if player has enough money");
        StartCooldown();
    }

    //
    public void SetLockedObject(ILockable lockedObject)
    {
        objectToUnlock = lockedObject;
    }
}
