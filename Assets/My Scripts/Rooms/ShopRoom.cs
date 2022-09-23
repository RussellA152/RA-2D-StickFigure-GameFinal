using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : BaseRoom
{

    void Start()
    {
        base.Start();
        roomEnemyCountState = BaseRoom.RoomEnemyCount.cleared;
        roomType = RoomType.shop;

        //itemDisplayTransforms = new Transform[amountOfItemDisplays];
    }



}
   