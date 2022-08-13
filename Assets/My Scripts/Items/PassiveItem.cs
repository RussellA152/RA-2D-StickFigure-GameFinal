using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : Item
{
    //public bool activateOnPickUp; //will this item affect the player immediately? EX: Health boost, speed boost, attack boost?
    //but not something that would proc during gameplay

    [Range(0.0f, 1.0f)]
    public float procChance;


    private void Start()
    {
        needsButtonPress = true;
    }

    public override void InteractableAction()
    {
        base.InteractableAction();
        GoToInventory();
    }

    //when picked up.. go to the player's passive item inventory
    void GoToInventory()
    {
        PlayerStats.instance.AddPassiveItemToInventory(this);
    }

    public virtual void PassiveProcAbility()
    {
        
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
