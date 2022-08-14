using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

//The Item class will inherit from the Interactable class
//because items are "interactable," meaning the player should be able to
//walk up to them and pick them up
//when picked up, the items will transfer over the player's item inventory (with exception to Instant items)
public class Item : Interactable
{
    private bool spawned = false; //has this item spawned in the world?

    private bool retrieved = false; //was this item picked up by the player?
    //if so, the item won't be able to picked up agains

    private void OnEnable()
    {
        spawned = true;
    }
    private void OnDisable()
    {
        //if the item wasn't picked up by the player, allow it to spawn again 
        if(!retrieved)
            spawned = false;
    }
    private void OnDestroy()
    {
        //if the item wasn't picked up by the player, allow it to spawn again
        if (!retrieved)
            spawned = false;
    }

    private void Update()
    {
        //if player hasn't picked up the item, check if they are trying to pick it up
        if (!retrieved)
        {
            CheckInteraction();
            //Debug.Log("Check interaction!");

        }

    }

    public void SetRetrieved(bool boolean)
    {
        retrieved = boolean;
    }

    //what the item does when the player picks it up
    public override void InteractableAction()
    {
        //add this item to the player's item list
        //Instant items will not transfer to inventory
        //GoToPlayerInventory();

        SetRetrieved(true);

        Debug.Log("Retrieved item!");
    }

    public virtual void AddItem()
    {

    }

    public virtual void CopyStats()
    {

    }


    //what the item does for the player
    public virtual void ItemAction(GameObject player)
    {
        //Debug.Log("Item action!");
    }
    
    //reverse the effect this item had
    public virtual void ReverseAction()
    {

    }

    public virtual void OnDrop()
    {
        
    }
}