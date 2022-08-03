using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTemplates : MonoBehaviour
{
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    public GameObject closedRoom; // a "wall" that is about the size of a room that prevents player from leaving dungeon

    public List<GameObject> rooms;
}
