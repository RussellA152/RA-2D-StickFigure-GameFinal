using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : EquipmentItem
{
    public float amountToHeal;

    public override void ItemAction(GameObject player)
    {
        //only use this item if it has charge
        //other just do nothing
        if (CheckEquipmentCharge())
        {
            Debug.Log("Heal Player for one hundred!");
        }
  
    }


    public override void CopyStats()
    {
        //downcast Equipment to Potion so we can copy new stats
        Potion item = (Potion) playerEquipmentInstance;

        item.amountOfCharge = amountOfCharge;
        item.chargeConsumedPerUse = chargeConsumedPerUse;
        item.amountToHeal = amountToHeal;


        //SetNewItemInstance(item);

        
        //Debug.Log("Potion new instance is " + equipmentItemScript);

        //TEMPORARY DESTROYS OLD EQUIPMENT ITEM INSTANCE
        //this.gameObject.SetActive(false);
    }
}
