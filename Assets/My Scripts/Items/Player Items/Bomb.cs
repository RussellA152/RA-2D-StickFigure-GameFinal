using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EquipmentItem
{
    public float damage;

    public override void ItemAction(GameObject player)
    {
        //only use this item if it has charge
        //other just do nothing
        if (CheckEquipmentCharge())
        {
            Debug.Log("Throw a bomb!");
        }

    }


    public override void CopyStats()
    {
        //downcast Equipment to Potion so we can copy new stats
        Bomb item = (Bomb)equipmentItemScript;


        item.amountOfCharge = amountOfCharge;
        item.chargeConsumedPerUse = chargeConsumedPerUse;
        item.damage = damage;

        SetNewItemInstance(item);

        

        //Debug.Log("Bomb new instance is " + equipmentItemScript);

        //TEMPORARY DESTROYS OLD EQUIPMENT ITEM INSTANCE
       // this.gameObject.SetActive(false);
    }
}
