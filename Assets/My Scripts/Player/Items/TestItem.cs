using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItem : PassiveItem
{

    public override void ItemAction(GameObject player)
    {
        //increase the player's health by 100
        //PlayerStats.instance.GetHealthScript().ModifyMaxHealth(100);
        PlayerStats.instance.ModifyMaxHealth(100);
        Debug.Log("Modify player's max hp");
    }

    public override void ReverseAction()
    {
        PlayerStats.instance.ModifyMaxHealth(-100);
        //PlayerStats.instance.GetHealthScript().ModifyMaxHealth(-100);
    }
}
