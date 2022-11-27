using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrassKnuckles : Item
{
    public override void ItemAction(GameObject player)
    {
        // increase base damage
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyDamageMultiplier(myScriptableObject.damageMultiplierModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
