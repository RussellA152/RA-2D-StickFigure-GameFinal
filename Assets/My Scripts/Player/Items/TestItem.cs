using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : ItemStats
{

    public override void ItemAction(GameObject player)
    {
        //increase the player's health by 100
        player.GetComponent<PlayerHealth>().ModifyMaxHealth(100);
    }
}
