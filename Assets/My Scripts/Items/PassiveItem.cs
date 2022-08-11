using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem : Item
{
    public bool activateOnPickUp; //will this item affect the player immediately? EX: Health boost, speed boost, attack boost?
    //but not something that would proc during gameplay
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

}
