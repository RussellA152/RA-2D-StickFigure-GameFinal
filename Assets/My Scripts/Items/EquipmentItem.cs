using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentItem : Item
{

    // Start is called before the first frame update
    void Start()
    {
        needsButtonPress = true;
    }

    public override void InteractableAction()
    {
        base.InteractableAction();
        GoToInventory();
    }

    //When the player swaps out this item for a different equipment item
    //void EquipmentSwap()
    //{

    //}

    //when picked up.. go to the player's equipment item inventory
    void GoToInventory()
    {
        PlayerStats.instance.AddEquipmentItemToInventory(this);
    }
}
