using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
using UnityEngine;
using System;

public class ItemManager : MonoBehaviour
{
    // 3 object pools that contain passive, equipment, and instant items
    ObjectPool<Item> passiveItemPool; 
    ObjectPool<Item> equipmentItemPool; 
    ObjectPool<Item> instantItemPool;

    // A list of item prefabs that are in the game world (items that have been spawned in (picked up by player, or not))
    [SerializeField] private List<Item> activeItems = new List<Item>();

    [Header("Items to Pool")]
    // These are the lists of items that you can drag item prefabs into from the inspector
    // These lists are mutable, so we can remove elements from them, which we will do when an item is spawned (with exception to Instant items)
    [SerializeField] private List<Item> passiveItemsToSpawn; 
    [SerializeField] private List<Item> equipmentItemsToSpawn;
    [SerializeField] private List<Item> instantItemsToSpawn;


    // These are the array of items copied from the lists above, 
    // They act as backups/references to all items in the game, and you cannot remove elements from them
    // Instant items don't have an array because we won't remove them from the above list
    [SerializeField] private Item[] passiveItemsArray;
    [SerializeField] private Item[] equipmentItemsArray; 


    // The amount of items that the respective object pool will pre-allocate
    [SerializeField] private int defaultPassiveCapacity;
    [SerializeField] private int defaultEquipmentCapacity;
    [SerializeField] private int defaultInstantCapacity;

    //public Action spawnItemsInARoomEvent; // this eventsystem is invoked when a set of items has spawned inside of a room
                                          // we will have our shop and treasure rooms subscribe to this eventsystem in OnEnable(), but they won't actually
                                          // do anything until they have all the items in their room (if a room needs 3 items, and it has those 3 items

    private void Awake()
    {
        //creating all three item pools
        //all three use the same OnTake and OnReturn functions
        //however, they use different Constructor functions and have different default capacities
        passiveItemPool = new ObjectPool<Item>(CreatePassiveItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultPassiveCapacity);

        equipmentItemPool = new ObjectPool<Item>(CreateEquipmentItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultEquipmentCapacity);

        instantItemPool = new ObjectPool<Item>(CreateInstantItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultInstantCapacity);

        passiveItemsArray = new Item[passiveItemsToSpawn.Count];
        equipmentItemsArray = new Item[equipmentItemsToSpawn.Count];

        // copy item elements from serialized array, to a list
        TransferItemsFromListToArray(passiveItemsToSpawn, passiveItemsArray);
        TransferItemsFromListToArray(equipmentItemsToSpawn,equipmentItemsArray);


    }

    private void Start()
    {

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

        if(passiveItemsToSpawn.Count == 0 && equipmentItemsToSpawn.Count == 0)
        {

            return;
        }

        if(random == 1)
        {
            if (passiveItemsToSpawn.Count == 0)
            {
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
            if (passiveItemsToSpawn.Count == 0)
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
            if (equipmentItemsToSpawn.Count == 0)
            {
                random = 1;
            }
            else
            {
                var equipmentItem = equipmentItemPool.Get();
                return;
            }
        }
 
    }

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

    Item CreatePassiveItem()
    {
        //create a random index from 0 to the length of the passive item list 
        int randomIndex = Random.Range(0, passiveItemsToSpawn.Count);

        var passiveItem = Instantiate(passiveItemsToSpawn[randomIndex]);

        passiveItem.SetPool(passiveItemPool);

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

        //need to add this item to the activeItems list
        activeItems.Add(item);

        SpawnItemInCurrentRoom(item);

    }

    //this function performs actions on the item when they return to the pool
    //We will release/return items back into the pool when they are ignored by the player until a loop
    void OnReturnItemToPool(Item item)
    {
        item.gameObject.SetActive(false);

        //need to remove this item to the activeItems list
        activeItems.Remove(item);

    }

    private void SpawnItemInCurrentRoom(Item item)
    {
        List<ItemLocker> lockList = null;

        //for loop through each item display transforms inside of the current room (the room must be a shop, treasure, or boss room)
        switch (LevelManager.instance.GetCurrentRoom().roomType)
        {
            case BaseRoom.RoomType.treasure:
                lockList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();
                SetLock(item, lockList, false);

                break;
            case BaseRoom.RoomType.shop:
                lockList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();
                SetLock(item, lockList, true);

                //put code there

                break;
            case BaseRoom.RoomType.boss:
                lockList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();
                SetLock(item, lockList, true);
                break;
        }
           
    }

    private void SetLock(Item item, List<ItemLocker> lockList, bool shouldLock)
    {
        //if this room's itemDisplayList has a length greater than 0..
        if (lockList.Count != 0)
        {
            //iterate through the room's itemDisplayList
            foreach (ItemLocker _lock in lockList)
            {
                //if this lock component isn't null
                if (_lock != null)
                {
                    //set the randomly chosen item's position to the lock's transform position
                    item.transform.localPosition = _lock.gameObject.transform.position;

                    // set the randomly chosen item's parent to the lock's gameobject
                    item.transform.SetParent(_lock.gameObject.transform);

                    // if we should lock this item, call the lock's LockItems() function
                    if (shouldLock)
                        _lock.LockItems();

                    //now remove this lock element from the itemLockList
                    //we remove this element so that the items won't try to be placed on the same transform
                    lockList.Remove(_lock);

                    break;
                }
            }
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
}
