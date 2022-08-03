using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The ItemStats class will inherit from the Interactable class
//because items are "interactable," meaning the player should be able to
//walk up to them and pick them up
//when picked up, the items will transfer over the player's item inventory
public class ItemStats: Interactable
{
    [SerializeField] private bool retrieved = false; //was this item picked up by the player?
                                                     //if so, the item won't be able to picked up agains

    //[SerializeField] private bool activateOnPickup; //should this item do its action the moment the player picks it up?
                                                    //used for simple items like health or speed boost

    [SerializeField] private bool hasPassiveAbility; //does this item have a passive action?
                                                     //will this item activate continously throughout playthrough?

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
        Debug.Log("Go to the player's item inventory!");

        //add this item to the player's item list
        //interacter.gameObject.GetComponent<PlayerItems>().AddItemToInventory(this);

        retrieved = true;
    }

    //what the item does for the player
    public virtual void ItemAction(GameObject player)
    {

    }
}
