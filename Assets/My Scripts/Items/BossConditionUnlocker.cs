using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player defeat the boss in the room to unlock this locked object (usually an Item)
public class BossConditionUnlocker : Unlocker
{
    private bool bossKilled = false;

    private void Start()
    {
        // when the boss is killed, every BossConditionUnlocker will set "bossKilled" to true
        // then the player will be able to interact with this unlocker
        EnemyManager.enemyManagerInstance.onBossKill += SetBossKilledTrue;
    }

    public override void CheckIfConditionIsFulfilled()
    {
        // might need BossRoom to tell this lock that the boss was killed?
        // or have a boss was killed event system to unlock this item (probably this)
        if (bossKilled)
        {
            UnlockItem();
        }
        else
        {
            // Play an unsucessful interaction sound
            Debug.Log("Boss not yet killed!");
        }


        // start a small cooldown after interacting with lock
        StartCooldown();
    }

    private void SetBossKilledTrue()
    {
        bossKilled = true;
    }

}
