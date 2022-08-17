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

    [SerializeField] private bool retrieved = false; //was this item picked up by the player?
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
        
        //Set retrived to true so player cannot pick up the item more than once
        SetRetrieved(true);

        //add this new instance of the item to the player's item list
        //Instant items will not transfer to inventory
        AddItem();
    }

    //Invoke AddComponent of the item's script to go to the player's inventory
    //need to AddComponent so the item is attached to the player, not the gameobject it initially spawned with
    public virtual void AddItem()
    {
        
    }

    //copies old instance's item stats (instance on the item gameobject) to the new instance of the item (the instance inside of the player)
    public virtual void CopyStats()
    {

    }


    //what the item does for the player
    //passes in player to access components if needed..
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