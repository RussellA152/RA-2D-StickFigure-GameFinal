using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLock : ItemLocker
{
    public override void CheckIfConditionIsFullfilled()
    {
        // might need BossRoom to tell this lock that the boss was killed?
        // or have a boss was killed event system to unlock this item (probably this)
        Debug.Log("Check if boss was killed");

        if (conditionFullfilled)
        {
            // if the player had sufficient funds for this item, then remove the lock
            objectToUnlock.RemoveLock();

            unlockedObject = true;
        }


        // start a small cooldown after interacting with lock
        StartCooldown();
    }

}
