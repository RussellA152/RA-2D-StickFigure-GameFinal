using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [Header("Room properties")]
    [SerializeField] private BasicDungeon room;
    [SerializeField] private GameObject door;

    [Header("Door Type To Instantiate")]
    [SerializeField] private int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private LevelManager templates; //a reference to the DungeonTemplates script

    private int randomRoom; //a random number that will be used to index through the room lists

    [HideInInspector]
    public bool spawnedRoom = false; // a bool that is true or false depending on if this spawner spawned any rooms

    private float destroyTime = 4f; //time until this spawner is destroyed

    private int xCoordinateDivider = 140;
    private int yCoordinateDivider = 100;

    private Vector2 spawnPosition;
    //private Vector2 spawnPositionCheck;

    private void Start()
    {
        //Destroy(gameObject, destroyTime);

        //sets "templates" equal to the LevelManager singleton so we can access the list of potential rooms
        templates = LevelManager.instance;

        Invoke("SpawnRooms", 0.8f);

    }

    private void SpawnRooms()
    {
        //only a room if this spawner hasn't already
        // and if we havent reached the cap on room spawns
        if (!spawnedRoom && templates.numberOfSpawnedRooms < templates.roomCap)
        {
            switch (openingDirection)
            {
                case 1:
                    //pass in a positive y position value so the new room can spawn above this spawner's room
                    spawnPosition = ChooseSpawnPosition(0f, 100f);
                    //pick a random room from the bottom rooms list
                    randomRoom = Random.Range(0, templates.bottomRooms.Length);
                    //spawn the random room at generated spawn position
                    Instantiate(templates.bottomRooms[randomRoom], spawnPosition, new Quaternion());


                    break;
                case 2:
                    //pass in a negative y position value so the new room can spawn below this spawner's room
                    spawnPosition = ChooseSpawnPosition(0f, -100f);
                    //pick a random room from the top rooms list
                    randomRoom = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[randomRoom], spawnPosition, new Quaternion());
                    


                    break;
                case 3:
                    //pass in a positive x position value so the new room can spawn to the right of this spawner's room
                    spawnPosition = ChooseSpawnPosition(140f, 0f);
                    //pick a random room from the left rooms list
                    randomRoom = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[randomRoom], spawnPosition, new Quaternion());
                   

                    break;
                case 4:
                    //pass in a negative x position value so the new room can spawn to the left of this spawner's room
                    spawnPosition = ChooseSpawnPosition(-140f, 0f);
                    //pick a random room from the right rooms list
                    randomRoom = Random.Range(0, templates.rightRooms.Length);             
                    Instantiate(templates.rightRooms[randomRoom], spawnPosition, new Quaternion());
                    
                    break;
            }
            //number of spawned rooms increments by 1 
            templates.numberOfSpawnedRooms++;

            //set spawnedRoom to true to ensure this spawner only creates one room
            spawnedRoom = true;

        }

        //only try to remove doors when all rooms have finished spawning
        else if (templates.roomGenerationComplete)
        {
            switch (openingDirection)
            {
                case 1:
                    //Check if a room above this spawner's room exists
                    CheckAdjacentRooms(0f, 1f);
                    break;
                case 2:
                    //Check if a room below this spawner's room exists
                    CheckAdjacentRooms(0f, -1f);
                    break;
                case 3:
                    //Check if a room to the right of this spawner's room exists
                    CheckAdjacentRooms(1f, 0f);
                    break;
                case 4:
                    //Check if a room to the left of this spawner's room exists
                    CheckAdjacentRooms(-1f, 0f);
                    break;
            }
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
        Vector2 spawnCoordinate = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);

        //keep checking if the level manager's roomCoordinate list contains the coordinate
        while (templates.roomCoordinatesOccupied.Contains(spawnCoordinate))
        {
            //increase the increment
            positionMultiplier++;

            newSpawnPosition = new Vector2(room.transform.position.x + ( xPosition * positionMultiplier), room.transform.position.y + ( yPosition * positionMultiplier));
            spawnCoordinate = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);
        }

        //when while ends, we have a found a valid room coordinate, so we add it to the LevelManager's roomCoordinates list
        templates.roomCoordinatesOccupied.Add(spawnCoordinate);

        return newSpawnPosition;
    }

    //this function checks if there is a room next to this spawner's room
    //if so, leave this room alone
    //if not, then remove this door
    private void CheckAdjacentRooms(float xCoordinate, float yCoordinate)
    {
        //make a vector2 representing a room next to this spawner's room
        //could be to the left (x - 1) or right (x + 1)
        //could be below (y - 1) or above (y + 1)
        Vector2 adjacentRoomCoordinate = new Vector2(room.roomCoordinate.x + xCoordinate, room.roomCoordinate.y + yCoordinate);

        //if the room coordinate list from LevelManager does NOT contain this adjacent room coordinate,
        // then there is an opening and we should remove this door
         if ( !templates.roomCoordinatesOccupied.Contains(adjacentRoomCoordinate) )
        {
            Debug.Log("REMOVE MY DOOR " + room.gameObject.name);
        }
                
        
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
