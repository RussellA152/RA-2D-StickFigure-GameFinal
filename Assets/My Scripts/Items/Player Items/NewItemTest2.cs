using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemTest2 : Item//, IDamageAttributes
{
    public override void ItemAction(GameObject player)
    {
        if (ShouldActivate())
        {
            Debug.Log("Invoke NewItemTest's ItemAction!");
        }
        
    }

    public override void InitializeValues()
    {
        Debug.Log("Fetch Stats!");

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        itemPrice = myScriptableObject.itemPrice;

        amountOfCharge = myScriptableObject.amountOfCharge;
        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;


        //procChance = myScriptableObject.procChance;

        //will fetch from persistant data source but just testing to see if stats would even carry over
        //type = ItemScriptableObject.ItemType.equipment;

        //procChance = 50f;
    }
}
