using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : Item
{
    public override void ItemAction(GameObject player)
    {
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
            PlayItemSound(itemActionSound);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        itemActionSound = myScriptableObject.itemActionSound;

    }
}
