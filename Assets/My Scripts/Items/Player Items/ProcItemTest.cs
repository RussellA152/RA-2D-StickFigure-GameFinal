using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcItemTest : PassiveItem
{
    public float damage;

    public override void ItemAction(GameObject player)
    {
        player.GetComponent<CharacterController2D>().onJump += PassiveProcAbility;
    }

    public override void PassiveProcAbility()
    {
        if (ShouldProc())
        {
            Debug.Log("Jump proc!");
            Debug.Log(this.transform.gameObject.name);
        }   
        
    }

    public override void CopyStats()
    {
        ProcItemTest item = (ProcItemTest) passiveItemScript;

        item.procChance = procChance;
        item.damage = damage;
        //passiveItemScript.damage = damage;

        //TEMPORARY DESTROYS OLD PASSIVE ITEM INSTANCE
        Destroy(this.gameObject);
    }

}
