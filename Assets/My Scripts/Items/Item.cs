//using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public abstract class Item : MonoBehaviour, IPriceable
{
    public string itemName;

    public ItemScriptableObject myScriptableObject;

    public ItemScriptableObject.ItemType type;

    public float procChance;

    public int amountOfCharge; //how many times can this item be used?
    public int chargeConsumedPerUse; //how much charge will this item consume on each use?

    public int itemPrice;

    //public bool active;

    //private bool hasSufficientCharge = true;

    //public ItemGiver itemGiver;

    private bool fetchedStats = false; //has this item already fetched its item stats from its ScriptableObject?

    private InputAction useEquipmentBinding;

    private IObjectPool<Item> myPool;

    //private bool wasActiveEquipment = false;

    private void Awake()
    {
        //Debug.Log("MY AWAKE! " + gameObject.name);

        // don't create a new binding every time awake is called
        //if(useEquipmentBinding == null)
        //{
            //Debug.Log("Create binding! " + gameObject.name);
            //useEquipmentBinding = new PlayerInputActions().Player.UseEquipment;
        //}
            

        //disabled on awake so that script won't affect player until they pick it up (which enables this script)
        this.enabled = false;

        //copy values from persistant data source when picked up
        //if (!fetchedStats)
        //{
         InitializeValues();
            //fetchedStats = true;
        //}
        
    }


    private void OnEnable()
    {
        // don't create a new binding every time awake is called
        // calling this here, because Awake is being called each time an Item is taken from pool
        if (useEquipmentBinding == null)
        {
            Debug.Log("Create binding! " + gameObject.name);
            useEquipmentBinding = new PlayerInputActions().Player.UseEquipment;
        }

        OnItemPickup();
        useEquipmentBinding.performed += UseEquipmentOnCommand;
        useEquipmentBinding.Enable();
    }

    private void OnDisable()
    {

        useEquipmentBinding.performed -= UseEquipmentOnCommand;
        useEquipmentBinding.Disable();
    }

    //what the item does for the player
    //passes in player to access components if needed..
    public abstract void ItemAction(GameObject player);

    //take values from persistant data source containing stats for each item
    public abstract void InitializeValues();


    public bool ShouldActivate()
    {
        switch (type)
        {
            case ItemScriptableObject.ItemType.passiveBuff:
                return true;

            //not needed for passive items that do not have to proc during gameplay (probably won't even invoke this function)
            case ItemScriptableObject.ItemType.passiveProc:
                // check if the item's proc chance was successful
                // if not, return and do not allow ability to activate
                // multiply proc chance by PlayerStat's proc chance multiplier
                return Random.value <= (procChance * PlayerStats.instance.GetProcChanceMultiplier());

            case ItemScriptableObject.ItemType.equipment:
                //if this item has sufficient charge, take some away
                //otherwise, return false and don't allow item to activate its use
                if (amountOfCharge < chargeConsumedPerUse)
                {
                    //when player's equipment item is out of charges we can probably play some sound that
                    //indicates the item can't be used
                    Debug.Log("Insufficient Charge");

                    //hasSufficientCharge = false;
                    return false;
                }

                else
                {
                    amountOfCharge -= chargeConsumedPerUse;
                    //hasSufficientCharge = true;

                    return true;
                }

            //this function is not needed for instant items since they will always activate on pickup (probably won't even invoke this function)
            case ItemScriptableObject.ItemType.instant:
                //instant items should always activate
                return true;
        }

        return true;

        
    }


    public void OnItemPickup()
    {
        switch (type)
        {
            default:

            case ItemScriptableObject.ItemType.passiveBuff:
                //adds the item of type "Passive Buff" to the player's passive item inventory
                ItemAction(PlayerStats.instance.GetPlayer());
                break;

            case ItemScriptableObject.ItemType.passiveProc:
                //adds the item of type "Passive Proc" to the player's passive item inventory
                ItemAction(PlayerStats.instance.GetPlayer());
                break;
            case ItemScriptableObject.ItemType.equipment:

                ItemSwapper.instance.activeEquipmentSlot = this;

                break;
            case ItemScriptableObject.ItemType.instant:
                // instant items do not get added to any inventory
                ItemAction(PlayerStats.instance.GetPlayer());

                // an instant will immediately return to their respective pool when picked up (no need to hold it anywhere)
                if (myPool != null)
                    myPool.Release(this);
                break;
        }

    }

    private void UseEquipmentOnCommand(InputAction.CallbackContext context)
    {
        //if this item is an equipment type, then invoke ItemAction() when the UseEquipment binding is peformed
        //player must also be allowed to use the equipment (ex. they cannot if they are in a knockdown state or dead)
        if (PlayerStats.instance.CanUseEquipment && type == ItemScriptableObject.ItemType.equipment && this.enabled)
        {
            ItemAction(PlayerStats.instance.GetPlayer());
        }
        else
        {
            Debug.Log("Only equipment items should activate from this binding!");
        }
    }

    //sets this item's pool equal to th pool based into the function (comes from ItemSpawner)
    public void SetPool(IObjectPool<Item> pool)
    {
        myPool = pool;
    }

    //public void ReturnToPool()
    //{
        //if (myPool != null)
            //myPool.Release(this);
    //}

    public IObjectPool<Item> GetMyPool()
    {
        return myPool;
    }

    // returns the price of this item
    public int GetPrice()
    {
        return itemPrice;
    }
}
