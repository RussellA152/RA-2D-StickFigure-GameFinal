using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : Interactable
{
    //[SerializeField] private NewItem itemToGiveToPlayer; //the item script that this giver will insert in the player's inventory
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D actualCollider; //box collider without isTrigger checked
    [SerializeField] private BoxCollider2D triggerCollider; //box collider with isTrigger checked

    //private Transform parent; //the parent gameobject of this item giver

    private Quaternion iniRot;

    [Header("Details of Item")]

    [SerializeField] private Item itemToGive;

    //[SerializeField] private Sprite itemSprite;

    //[SerializeField] private string itemScriptToLoad; //the name of the script to give to player (ex. Potion, Bomb)

    //[SerializeField] private ItemScriptableObject scriptableObject;

    //private Component itemComponent; //the item component added to player using AddComponent()

    //private NewItem itemScript; //the NewItem script of the itemComponent

    private bool spawned = false; //has this item spawned in the world?

    [SerializeField] private bool retrieved = false; //was this item picked up by the player?

    private bool gaveItemToPlayer = false; //did this ItemGiver give the player a NewItem script component?


    //if so, the item won't be able to picked up again


    private void Start()
    {
        iniRot = transform.rotation;

        //Always ignore collision between item and player
        Physics2D.IgnoreLayerCollision(7,11);
    }

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

    private void LateUpdate()
    {
        transform.rotation = iniRot;
    }

    public void SetRetrieved(bool boolean)
    {
        retrieved = boolean;
    }

    //what the item does when the player picks it up
    public override void InteractableAction()
    {
        Debug.Log("Picked up item!");

        //add this new instance of the item to the player's item list
        AddItemToPlayer();

        
    }

    //Invoke AddComponent of the item's script to go to the player's inventory
    //need to AddComponent so the item is attached to the player, not the gameobject it initially spawned with
    public void AddItemToPlayer()
    {
        if(!retrieved){
            if(itemToGive.type != ItemScriptableObject.ItemType.instant)
            {
                //parent = PlayerStats.instance.GetComponentHolder().transform;

                //this.transform.SetParent(parent);

                if(!itemToGive.enabled)
                    itemToGive.enabled = true;

                spriteRenderer.enabled = false;
                actualCollider.enabled = false;
                triggerCollider.enabled = false;

            }
            else
            {
                Debug.Log("If this is an instant item, then return to the object pooler immediately");
            }


            //Set retrived to true so player cannot pick up the item more than once
            SetRetrieved(true);

        }
        //only give item script to player if they its their first time interacting with this ItemGiver
        //if (!gaveItemToPlayer)
        //{
            //adds any item script this ItemGiver was specificed to add, then converts it to NewItem to access methods
            //itemScript = PlayerStats.instance.GetComponentHolder().AddComponent(Type.GetType(itemScriptToLoad)) as NewItem;

            //OLD (WILL THROW AN EXCEPTION IF CASTING DOESN'T WORK)
            //NewItem itemScript = (NewItem) PlayerStats.instance.GetComponentHolder().AddComponent(Type.GetType(itemScriptToLoad));

            //if(itemScript != null)
            //{
                //itemScript.SetScriptableObject(scriptableObject);

            //}
            //else
            //{
                //Debug.Log("An incorrect script name was provided to this Item Giver!");
            //}

            //gaveItemToPlayer = true;
        //}
        //else
        //{
            //itemScript.enabled = true;
        //}
        
    }


    public void OnDrop()
    {
        if (itemToGive.enabled)
            itemToGive.enabled = false;

        //drop this item infront of the player
        transform.position = new Vector2(PlayerStats.instance.GetComponentHolder().transform.position.x, PlayerStats.instance.GetComponentHolder().transform.position.y);

        //parent = null;

        //this.transform.SetParent(null);

        spriteRenderer.enabled = true;

        actualCollider.enabled = true;
        triggerCollider.enabled = true;

        //gaveItemToPlayer = false;

        SetRetrieved(false);

    }
}
