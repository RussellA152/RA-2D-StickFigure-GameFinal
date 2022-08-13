using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcItemTest : PassiveItem
{
    public override void ItemAction(GameObject player)
    {
        //base.ItemAction(player);

        player.GetComponent<CharacterController2D>().onJump += PassiveProcAbility;
    }

    public override void PassiveProcAbility()
    {
        if (ShouldProc())
        {
            Debug.Log("Jump proc!");
            Debug.Log(this.transform.gameObject.name);
            //Destroy(this.gameObject);
        }

        
        
    }
}
