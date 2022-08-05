using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The Item class will inherit from the Interactable class
//because items are "interactable," meaning the player should be able to
//walk up to them and pick them up
//when picked up, the items will transfer over the player's item inventory (with exception to Instant items)
public class Item : Interactable
{
    private bool spawned = false; //has this item spawned in the world?

    private bool retrieved = false; //was this picked up by the player?

    public bool goesToInventory; //will this item transfer to the player's inventory?


    //SerializeField] private bool retrieved = false; //was this item picked up by the player?
    //if so, the item won't be able to picked up agains

    //[SerializeField] private bool activateOnPickup; //should this item do its action the moment the player picks it up?
    //used for simple items like health or speed boost

    //[SerializeField] private bool hasPassiveAbility; //does this item have a passive action?
    //will this item activate continously throughout playthrough?

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

        }

    }

    //what the item does when the player picks it up
    public override void InteractableAction()
    {
        //Debug.Log("Go to the player's item inventory!");

        //add this item to the player's item list
        //Instant items would not transfer to inventory
        if(goesToInventory)
            interacter.gameObject.GetComponent<PlayerItems>().AddItemToInventory(this);

        retrieved = true;
    }

    //what the item does for the player
    public virtual void ItemAction(GameObject player)
    {

    }

    public virtual void OnDrop()
    {

    }
}