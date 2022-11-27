using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBeer : Item
{
    public override void ItemAction(GameObject player)
    {
        // increase player's damage and damage absorption (do more damage, and take less damage)
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyDamageMultiplier(myScriptableObject.damageMultiplierModifier);
            PlayerStats.instance.ModifyDamageAbsorptionMultiplier(myScriptableObject.damageAbsorptionMultiplierModifier);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

    }
}
