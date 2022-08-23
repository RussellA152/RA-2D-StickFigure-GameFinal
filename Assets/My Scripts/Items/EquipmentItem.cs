using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EquipmentItem : OldItem
{
    [HideInInspector]
    public Type classType;


    //the new equipment item instance that will be added to the player's item inventory gameobject
    //is used for copying stats over from old item instance from the ground, to the new instance inside of the player
    //this is so that we don't have to hard code all values, we will be able to set variable values in inspector
    [HideInInspector]
    public EquipmentItem playerEquipmentInstance;


    public int amountOfCharge; //how many times can this item be used?
    public int chargeConsumedPerUse; //how much charge will this item consume on each use?

    [HideInInspector]
    public bool hasSufficientCharge = true;

    public EquipmentItem originalDroppedInstance;

    private bool hasComponent; //does the player have this class as a component?


    // Start is called before the first frame update
    void Start()
    {
        hasComponent = false;
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
        //the new and old instance should become retrievable again because it will drop on the groun
        playerEquipmentInstance.SetRetrieved(false);
        originalDroppedInstance.SetRetrieved(false);

        //disable the player's equipment instance inside of the item inventory (its no longer the active equipment*)
        playerEquipmentInstance.enabled = false;

        //re-enable the original item gameobject
        originalDroppedInstance.gameObject.SetActive(true);

        //drop the original item in front of the player
        originalDroppedInstance.transform.position = new Vector3(PlayerStats.instance.transform.position.x, PlayerStats.instance.transform.position.y, PlayerStats.instance.transform.position.z);

    }

    public override void AddItem()
    {

        //set the originalDroppedInstance each item is picked up FOR THE FIRST TIME * 
        //this is to prevent the originalDroppedInstance from being the "Item Inventory" gameobject inside the Player
        originalDroppedInstance = this;

        if (!hasComponent)
        {
            playerEquipmentInstance = (EquipmentItem)PlayerStats.instance.GetComponentHolder().AddComponent(classType);
            hasComponent = true;
        }

        SetNewItemInstance(playerEquipmentInstance);
        //copies old instance's equipment item stats (instance on the item gameobject) to the new instance of the equipment item (the instance inside of the player)
        CopyStats();

        //if the new instance's itemscript is disabled, re-enable it
        if (!playerEquipmentInstance.enabled)
            playerEquipmentInstance.enabled = true;


        //add this equipment item script (includes any deriving class of EquipmentItem to the player gameobject
        //need to convert from Component to EquipmentItem so that deriving classes that downcast it to their respective class
        //adds the equipment item script to the player's equipment item inventory
        //PlayerStats.instance.AddEquipmentItemToInventory(playerEquipmentInstance);
    }

    //Need to set playerEquipmentInstance to the new added component instance
    //so that the reference to playerEquipmentInstance is not null when invoking EquipmentSwap()
    public void SetNewItemInstance(EquipmentItem item)
    {

        //the new script instance should remember the original gameobject that dropped it
        //this is so that if the player swaps out an equipment item, it will drop the original equipment item gameobject that it they initially picked up
        //we won't have to instaniate a new gameobject everytime
        item.originalDroppedInstance = originalDroppedInstance;

        //the new instance's equipItemScript is itself
        item.playerEquipmentInstance = item;

        //disable original item's gameobject (player has picked it up)
        this.gameObject.SetActive(false);
    }

}
