using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : BaseRoom
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        roomEnemyCountState = BaseRoom.RoomEnemyCount.cleared;
        roomType = RoomType.shop;
    }

    // Update is called once per frame
    void Update()
    {


    }
}
   