using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for checking for adjacent rooms/ connecting doors
//if this script detects that a room has an adjacent room (a room above, below, or to the left/right), it will check if there aren't connecting doors (has a dead end)
//and it will add a door to ensure all rooms connect
//this script also checks if there aren't any adjacent rooms, then it will remove doors (prevents openings leading to nowhere)
//only checks for doors AFTER room generation has finished
public class AdjacentRoomCheck : MonoBehaviour
{
    [SerializeField] private BasicDungeon room; //the full room

    //private BasicDungeon roomBelow; // the room BELOW this room
    //private BasicDungeon roomAbove; // the room ABOVE this room
    //private BasicDungeon roomLeft; // the room to the LEFT of this room
    //private BasicDungeon roomRight; // the room to the RIGHT of this room

    //the 4 doors of this room (some doors could already be disabled or enabled, it depends on the prefab)
    //but if needed, any door can be disabled or enabled 
    [Header("All Doors")]
    [SerializeField] private Door bottomDoor;
    [SerializeField] private Door topDoor;
    [SerializeField] private Door leftDoor;
    [SerializeField] private Door rightDoor;

    private LevelManager levelManager;

    private void Start()
    {
        levelManager = LevelManager.instance;

        StartCoroutine(WaitUntilRoomGenHasFinished());

    }

    IEnumerator WaitUntilRoomGenHasFinished()
    {
        //wait until room generation is complete to check for adjacent rooms
        while (levelManager.dungeonGenerationState == LevelManager.GenerationProgress.incomplete)
        {
            yield return null;
        }

        //check EVERY direction for a room

        //check for room above (will need a neighboring bottom door)
        CheckAdjacentRooms(0f, 1f, topDoor);
        //check for room below (will need a neighboring top door)
        CheckAdjacentRooms(0f, -1f, bottomDoor);
        //check for room to the right (will need a neighboring left door)
        CheckAdjacentRooms(1f, 0f, rightDoor);
        //check for room to the left (will need a neighboring right door)
        CheckAdjacentRooms(-1f, 0f, leftDoor);


    }

    //this function checks if there is a room next to this spawner's room
    //if so, check if we need to add a door for connection
    //if not, then remove this door (to prevent opening)
    private void CheckAdjacentRooms(float xCoordinate, float yCoordinate, Door door)
    {
        //make a vector2 representing a room next to this spawner's room
        //could be to the left (x - 1) or right (x + 1)
        //could be below (y - 1) or above (y + 1)
        Vector2 adjacentRoomCoordinate = new Vector2(room.localRoomCoordinate.x + xCoordinate, room.localRoomCoordinate.y + yCoordinate);
        

        //if an adjacent room exists, check the neighboring doors
        if (levelManager.roomCoordinatesOccupied.ContainsKey(adjacentRoomCoordinate))
        {
            GameObject adjacentRoom = levelManager.roomCoordinatesOccupied[adjacentRoomCoordinate];
            FindNearbyDoor(door, adjacentRoom);
        }

        //if the room coordinate list from LevelManager does NOT contain this adjacent room coordinate,
        //then there is an opening and we should remove this door
        if (!levelManager.roomCoordinatesOccupied.ContainsKey(adjacentRoomCoordinate))
        {
            RemoveDoor(door.transform);
        }

        

    }

    //pass in a door to check if we can add (enable)
    private void AddDoor(Transform doorToEnable)
    {
        if (!doorToEnable.gameObject.activeSelf)
            doorToEnable.gameObject.SetActive(true);

    }

    //pass in a door to check if we can remove (disable)
    private void RemoveDoor(Transform doorToDisable)
    {
        if (doorToDisable.gameObject.activeSelf)
            doorToDisable.gameObject.SetActive(false);
    }

    private void FindNearbyDoor(Door myDoor, GameObject adjacentRoom)
    {
        //if a door exists here, find the neighbor
        if (myDoor.gameObject.activeSelf)
        {
            if(myDoor == bottomDoor)
            {
                myDoor.SetNeighboringDoor(adjacentRoom.GetComponent<BasicDungeon>().topDoor);
            }
            else if (myDoor == topDoor)
            {
                myDoor.SetNeighboringDoor(adjacentRoom.GetComponent<BasicDungeon>().bottomDoor);
            }
            else if (myDoor == leftDoor)
            {
                myDoor.SetNeighboringDoor(adjacentRoom.GetComponent<BasicDungeon>().rightDoor);
            }
            else if(myDoor == rightDoor)
            {
                myDoor.SetNeighboringDoor(adjacentRoom.GetComponent<BasicDungeon>().leftDoor);
            }
        }
    }
    /*
    public BasicDungeon GetRoomAbove()
    {
        return roomAbove;
    }
    public BasicDungeon GetRoomBelow()
    {
        return roomBelow;
    }
    public BasicDungeon GetRoomLeft()
    {
        return roomLeft;
    }
    public BasicDungeon GetRoomRight()
    {
        return roomRight;
    }
    */

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
