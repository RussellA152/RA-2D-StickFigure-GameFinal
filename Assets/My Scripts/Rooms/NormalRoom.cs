using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoom : BaseRoom
{
    // Start is called before the first frame update

    private void Start()
    {
        base.Start();
        // starting room is always cleared on start
        //if(!isStartingRoom)
            //roomEnemyCountState = RoomEnemyCount.cleared;
        roomType = RoomType.normal;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyCountStatus();
    }
}
