using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class PassiveItem : OldItem
{
    /*
    //public bool activateOnPickUp; //will this item affect the player immediately? EX: Health boost, speed boost, attack boost?
    //but not something that would proc during gameplay

    //public string description;

    [HideInInspector]
    public Type classType;

    //the new passive item instance that will be added to the player's item inventory gameobject
    //is used for copying stats over from old item instance from the ground, to the new instance inside of the player
    //this is so that we don't have to hard code all values, we will be able to set variable values in inspector
    [HideInInspector]
    public PassiveItem passiveItemScript;

    public float procChance;

    private void Start()
    {
        //all passive items need a button to press to pick up
        needsButtonPress = true;
        //need to fetch the class type so we can add component to the player's passive item inventory using a variable
        classType = this.GetType();
    }

    //if this passive item will proc something while inside of the player's inventory..
    //subscribe this function to a corresponding event system
    public virtual void PassiveProcAbility()
    {
        
    }

    public override void AddItem()
    {
        //add this passive item script (includes any deriving class of PassiveItem to the player gameobject
        //need to convert from Component to PassiveItem so that deriving classes that downcast it to their respective class
        passiveItemScript = (PassiveItem) PlayerStats.instance.GetComponentHolder().AddComponent(classType);

        //adds the passive item script to the player's passive item inventory
        //PlayerStats.instance.AddPassiveItemToInventory(passiveItemScript);

        //copies old instance's passive item stats (instance on the item gameobject) to the new instance of the passive item (the instance inside of the player)
        CopyStats();
    }

    public bool ShouldProc()
    {
        //check if the item's proc chance was successful
        //if not, return and do not allow ability to activate
        if (Random.value < procChance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    */

}
