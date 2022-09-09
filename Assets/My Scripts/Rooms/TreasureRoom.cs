using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureRoom : BaseRoom
{
    //[SerializeField] private List<Transform> itemDisplayList = new List<Transform>();
    //[SerializeField] private int numberOfItems;
    //[SerializeField] private int amountOfItemDisplays;
    //[SerializeField] private Transform[] itemDisplayTransforms; 

    void Start()
    {
        base.Start();
        roomEnemyCountState = BaseRoom.RoomEnemyCount.cleared;
        roomType = RoomType.treasure;

        //itemDisplayTransforms = new Transform[amountOfItemDisplays];
    }

    //public List<Transform> GetItemDisplayTransformList()
    //{
        //return itemDisplayList;
    //}

    //public int GetNumberOfItems()
    //{
        //return numberOfItems;
    //}

    //public void ModifyNumberOfItems(int amount)
    //{
        //numberOfItems += amount;
    //}

}
