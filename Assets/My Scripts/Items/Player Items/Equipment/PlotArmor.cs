using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotArmor : Item
{
    public override void ItemAction(GameObject player)
    {
        // Turn off player's hitbox for a few seconds
        if (ShouldActivate())
        {
            //PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
            PlayerStats.instance.TurnOffHurt(myScriptableObject.itemDuration);
            Debug.Log("Hello? Plot armor?");
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        usageCooldown = myScriptableObject.usageCooldown;

    }
}
