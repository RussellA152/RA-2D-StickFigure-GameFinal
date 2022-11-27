using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dollar : Item
{
    //private int amountOfMoneyToGive;

    public override void ItemAction(GameObject player)
    {
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerMoney(myScriptableObject.amountOfMoneyGiven);
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        //amountOfMoneyToGive = myScriptableObject.amountOfMoneyGiven;

    }
}
