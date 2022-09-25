using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player defeat the boss in the room to unlock this locked object (usually an Item)
public class BossConditionUnlocker : Unlocker
{
    public override void CheckIfConditionIsFullfilled()
    {
        // TEMPORARY (REMOVE LATER) *** 
        conditionFullfilled = true;

        // might need BossRoom to tell this lock that the boss was killed?
        // or have a boss was killed event system to unlock this item (probably this)
        if (!conditionFullfilled)
        {
            Debug.Log("Check if boss was killed");

        }
        else
        {
            UnlockItem();
        }


        // start a small cooldown after interacting with lock
        StartCooldown();
    }

}
