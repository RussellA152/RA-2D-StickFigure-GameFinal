using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipmentItem : Item
{
    [HideInInspector]
    public Type classType;

    [HideInInspector]
    public EquipmentItem equipmentItemScript;


    private bool uses; //how many times can this item be used?


    // Start is called before the first frame update
    void Start()
    {
        needsButtonPress = true;
        classType = this.GetType();
    }

    //public override void InteractableAction()
    //{
        //base.InteractableAction();
        //AddItem();
    //}

    //When the player swaps out this item for a different equipment item
    //void EquipmentSwap()
    //{

    //}

    public override Item AddItem()
    {
        //add this equipment item script (includes any deriving class of EquipmentItem to the player gameobject
        equipmentItemScript = (EquipmentItem) PlayerStats.instance.gameObject.AddComponent(classType);

        //adds the equipment item script to the player's equipment item inventory
        PlayerStats.instance.AddEquipmentItemToInventory(equipmentItemScript);

        return equipmentItemScript;

        //copies old instance's equipment item stats (instance on the item gameobject) to the new instance of the equipment item (the instance inside of the player)
        //CopyStats();
    }

}
