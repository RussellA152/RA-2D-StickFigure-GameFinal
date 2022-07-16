using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [Header("Room properties")]
    [SerializeField] private BasicDungeon room;

    [Header("Door Type To Instantiate")]
    private int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private LevelManager templates; //a reference to the DungeonTemplates script

    private int randomRoom; //a random number that will be used to index through the room lists

    [HideInInspector]
    public bool spawnedRoom = false; // a bool that is true or false depending on if this spawner spawned any rooms

    private float destroyTime = 4f; //time until this spawner is destroyed

    private int xCoordinateDivider = 150;
    private int yCoordinateDivider = 110;

    private Vector2 spawnPosition;
    private Vector2 roomCoordinatesToGive;

    private void Start()
    {
        //destroy this spawner after a few seconds
        Destroy(gameObject, destroyTime);

        //sets "templates" equal to the LevelManager singleton so we can access the list of potential rooms
        templates = LevelManager.instance;

        //templates.spawnNewRooms += SpawnRooms;
        //templates.SpawnNewRoomsEvent();

        Invoke("SpawnRooms", 0.1f);

    }

    private void SpawnRooms()
    {
        //only spawn a room if this spawner hasn't already
        //and if we havent reached the cap on room spawns
        if (!spawnedRoom && templates.numberOfSpawnedRooms < templates.roomCap)
        {
            //pick a random direction to spawn a room in
            // 1 --> need bottom door
            // 2 --> need top door
            // 3 --> need left door
            // 4 --> need right door
            openingDirection = Random.Range(1, 5);

            switch (openingDirection)
            {
                case 1:
                    //pass in a positive y position value so the new room can spawn above this spawner's room
                    spawnPosition = ChooseSpawnPosition(0f, 110f);
                    //pick a random room from the bottom rooms list
                    randomRoom = Random.Range(0, templates.allRooms.Length);
                    //spawn the random room at generated spawn position
                    GameObject bottomRoomSpawned = Instantiate(templates.allRooms[randomRoom], spawnPosition, new Quaternion());
                    //set the coordinates of this new bottom room
                    bottomRoomSpawned.GetComponent<BasicDungeon>().SetCoordinates(roomCoordinatesToGive);

                    //Debug.Log(room.gameObject.name +" spawned " + bottomRoomSpawned);

                    break;
                case 2:
                    //pass in a negative y position value so the new room can spawn below this spawner's room
                    spawnPosition = ChooseSpawnPosition(0f, -110f);
                    //pick a random room from the top rooms list
                    randomRoom = Random.Range(0, templates.allRooms.Length);
                    GameObject topRoomSpawned = Instantiate(templates.allRooms[randomRoom], spawnPosition, new Quaternion());
                    //set the coordinates of this new top room
                    topRoomSpawned.GetComponent<BasicDungeon>().SetCoordinates(roomCoordinatesToGive);

                    //Debug.Log(room.gameObject.name + " spawned " + topRoomSpawned);

                    break;
                case 3:
                    //pass in a positive x position value so the new room can spawn to the right of this spawner's room
                    spawnPosition = ChooseSpawnPosition(150f, 0f);
                    //pick a random room from the left rooms list
                    randomRoom = Random.Range(0, templates.allRooms.Length);
                    GameObject leftRoomSpawned = Instantiate(templates.allRooms[randomRoom], spawnPosition, new Quaternion());
                    //set the coordinates of this new left room
                    leftRoomSpawned.GetComponent<BasicDungeon>().SetCoordinates(roomCoordinatesToGive);

                    //Debug.Log(room.gameObject.name + " spawned " + leftRoomSpawned);

                    break;
                case 4:
                    //pass in a negative x position value so the new room can spawn to the left of this spawner's room
                    spawnPosition = ChooseSpawnPosition(-150f, 0f);
                    //pick a random room from the right rooms list
                    randomRoom = Random.Range(0, templates.allRooms.Length);
                    GameObject rightRoomSpawned = Instantiate(templates.allRooms[randomRoom], spawnPosition, new Quaternion());
                    //set the coordinates of this new right room
                    rightRoomSpawned.GetComponent<BasicDungeon>().SetCoordinates(roomCoordinatesToGive);

                    //Debug.Log(room.gameObject.name + " spawned " + rightRoomSpawned);
                    break;
            }
            //number of spawned rooms increments by 1 
            templates.numberOfSpawnedRooms++;

            //set spawnedRoom to true to ensure this spawner only creates one room
            spawnedRoom = true;

            //unsubscribe from event system when this spawner has created a new room
            //templates.spawnNewRooms -= SpawnRooms;

        }
        
    }

    private Vector2 ChooseSpawnPosition(float xPosition, float yPosition)
    {
        //this multiplier is used to multiply to the xPosition & yPosition if there are rooms on top of each other
        int positionMultiplier = 1;

        //generate a Vector2 for the new room based on this spawner's room's position
        // ex. if this spawner is supposed to spawn a room with a bottom door,
        // cont. then we know we must generate a room on top of this spawner's room (so we add a value to the transform.position.y (about 100))
        Vector2 newSpawnPosition = new Vector2(room.transform.position.x + xPosition, room.transform.position.y + yPosition);
        roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);

        //keep checking if the level manager's roomCoordinate list contains the coordinate
        while (templates.roomCoordinatesOccupied.Contains(roomCoordinatesToGive))
        {
            //increase the increment
            positionMultiplier++;

            newSpawnPosition = new Vector2(room.transform.position.x + ( xPosition * positionMultiplier), room.transform.position.y + ( yPosition * positionMultiplier));
            roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);

            //Debug.Log("Move me again!");
        }

        //when while ends, we have a found a valid room coordinate, so we add it to the LevelManager's roomCoordinates list
        templates.roomCoordinatesOccupied.Add(roomCoordinatesToGive);

        return newSpawnPosition;
    }

    private void OnDestroy()
    {
        //CancelInvoke();
        templates.spawnNewRooms -= SpawnRooms;
    }

    private void OnDisable()
    {
        //CancelInvoke();
        templates.spawnNewRooms -= SpawnRooms;
    }
}
