using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is placed on the "Clog Preventer" gameobject in the starting dungeon/room gameobject
//it is meant to prevent closed rooms from spawning on top of the starting room
public class ClogPrevention : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //prevents starting dungeon/room from getting clogged with closed rooms
        if(collision.CompareTag("SpawnPoint"))
            Destroy(collision.gameObject);
    }
}
