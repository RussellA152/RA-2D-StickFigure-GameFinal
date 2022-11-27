using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Adrenaline : Item
{
    public override void ItemAction(GameObject player)
    {
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyRunningSpeed(myScriptableObject.movementSpeedMultiplierModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
