using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipmentItem : Item
{
    [HideInInspector]
    public Type classType;


    //the new equipment item instance that will be added to the player's item inventory gameobject
    //is used for copying stats over from old item instance from the ground, to the new instance inside of the player
    //this is so that we don't have to hard code all values, we will be able to set variable values in inspector
    [HideInInspector]
    public EquipmentItem equipmentItemScript;


    public int amountOfCharge; //how many times can this item be used?
    public int chargeConsumedPerUse; //how much charge will this item consume on each use?

    [HideInInspector]
    public bool hasSufficientCharge = true;

    public bool inPlayerInventory;

    public EquipmentItem originalGameObject;

    private bool haveComponent;


    // Start is called before the first frame update
    void Start()
    {
        haveComponent = false;
        inPlayerInventory = false;
        needsButtonPress = true;
        classType = this.GetType();
    }

    public bool CheckEquipmentCharge()
    {
        //if this item has sufficient charge, take some away
        //otherwise, return and don't allow item to activate its use
        if (amountOfCharge < chargeConsumedPerUse)
        {
            //when player's equipment item is out of charges we can probably play some sound that
            //indicates the item can't be used
            Debug.Log("Insufficient Charge");
            hasSufficientCharge = false;
            return false;
        }

        else
        {
            amountOfCharge -= chargeConsumedPerUse;
            hasSufficientCharge = true;

            return true;
        }
    }

    //When the player swaps out this item for a different equipment item
    public void EquipmentSwap()
    {
        //inPlayerInventory = false;
        Debug.Log("new instance is " + equipmentItemScript);

        equipmentItemScript.inPlayerInventory = false;
        equipmentItemScript.originalGameObject.inPlayerInventory = false;

        equipmentItemScript.SetRetrieved(false);

        originalGameObject.SetRetrieved(false);

        equipmentItemScript.enabled = false;

        equipmentItemScript.originalGameObject.gameObject.SetActive(true);

        equipmentItemScript.originalGameObject.transform.position = new Vector3(PlayerStats.instance.transform.position.x, PlayerStats.instance.transform.position.y, PlayerStats.instance.transform.position.z);

    }

    public override void AddItem()
    {
        //Debug.Log("in player inventory = " + inPlayerInventory + "      " + this.name);
        if (!inPlayerInventory)
        {
            //set the originalGameObject each item is picked up FOR THE FIRST TIME * 
            //this is to prevent the originalGameObject from being the "Item Inventory" gameobject inside the Player
            originalGameObject = this;
            if (!haveComponent)
            {
                equipmentItemScript = (EquipmentItem)PlayerStats.instance.GetComponentHolder().AddComponent(classType);
                haveComponent = true;
            }
                

            //copies old instance's equipment item stats (instance on the item gameobject) to the new instance of the equipment item (the instance inside of the player)
            CopyStats();
        }


        if (!equipmentItemScript.enabled)
            equipmentItemScript.enabled = true;
        //add this equipment item script (includes any deriving class of EquipmentItem to the player gameobject
        //need to convert from Component to EquipmentItem so that deriving classes that downcast it to their respective class
        //adds the equipment item script to the player's equipment item inventory
        PlayerStats.instance.AddEquipmentItemToInventory(equipmentItemScript);

        //copies old instance's equipment item stats (instance on the item gameobject) to the new instance of the equipment item (the instance inside of the player)
        //CopyStats();

        inPlayerInventory = true;

        

        //Debug.Log("in player inventory = " + inPlayerInventory + "      " + this.name);
    }

    //Need to set equipmentItemScript to the new added component instance
    //so that the reference to equipmentItemScript is not null when invoking EquipmentSwap()
    public void SetNewItemInstance(EquipmentItem item)
    {
        //this equipmentItemScript is now the new instance downcased from the derived class
        equipmentItemScript = item;
        //the new script instance should remember the original gameobject that dropped it
        //this is so that if the player swaps out an equipment item, it will drop the original equipment item gameobject that it they initially picked up
        //we won't have to instaniate a new gameobject everytime
        item.originalGameObject = originalGameObject;
        //the new instance's equipItemScript is itself
        item.equipmentItemScript = equipmentItemScript;

        //disable original item's gameobject (player has picked it up)
        this.gameObject.SetActive(false);
    }

}
