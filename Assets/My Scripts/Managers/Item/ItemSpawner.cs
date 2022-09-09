using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    ObjectPool<Item> passiveItemPool; 
    ObjectPool<Item> equipmentItemPool; 
    ObjectPool<Item> instantItemPool;

    [SerializeField] private List<Item> activeItems = new List<Item>();

    [Header("Items to Pool")]
    [SerializeField] private Item[] passiveItemsToSpawn;
    [SerializeField] private Item[] equipmentItemsToSpawn;
    [SerializeField] private Item[] instantItemsToSpawn;

    [SerializeField] private int defaultPassiveCapacity;
    [SerializeField] private int defaultEquipmentCapacity;
    [SerializeField] private int defaultInstantCapacity;

    private void Awake()
    {
        //creating all three item pools
        //all three use the same OnTake and OnReturn functions
        //however, they use different Constructor functions and have different default capacities
        passiveItemPool = new ObjectPool<Item>(CreatePassiveItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultPassiveCapacity);

        equipmentItemPool = new ObjectPool<Item>(CreateEquipmentItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultEquipmentCapacity);

        instantItemPool = new ObjectPool<Item>(CreateInstantItem, OnTakeItemFromPool, OnReturnItemToPool, null, true, defaultInstantCapacity);

        //inReserve = defaultCapacity;

    }

    private void OnEnable()
    {
        LevelManager.instance.onPlayerEnterNewArea += SpawnRandomItem;
    }

    private void OnDisable()
    {
        LevelManager.instance.onPlayerEnterNewArea -= SpawnRandomItem;
    }

    public void SpawnRandomItem()
    {
        //items won't spawn inside of normal rooms, only shop,treasure, or boss rooms
        if(LevelManager.instance.GetCurrentRoom().roomType != BaseRoom.RoomType.normal)
        {
            //create a random number from 1-3 (decides what kind of item to spawn)
            int random = Random.Range(1, 4);

            switch (random)
            {
                //if the item we will spawn is of type PassiveBuff
                case 1:
                    var passiveBuffItem = passiveItemPool.Get();
                    Debug.Log("Spawn passive buff item");
                    //PickRandomSpawnLocation(passiveBuffItem);
                    break;

                //if the item we will spawn is of type PassiveProc
                case 2:
                    var passiveProcItem = passiveItemPool.Get();
                    Debug.Log("Spawn passive proc item");
                    //PickRandomSpawnLocation(passiveProcItem);
                    break;

                //if the item we will spawn is of type Equipment
                case 3:
                    var equipmentItem = equipmentItemPool.Get();
                    Debug.Log("Spawn equipment item");
                    //PickRandomSpawnLocation(equipmentItem);
                    break;
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
        int randomIndex = Random.Range(0, passiveItemsToSpawn.Length);

        var passiveItem = Instantiate(passiveItemsToSpawn[randomIndex]);

        passiveItem.SetPool(passiveItemPool);

        return passiveItem;
    }

    Item CreateEquipmentItem()
    {
        //create a random index from 0 to the length of the equipment item list 
        int randomIndex = Random.Range(0, equipmentItemsToSpawn.Length);

        var equipmentItem = Instantiate(equipmentItemsToSpawn[randomIndex]);

        equipmentItem.SetPool(equipmentItemPool);

        return equipmentItem;
    }

    Item CreateInstantItem()
    {
        //create a random index from 0 to the length of the instant item list 
        int randomIndex = Random.Range(0, instantItemsToSpawn.Length);

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

        //pick a random spawn location for this item
        //PickRandomSpawnLocation(item);

        //inReserve--;
    }

    //this function performs actions on the item when they return to the pool
    //We will release/return items back into the pool when they are ignored by the player until a loop
    void OnReturnItemToPool(Item item)
    {
        item.gameObject.SetActive(false);

        //need to remove this item to the activeItems list
        activeItems.Remove(item);

        //inReserve++;
    }

    private void SpawnItemInCurrentRoom(Item item)
    {
        List<Transform> itemDisplayList = null;

        //for loop through each item display transforms inside of the current room (the room must be a shop, treasure, or boss room)
        switch (LevelManager.instance.GetCurrentRoom().roomType)
        {
            case BaseRoom.RoomType.treasure:
                itemDisplayList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();
                
                break;
            case BaseRoom.RoomType.shop:
                itemDisplayList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();

                break;
            case BaseRoom.RoomType.boss:
                itemDisplayList = LevelManager.instance.GetCurrentRoom().GetItemDisplayTransformList();
                break;
        }

        if(itemDisplayList.Count != 0)
        {
            foreach (Transform displayTransform in itemDisplayList)
            {
                Debug.Log("Entering for each loop");
                if (displayTransform != null)
                {
                    Debug.Log("Placing an item on a display");
                    item.transform.localPosition = displayTransform.position;
                    itemDisplayList.Remove(displayTransform);
                    break;
                }
            }
        }
        

        
    }

    //will pick a random spawn location from the current room (The room must contain an item display)
    public void PickRandomSpawnLocation(Item item)
    {
        //testing
        //item.transform.position = new Vector2(10f, 10f);

    }
}
