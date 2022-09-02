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

    private void Start()
    {
        //SpawnItem(ItemScriptableObject.ItemType.passiveBuff);
        //SpawnItem(ItemScriptableObject.ItemType.passiveProc);
        SpawnItem(ItemScriptableObject.ItemType.equipment);
        //SpawnItem(ItemScriptableObject.ItemType.instant);
    }

    public void SpawnItem(ItemScriptableObject.ItemType itemType)
    {
        switch (itemType)
        {
            //if the item we will spawn is of type PassiveBuff
            case ItemScriptableObject.ItemType.passiveBuff:
                var passiveBuffItem = passiveItemPool.Get();
                //PickRandomSpawnLocation(passiveBuffItem);
                break;

            //if the item we will spawn is of type PassiveProc
            case ItemScriptableObject.ItemType.passiveProc:
                var passiveProcItem = passiveItemPool.Get();
                //PickRandomSpawnLocation(passiveProcItem);
                break;

            //if the item we will spawn is of type Equipment
            case ItemScriptableObject.ItemType.equipment:
                var equipmentItem = equipmentItemPool.Get();
                //PickRandomSpawnLocation(equipmentItem);
                break;

            //if the item we will spawn is of type Instant
            case ItemScriptableObject.ItemType.instant:
                var instantItem = instantItemPool.Get();
                //PickRandomSpawnLocation(instantItem);
                
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

        //pick a random spawn location for this item
        PickRandomSpawnLocation(item);

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

    //will pick a random spawn location from the current room (The room must contain an item display)
    public void PickRandomSpawnLocation(Item item)
    {
        item.transform.position = new Vector2(10f, 10f);

    }
}
