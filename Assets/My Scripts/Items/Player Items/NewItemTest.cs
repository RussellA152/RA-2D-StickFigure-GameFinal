using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemTest : NewItem
{
    public override void ItemAction(GameObject player)
    {
        Debug.Log("Invoke NewItemTest's ItemAction!");
    }

    public override void InitializeValues()
    {
        Debug.Log("Fetch Stats!");


        //will fetch from persistant data source but just testing to see if stats would even carry over
        type = ItemType.equipment;

        procChance = 50f;
    }
}
