using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : Interactable
{
    //[SerializeField] private NewItem itemToGiveToPlayer; //the item script that this giver will insert in the player's inventory

    [Header("Details of Item")]

    [SerializeField] private Sprite itemSprite;

    [SerializeField] private string scriptToLoad; //the name of the script to give to player (ex. Potion, Bomb)

    private bool spawned = false; //has this item spawned in the world?

    private bool retrieved = false; //was this item picked up by the player?

    private bool gaveItemToPlayer = false; //did this ItemGiver give the player a NewItem script component?

    
    //if so, the item won't be able to picked up again

    private void OnEnable()
    {
        spawned = true;
    }
    private void OnDisable()
    {
        //if the item wasn't picked up by the player, allow it to spawn again 
        if (!retrieved)
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
        Debug.Log("Picked up item!");

        //Set retrived to true so player cannot pick up the item more than once
        SetRetrieved(true);

        //add this new instance of the item to the player's item list
        //Instant items will not transfer to inventory
        AddItemToPlayer();
    }

    //Invoke AddComponent of the item's script to go to the player's inventory
    //need to AddComponent so the item is attached to the player, not the gameobject it initially spawned with
    public void AddItemToPlayer()
    {
        //only give item script to player if they its their first time interacting with this ItemGiver
        if (!gaveItemToPlayer)
        {
            //adds any item script this ItemGiver was specificed to add
            PlayerStats.instance.GetComponentHolder().AddComponent(Type.GetType(scriptToLoad));

            gaveItemToPlayer = true;
        }
        
    }


    public virtual void OnDrop()
    {

    }
}
