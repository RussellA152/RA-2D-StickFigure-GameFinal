using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautHelmet : Item
{
    public override void ItemAction(GameObject player)
    {
        // decrease player's gravity (floatier gameplay)
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerGravity(myScriptableObject.gravityModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
