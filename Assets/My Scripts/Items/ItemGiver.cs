using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : Interactable
{
    //[SerializeField] private NewItem itemToGiveToPlayer; //the item script that this giver will insert in the player's inventory
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D actualCollider; //box collider without isTrigger checked
    [SerializeField] private BoxCollider2D triggerCollider; //box collider with isTrigger checked

    private Transform parent; //the parent gameobject of this item giver

    [SerializeField] private bool displayRotation;

    [Header("Details of Item")]

    [SerializeField] private Item itemToGive;

    //[SerializeField] private Sprite itemSprite;

    //private bool spawned = false; //has this item spawned in the world?

    private bool retrieved = false; //was this item picked up by the player?

    private float dropForceX = 1750f; //how much force is applied to this ItemGiver when it is dropped onto the ground (x direction)
    private float dropForceY = 0f; //how much force is applied to this ItemGiver when it is dropped onto the ground (y direction)

    private float dropOffsetX = 1f; //the offset applied to this ItemGiver when dropped (x direction)
    private float dropOffsetY = 0.1f; //the offset applied to this ItemGiver when dropped (y direction)

    private void Start()
    {
        //Always ignore collision between item and player
        Physics2D.IgnoreLayerCollision(7,11);

    }

    private void OnEnable()
    {
        base.OnEnable();

        //spawned = true;
    }
    private void OnDisable()
    {
        base.OnDisable();

        //if the item wasn't picked up by the player, allow it to spawn again 
        //if (!retrieved)
            //spawned = false;
    }
    private void OnDestroy()
    {
        //if the item wasn't picked up by the player, allow it to spawn again
        //if (!retrieved)
            //spawned = false;
    }

    private void Update()
    {
        //if player hasn't picked up the item, check if they are trying to pick it up
        if (!retrieved && inTrigger)
        {
            CheckInteraction();
        }

    }

    //set "retrieved" to either true or false
    private void SetRetrieved(bool boolean)
    {
        retrieved = boolean;
    }

    //what the item does when the player picks it up
    public override void InteractableAction()
    {
        Debug.Log("Picked up item!");

        //start interaction cooldown after interacting with this ItemGiver
        StartCooldown();

        //add this new instance of the item to the player's item list
        AddItemToPlayer();

        
    }

    //Invoke AddComponent of the item's script to go to the player's inventory
    //need to AddComponent so the item is attached to the player, not the gameobject it initially spawned with
    private void AddItemToPlayer()
    {
        if(!retrieved){

            //if the player already has an equipment item, then invoke the swapEquipment event system
            if(ItemManager.instance.activeEquipmentSlot != null && itemToGive.type == ItemScriptableObject.ItemType.equipment)
            {
                //call the event system so that the previous equipment item's itemgiver will call its OnDrop
                //this allows us to not need to know about other itemgivers
                ItemManager.instance.SwapActiveEquipment();

                //now this item giver will subscribe to event system
                ItemManager.instance.swapEquipmentEvent += OnDrop;
            }
            //otherwise, subscribe this OnDrop to the swapEquipment event system
            else if(ItemManager.instance.activeEquipmentSlot == null && itemToGive.type == ItemScriptableObject.ItemType.equipment)
            {
                ItemManager.instance.swapEquipmentEvent += OnDrop;
            }

            //set the parent of this ItemGiver to ItemManager's item holder gameobject
            parent = ItemManager.instance.GetItemHolder().transform;

            this.transform.SetParent(parent);

            //enable the Item script belonging to this ItemGiver
            if(!itemToGive.enabled)
                itemToGive.enabled = true;

            //disable the sprite renderer of this ItemGiver
            spriteRenderer.enabled = false;
            //disable all colliders belonging to this ItemGiver
            actualCollider.enabled = false;
            triggerCollider.enabled = false;

            //Set retrived to true so player cannot pick up the item more than once
            SetRetrieved(true);


        }
    }


    public void OnDrop()
    {
        //reset the velocity of this ItemGiver when dropped so it does not drop too far away from player
        //without, spamming pick up will send ItemGiver too far away
        rb.velocity = Vector2.zero;

        Debug.Log("Dropping item " + gameObject.name);
        //disable the Item script belonging to this ItemGiver
        if (itemToGive.enabled)
            itemToGive.enabled = false;   

        //set the parent of this itemgiver to null
        parent = null;
        this.transform.SetParent(null);


        //re-enable the sprite of this item
        spriteRenderer.enabled = true;

        //re-enable the colliders of this item
        actualCollider.enabled = true;
        triggerCollider.enabled = true;

        Transform interacter = GetInteracter();

        //if the interacter (player) is facing right, then this item will have force applied in the right direction
        if (interacter.localScale.x == 1)
        {
            //drop this item infront of the player (the interacter is always the player)
            transform.localPosition = new Vector2(interacter.position.x + dropOffsetX, interacter.position.y + dropOffsetY);
            rb.AddForce(new Vector2(dropForceX, dropForceY));
        }
        //if the interacter (player) is facing left, then this item will have force applied in the left direction    
        else if (interacter.localScale.x == -1)
        {
            //drop this item infront of the player (the interacter is always the player)
            transform.localPosition = new Vector2(interacter.position.x - dropOffsetX, interacter.position.y + dropOffsetY);
            rb.AddForce(new Vector2(-dropForceX, dropForceY));
        }
            

        //allow this item to be picked up again
        SetRetrieved(false);

        //unsubscribe from SwapEquipment event after being dropped
        ItemManager.instance.swapEquipmentEvent -= OnDrop;

    }
}
