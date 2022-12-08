using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;


    [SerializeField] private Transform itemHolder; // gameobject that holds retrieved Items as children

    private InputAction useEquipmentBinding;

    public Item activeEquipmentSlot;

    public event Action swapEquipmentEvent; // event that occurs when player swaps their current equipment item for a new equipment item

    public event Action itemPickupEvent; // event that occurs when the player picks up any item

    private bool canSwapEquipment = true;


    // 3 object pools that contain passive, equipment, and instant items
    ObjectPool<Item> passiveItemPool; 
    ObjectPool<Item> equipmentItemPool; 
    ObjectPool<Item> instantItemPool;

    // A list of item prefabs that are in the game world (items that have been spawned in (picked up by player, or not))
    //[SerializeField] private List<Item> activeItems = new List<Item>();
    private Dictionary<Item, Item> activeItems = new Dictionary<Item, Item>(); // dictionary (key = Item, value = bool representing if the item is active in the game world or not (aka dropped on the map))

    [Header("Items to Pool")]
    // These are the lists of items that you can drag item prefabs into from the inspector
    // These lists are mutable, so we can remove elements from them, which we will do when an item is spawned (with exception to Instant items)
    [SerializeField] private List<Item> passiveItemsToSpawn; 
    [SerializeField] private List<Item> equipmentItemsToSpawn;
    [SerializeField] private List<Item> instantItemsToSpawn;


    // These are the array of items copied from the lists above, 
    // They act as backups/references to all items in the game, and you cannot remove elements from them
    // Instant items don't have an array because we won't remove them from the above list
    private Item[] passiveItemsArray;
    private Item[] equipmentItemsArray;

    private bool calledIgnored = false;

    // The amount of items that the respective object pool will pre-allocate
    [SerializeField] private int defaultPassiveCapacity;
    [SerializeField] private int defaultEquipmentCapacity;
    [SerializeField] private int defaultInstantCapacity;

    [Header("Active number of items")]
    [SerializeField] private int activePassiveCount;
    [SerializeField] private int activeEquipmentCount;
    [SerializeField] private int activeInstantCount;

    [Header("Inactive number of items")]
    [SerializeField] private int inactivePassiveCount;
    [SerializeField] private int inactiveEquipmentCount;
    [SerializeField] private int inactiveInstantCount;

    [SerializeField] private int maxInstantItemsAtOnce;


    //public Action spawnItemsInARoomEvent; // this eventsystem is invoked when a set of items has spawned inside of a room
    // we will have our shop and treasure rooms subscribe to this eventsystem in OnEnable(), but they won't actually
    // do anything until they have all the items in their room (if a room needs 3 items, and it has those 3 items

    private void Awake()
    {
        // debugging button to instantly return items to pool (TEMPORARY)
        useEquipmentBinding = new PlayerInputActions().Player.CombatRoll;
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        //creating all three item pools
        //all three use the same OnTake and OnReturn functions
        //however, they use different Constructor functions and have different default capacities
        passiveItemPool = new ObjectPool<Item>(CreatePassiveItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultPassiveCapacity);

        equipmentItemPool = new ObjectPool<Item>(CreateEquipmentItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultEquipmentCapacity);

        instantItemPool = new ObjectPool<Item>(CreateInstantItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultInstantCapacity);

        passiveItemsArray = new Item[passiveItemsToSpawn.Count];
        equipmentItemsArray = new Item[equipmentItemsToSpawn.Count];

        // Transfer all elements from each item list to a dictionary
        //CopyItemsToDictionary(passiveItemsToSpawn);
        //CopyItemsToDictionary(equipmentItemsToSpawn);
        //CopyItemsToDictionary(instantItemsToSpawn);

        // copy item elements from serialized array, to a list
        //TransferItemsFromListToArray(passiveItemsToSpawn, passiveItemsArray);
        //TransferItemsFromListToArray(equipmentItemsToSpawn,equipmentItemsArray);


    }

    private void Start()
    {
        useEquipmentBinding.Enable();
        //StartCoroutine(TestItemReturn(3f));
    }

    private void Update()
    {
        // TEMP (USING FOR DEBUGGING IGNORED ITEMS RETURNING TO POOLS)
        //if (useEquipmentBinding.triggered && !calledIgnored)
        //{
        //    Debug.Log("Return items on command!");
        //    calledIgnored = true;
        //    ReturnIgnoredItems();
        //}

        activePassiveCount = passiveItemPool.CountActive;
        inactivePassiveCount = passiveItemPool.CountInactive;

        activeEquipmentCount = equipmentItemPool.CountActive;
        inactiveEquipmentCount = equipmentItemPool.CountInactive;

        activeInstantCount = instantItemPool.CountActive;
        inactiveInstantCount = instantItemPool.CountInactive;


        // if the instant items reaches max count (too many items at once)
        // tell the object pool to return them all back to the pool
        if(inactiveInstantCount >= maxInstantItemsAtOnce)
        {
            foreach(Item item in activeItems.Keys)
            {
                if(item.type == ItemScriptableObject.ItemType.instant)
                {
                    instantItemPool.Release(item);
                }
            }
        }

    }

    private void OnEnable()
    {
        //subscribe RepeatSpawn to the onPlayerEnterNewArea
        //when player enters a room, spawn items inside of that room (if needed)
        // currentRoom is always updated before onPlayerEnterNewArea is invoked, so its safe to use current room in ItemSpawner
        LevelManager.instance.onPlayerEnterNewArea += RepeatSpawn;
    }

    private void OnDisable()
    {
        LevelManager.instance.onPlayerEnterNewArea -= RepeatSpawn;
    }

    //This function will call "SpawnRandomItem" for as many times as the current rooom needs
    //Ex. if the current room needs 2 items, then this function will call "SpawnRandomItem" 2 times
    private void RepeatSpawn()
    {
        //get the current room and its number of items needed
        BaseRoom currentRoom = LevelManager.instance.GetCurrentRoom();
        int numberOfItemsNeeded = currentRoom.GetNumberOfItems();

        for(int i = 0; i < numberOfItemsNeeded; i++)
        {
            SpawnRandomItem();

            currentRoom.ModifyNumberOfItems(-1);

        }
    }

    public void SpawnRandomItem()
    {
        //create a random number from 1-3 (decides what kind of item to spawn)
        int random = Random.Range(1, 4);

        // if both item list counts are empty...
        if(passiveItemsToSpawn.Count <= 0 && equipmentItemsToSpawn.Count <= 0)
        {

            return;
        }

        // if random was 1, spawn a passive item
        if(random == 1)
        {
            if (passiveItemsToSpawn.Count <= 0)
            {
                // if we don't have enough passive items, spawn an equipment
                random = 3;
            }
            else
            {
                var passiveBuffItem = passiveItemPool.Get();
                return;
            }
        }

        if(random == 2)
        {
            if (passiveItemsToSpawn.Count <= 0)
            {
                random = 3;
            }
            else
            {
                var passiveProcItem = passiveItemPool.Get();
                return;

            }
        }

        if(random == 3)
        {
            if (equipmentItemsToSpawn.Count <= 0)
            {
                // if we don't have enough equipment items, spawn a passive item
                random = 1;
            }
            else
            {
                var equipmentItem = equipmentItemPool.Get();
                return;
            }
        }
 
    }
    /*
    public void SpawnItemFromType(ItemScriptableObject.ItemType itemType)
    {
        switch (itemType)
        {
            //if the item we will spawn is of type PassiveBuff
            case ItemScriptableObject.ItemType.passiveBuff:
                var passiveBuffItem = passiveItemPool.Get();
                break;

            //if the item we will spawn is of type PassiveProc
            case ItemScriptableObject.ItemType.passiveProc:
                var passiveProcItem = passiveItemPool.Get();
                break;

            //if the item we will spawn is of type Equipment
            case ItemScriptableObject.ItemType.equipment:
                var equipmentItem = equipmentItemPool.Get();
                break;

            //if the item we will spawn is of type Instant
            case ItemScriptableObject.ItemType.instant:
                var instantItem = instantItemPool.Get();
                
                break;
        }

    }
    */
    Item CreatePassiveItem()
    {
        //create a random index from 0 to the length of the passive item list 
        int randomIndex = Random.Range(0, passiveItemsToSpawn.Count);

        // instantiate the randomly chosen item
        var passiveItem = Instantiate(passiveItemsToSpawn[randomIndex]);

        // set the object pool of the item (so it remembers which object to return to)
        passiveItem.SetPool(passiveItemPool);

        // remove item from dedicated list to prevent duplicates from spawning
        passiveItemsToSpawn.RemoveAt(randomIndex);

        return passiveItem;
    }

    Item CreateEquipmentItem()
    {
        //create a random index from 0 to the length of the equipment item list 
        int randomIndex = Random.Range(0, equipmentItemsToSpawn.Count);

        var equipmentItem = Instantiate(equipmentItemsToSpawn[randomIndex]);

        equipmentItem.SetPool(equipmentItemPool);

        equipmentItemsToSpawn.RemoveAt(randomIndex);

        return equipmentItem;
    }

    Item CreateInstantItem()
    {
        Debug.Log("CREATE INSTANT ITEM!");

        //create a random index from 0 to the length of the instant item array 
        int randomIndex = Random.Range(0, instantItemsToSpawn.Count);

        var instantItem = Instantiate(instantItemsToSpawn[randomIndex]);

        instantItem.SetPool(instantItemPool);

        return instantItem;
    }

    // this function performs actions on the item when they are spawned in 
    void OnTakeItemFromPool(Item item)
    {
                      
        //set item active
        item.gameObject.SetActive(true);

        switch (item.type)
        {
            //if the item we will spawn is of type PassiveBuff
            case ItemScriptableObject.ItemType.passiveBuff:
                SpawnItemInCurrentRoom(item);

                break;

            //if the item we will spawn is of type PassiveProc
            case ItemScriptableObject.ItemType.passiveProc:
                SpawnItemInCurrentRoom(item);
                break;

            //if the item we will spawn is of type Equipment
            case ItemScriptableObject.ItemType.equipment:
                SpawnItemInCurrentRoom(item);
                break;

            // only non-instant item types will spawn in the current room (on displays with a potential lock)
            // instant items will spawn as drops from enemies
            case ItemScriptableObject.ItemType.instant:
                    

                break;
        }

    }

    // this function performs actions on the item when they return to the pool
    // We will release/return items back into the pool when they are ignored by the player till the end of the game
    void OnReturnItemToPool(Item item)
    {
        // separate the item from its parent (usually ItemHolder)
        item.transform.SetParent(null);

        // disable item gameobject
        item.gameObject.SetActive(false);

        //need to remove this item to the activeItems list (it is not active in the game world anymore)
        //activeItems[item] = false;
        //activeItems.Remove(item);

        // when an item returns to pool, it should be placed back into drop pools again (able to be dropped again)
        // without this, the item won't be able to be randomly chosen for spawn anymore
        switch (item.type)
        {
            //if the item we will spawn is of type PassiveBuff
            case ItemScriptableObject.ItemType.passiveBuff:
                passiveItemsToSpawn.Add(activeItems[item]);
                //passiveItemsToSpawn.Add(item);
                break;

            //if the item we will spawn is of type PassiveProc
            case ItemScriptableObject.ItemType.passiveProc:
                passiveItemsToSpawn.Add(activeItems[item]);
                break;

            //if the item we will spawn is of type Equipment
            case ItemScriptableObject.ItemType.equipment:
                equipmentItemsToSpawn.Add(activeItems[item]);
                break;

            //if the item we will spawn is of type Instant
            // then do not re-add to a list because instant items are not removed from list in the first place
            //case ItemScriptableObject.ItemType.instant:
                //instantItemsToSpawn.Add(item);

                //break;
        }

    }

    private void SpawnItemInCurrentRoom(Item item)
    {
        // the unlocker list is a list of Unlocker's that each Item Room contains (this is where items will be placed on)
        List<Unlocker> unlockerList = null;

        // check what room the player is currently in, to spawn an item inside
        // pass in the current random item, the room's dedicated unlockerList (a list that holds ItemLockers), and a bool representing whether we should lock the item or not into SetLock()
        switch (LevelManager.instance.GetCurrentRoom().roomType)
        {
            case BaseRoom.RoomType.treasure:
                unlockerList = LevelManager.instance.GetCurrentRoom().GetUnlockerList();
                // treasure rooms will not have locked items (the player can freely pick up items)
                SetLock(item, unlockerList, false);

                break;
            case BaseRoom.RoomType.shop:
                unlockerList = LevelManager.instance.GetCurrentRoom().GetUnlockerList();
                // shop rooms will have locked items with a paywall
                SetLock(item, unlockerList, true);

                break;
            case BaseRoom.RoomType.boss:
                unlockerList = LevelManager.instance.GetCurrentRoom().GetUnlockerList();
                // boss room will have locked items with a condition that the boss must be killed
                SetLock(item, unlockerList, true);
                break;
        }
           
    }

    private void SetLock(Item item, List<Unlocker> unlockerList, bool shouldLock)
    {
        //if this room's unlockerList has a length greater than 0..
        if (unlockerList.Count != 0)
        {
            //iterate through the room's UnlockerList
            foreach (Unlocker unlocker in unlockerList)
            {
                //if this ItemLocker component isn't null
                if (unlocker != null)
                {
                    //set the randomly chosen item's position to the lock's gameObject transform position
                    item.transform.localPosition = unlocker.gameObject.transform.position;

                    // set the randomly chosen item's parent to the lock's gameobject
                    item.transform.SetParent(unlocker.gameObject.transform);

                    //item.transform.GetComponent<ItemGiver>().SetPositionInRoom(unlocker.transform.position);

                    // if we should lock this item, call the item's AddLock() function
                    if (shouldLock)
                    {
                        // locks item... (ItemGiver implements ILockable)
                        item.gameObject.GetComponent<ILockable>().AddLock();

                        //set the unlocker's :lockedGameObject" variable equal to this newly spawned item's gameobject
                        unlocker.SetGameObjectToUnlock(item.gameObject);
                    }

                    //now remove this lock element from the UnlockerList
                    //we remove this element so that the items won't try to be placed on the same transform
                    unlockerList.Remove(unlocker);

                    break;
                }
            }
        }
    }

    public void SpawnInstantItemAtLocation(Vector2 positionToSpawn)
    {

        // fetch an instant item from pool
        var instantItem = instantItemPool.Get();

        //Debug.Log("is instant item null? " + instantItem);

        // spawn the instant item at "positionToSpawn" (usually where an enemy died)
        SpawnItemAtPosition(instantItem, positionToSpawn);

    }

    

    private void SpawnItemAtPosition(Item item, Vector2 position)
    {
        item.transform.position = position;
        //Debug.Log("Where is this item spawning?" + item.transform.position);
    }

    private void CopyItemsToDictionary(List<Item> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            // add all potential items and set bool to false ("false" means the item is not active in the game world)
            activeItems.Add(list[i], list[i]);
        }
    }

    // transfer every Item from list to an array for backup
    private void TransferItemsFromListToArray(List<Item> list, Item[] array)
    {

        for(int i = 0; i < list.Count; i++)
        {
            Debug.Log("List of i: " + list[i] + "Array of i: " + array[i]);
            array[i] = list[i];
        }
    }

    public void SwapActiveEquipment()
    {
        if (swapEquipmentEvent != null && canSwapEquipment)
        {
            swapEquipmentEvent();
        }
    }

    public void NewItemPickup()
    {
        if (itemPickupEvent != null)
        {

            itemPickupEvent();
            
        }
    }

    public Item GetActiveEquipmentItem()
    {
        return activeEquipmentSlot;
    }


    public Transform GetItemHolder()
    {
        return itemHolder;
    }

    public void SetCanSwapEquipment(bool boolean)
    {
        canSwapEquipment = boolean;
    }

    public bool GetCanSwapEquipment()
    {
        return canSwapEquipment;
    }


    //DEBUGGING FUNCTIONS FOR RETURNING ITEMS TO POOL (TEMPORARY)

    //IEnumerator TestItemReturn(float timer)
    //{
    //Debug.Log("Starting Item Return Coroutine!");
    //yield return new WaitForSeconds(timer);

    //ReturnIgnoredItems();
    //}

    // this function will check all items in the game world, and check if the player has picked them up
    // if the player has not picked them up after beating the level, they will return the pool

    private void ReturnIgnoredItems()
    {
        // iterate through all items that have been dropped into the world (meaning.. active)
        foreach (Item item in activeItems.Values)
        {
            // if an item is disabled (always disabled when spawned in),
            // and their parent gameobject is not this ItemManager (when picked up, their parent becomes the itemHolder),
            // then this item was never picked up by the player, and should return to pool
            if(!item.enabled && item.transform.parent != itemHolder)
            {
                item.ReturnToPool();
            }

        }
    }
}
