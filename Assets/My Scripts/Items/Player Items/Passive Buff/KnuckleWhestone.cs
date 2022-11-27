using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnuckleWhestone : Item
{
    public override void ItemAction(GameObject player)
    {
        // increase amount of force applied to enemies from player attacks
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyAttackPowerMultiplier(myScriptableObject.attackPowerMultiplierModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
