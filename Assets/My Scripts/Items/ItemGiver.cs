using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ItemGiver is the physical, interactable part of an Item
// This is what gives an Item the logic of being picked up and dropped
public class ItemGiver : Interactable, ILockable
{
    //[SerializeField] private NewItem itemToGiveToPlayer; //the item script that this giver will insert in the player's inventory
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D actualCollider; //box collider without isTrigger checked
    [SerializeField] private BoxCollider2D triggerCollider; //box collider with isTrigger checked

    private Transform parent; //the parent gameobject of this item giver

    //[SerializeField] private bool displayRotation;

    [Header("Details of Item")]

    [SerializeField] private Item itemToGive;

    [SerializeField] private bool kinematicOnEnable; // should this item be kinematic when enabled?


    //[SerializeField] private Vector2 positionInRoom; // the position where this ItemGiver is in the room

    [SerializeField] private bool retrieved = false; //was this item picked up by the player?

    private float dropForceX = 1750f; //how much force is applied to this ItemGiver when it is dropped onto the ground (x direction)
    private float dropForceY = 0f; //how much force is applied to this ItemGiver when it is dropped onto the ground (y direction)

    private float dropOffsetX = 1f; //the offset applied to this ItemGiver when dropped (x direction)
    private float dropOffsetY = 0.1f; //the offset applied to this ItemGiver when dropped (y direction)

    private void Start()
    {
        //Always ignore collision between item and player
        Physics2D.IgnoreLayerCollision(7,11);

        //LevelManager.instance.onPlayerEnterNewArea += ResetPositionInRoom;

    }

    private void OnEnable()
    {
        base.OnEnable();

        // allow ItemGiver to be interactable and picked up again if enabled again (this is for object pooling purposes)
        SetRetrieved(false);
        SetCanInteract(true);

        if (kinematicOnEnable)
            // set isKinematic to true onEnable so that item will stay on top of a display (not effected by physics until dropped onto ground)
            rb.isKinematic = true;
        else
            rb.isKinematic = false;

        // disable itemToGive so that it will be enabled when picked up (this is for object pooling purposes)
        itemToGive.enabled = false;

        // enable spriteRenderer when ItemGiver is enabled (this is for object pooling purposes)
        spriteRenderer.enabled = true;

        // make sure colliders are enabled when ItemGiver is enabled  (this is for object pooling purposes)
        actualCollider.enabled = true;
        triggerCollider.enabled = true;

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
        //Debug.Log("Picked up item!");

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

            //when picked up, set isKinematic to true to prevent item from moving 
            rb.isKinematic = true;

            SwapOutOldEquipmentItem();
            
            // only set parent of itemToGive to ItemHolder if the item is NOT of type Instant
            // this is because instant items are disabled and return to pool when picked up 
            if(itemToGive.type != ItemScriptableObject.ItemType.instant)
            {
                //set the parent of this ItemGiver to ItemManager's item holder gameobject
                parent = ItemSwapper.instance.GetItemHolder().transform;

                this.transform.SetParent(parent);
            }
            

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

            // item was picked up, call "OnItemPickup"
            itemToGive.OnItemPickup();


        }
    }

    // This function checks if the player is currently trying to pick up an item of type: equipment
    //      If the item is of type: equipment, check if they already have an equipment item slotted
    //          if they do not, subscribe OnDrop() to the swapEquipmentEvent
    //          if they did have an equipment item slotted, then invoke the swapEquipmentEvent which will force the previous equipment item to call OnDrop(), then make subscribe THIS OnDrop to the same event system
    private void SwapOutOldEquipmentItem()
    {
        //if the player already has an equipment item, then invoke the swapEquipment event system
        if (ItemSwapper.instance.activeEquipmentSlot != null && itemToGive.type == ItemScriptableObject.ItemType.equipment)
        {
            //call the event system so that the previous equipment item's itemgiver will call its OnDrop
            //this allows us to not need to know about other itemgivers
            ItemSwapper.instance.SwapActiveEquipment();

            //now this item giver will subscribe to event system
            ItemSwapper.instance.swapEquipmentEvent += OnDrop;
        }
        //otherwise, subscribe this OnDrop to the swapEquipment event system
        else if (ItemSwapper.instance.activeEquipmentSlot == null && itemToGive.type == ItemScriptableObject.ItemType.equipment)
        {
            ItemSwapper.instance.swapEquipmentEvent += OnDrop;
        }
    }

    // drops the item to the ground
    public void OnDrop()
    {
        //when dropped, set isKinematic to false to allow item to fall to the ground
        rb.isKinematic = false;

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
        ItemSwapper.instance.swapEquipmentEvent -= OnDrop;

    }

    public void AddLock()
    {
        // don't allow player to interact/pick up this itemgiver's item
        SetCanInteract(false);

        // when locked, don't let player collide with trigger until unlocked
        triggerCollider.enabled = false;
    }

    public void RemoveLock()
    {
        // allow player to interact/pick up this itemgiver's item
        SetCanInteract(true);

        // when unlocked, allow player to collide with trigger
        triggerCollider.enabled = true;
    }

    // when the player walks into a new room, return to the initial position this ItemGiver was enabled at
    // this prevents player from bringing items into another room
    /*
    public void SetPositionInRoom(Vector2 position)
    {
        positionInRoom = position;
    }

    private void ResetPositionInRoom()
    {
        this.transform.position = positionInRoom;
    }
    */
}
