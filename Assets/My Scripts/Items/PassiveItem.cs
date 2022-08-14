using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System;

public class PassiveItem : Item
{
    //public bool activateOnPickUp; //will this item affect the player immediately? EX: Health boost, speed boost, attack boost?
    //but not something that would proc during gameplay

    //public string description;

    //public string className;

    [HideInInspector]
    public Type classType;

    [HideInInspector]
    public PassiveItem passiveItemScript;

    public float procChance;


    private void Start()
    {
        needsButtonPress = true;
        classType = this.GetType();
    }

    public override void InteractableAction()
    {
        base.InteractableAction();
        GoToInventory();
    }

    //when picked up.. go to the player's passive item inventory
    void GoToInventory()
    {
        AddItem();
    }

    public virtual void PassiveProcAbility()
    {
        
    }

    public override void AddItem()
    {
        //add this passive item script (includes any deriving class of PassiveItem to the player gameobject
        passiveItemScript = (PassiveItem) PlayerStats.instance.gameObject.AddComponent(classType);

        //adds the passive item script to the player's passive item inventory
        PlayerStats.instance.AddPassiveItemToInventory(passiveItemScript);

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

}
