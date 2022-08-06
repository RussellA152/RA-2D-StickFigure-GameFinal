using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalRoom : BasicRoom
{
    // Start is called before the first frame update
    void Start()
    {
        roomEnemyCountState = RoomEnemyCount.uncleared;
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyCountStatus();
    }
}
