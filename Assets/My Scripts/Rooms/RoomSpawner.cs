using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [Header("Room properties")]
    [SerializeField] private BaseRoom room;

    [Header("Door Type To Instantiate")]
    private int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private LevelManager levelManager; //a reference to the LevelManager singleton

    private int randomRoom; //a random number that will be used to index through the room lists

    [HideInInspector]
    public bool spawnedRoom = false; // a bool that is true or false depending on if this spawner spawned any rooms

    private float destroyTime = 4f; //time until this spawner is destroyed

    private int xCoordinateAdder = 150; //how much we add to the newly spawned room's transform position.x for a spawn location
    private int yCoordinateAdder = 110; //how much we add to the newly spawned room's transform position.y for a spawn location

    private int xCoordinateDivider = 150; //how much we divide the newly spawned room's transform position.x by.. to get a coordinate
    private int yCoordinateDivider = 110; //how much we divide the newly spawned room's transform position.y by.. to get a coordinate
    

    private Vector2 spawnPosition;
    private Vector2 roomCoordinatesToGive;

    private void Start()
    {
        //destroy this spawner after a few seconds
        //Destroy(gameObject, destroyTime);

        //sets "levelManager" equal to the LevelManager singleton so we can access the list of potential rooms
        levelManager = LevelManager.instance;
        InvokeRepeating("ChooseRandomRoomType", 0.05f, 0.05f);
        //InvokeRepeating("SpawnRooms", 0.1f,0.1f);

    }

    private void SpawnRooms(BaseRoom.RoomType roomType, GameObject[] arrayToUse)
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
                spawnPosition = ChooseSpawnPosition(0f, yCoordinateAdder);

                if (spawnPosition == Vector2.zero)
                    break;

                //pick a random number from 0 to the length of the room list
                randomRoom = Random.Range(0, arrayToUse.Length);

                //spawn the random bottom room at generated spawn position
                GameObject bottomRoomSpawned = Instantiate(arrayToUse[randomRoom], spawnPosition, new Quaternion());

                //local cache of the instaniated room's BaseRoom script
                BaseRoom BaseRoomComponentBottom = bottomRoomSpawned.GetComponent<BaseRoom>();

                //fetch the room's BaseRoom component and set its coordinate
                GiveNewCoordinateForSpawnedRoom(BaseRoomComponentBottom, roomCoordinatesToGive);

                levelManager.numberOfSpawnedAllRooms++;
                IncrementNumberOfRoomSpawned(roomType);

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 2:
                //pass in a negative y position value so the new room can spawn below this spawner's room
                spawnPosition = ChooseSpawnPosition(0f, -yCoordinateAdder);

                if (spawnPosition == Vector2.zero)
                    break;

                //pick a random number from 0 to the length of the room list
                randomRoom = Random.Range(0, arrayToUse.Length);

                //spawn the random top room at generated spawn position
                GameObject topRoomSpawned = Instantiate(arrayToUse[randomRoom], spawnPosition, new Quaternion());

                //local cache of the instaniated room's BaseRoom script
                BaseRoom BaseRoomComponentTop = topRoomSpawned.GetComponent<BaseRoom>();

                //fetch the room's BaseRoom component and set its coordinate
                GiveNewCoordinateForSpawnedRoom(BaseRoomComponentTop, roomCoordinatesToGive);

                levelManager.numberOfSpawnedAllRooms++;
                IncrementNumberOfRoomSpawned(roomType);


                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 3:
                //pass in a positive x position value so the new room can spawn to the right of this spawner's room
                spawnPosition = ChooseSpawnPosition(xCoordinateAdder, 0f);

                if (spawnPosition == Vector2.zero)
                    break;

                //pick a random number from 0 to the length of the room list
                randomRoom = Random.Range(0, arrayToUse.Length);

                //spawn the random left room at generated spawn position
                GameObject leftRoomSpawned = Instantiate(arrayToUse[randomRoom], spawnPosition, new Quaternion());

                //local cache of the instaniated room's BaseRoom script
                BaseRoom BaseRoomComponentLeft = leftRoomSpawned.GetComponent<BaseRoom>();

                //fetch the room's BaseRoom component and set its coordinate
                GiveNewCoordinateForSpawnedRoom(BaseRoomComponentLeft, roomCoordinatesToGive);

                levelManager.numberOfSpawnedAllRooms++;
                IncrementNumberOfRoomSpawned(roomType);

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 4:
                //pass in a negative x position value so the new room can spawn to the left of this spawner's room
                spawnPosition = ChooseSpawnPosition(-xCoordinateAdder, 0f);

                if (spawnPosition == Vector2.zero)
                    break;

                //pick a random number from 0 to the length of the room list
                randomRoom = Random.Range(0, arrayToUse.Length);

                //spawn the random right room at generated spawn position
                GameObject rightRoomSpawned = Instantiate(arrayToUse[randomRoom], spawnPosition, new Quaternion());

                //local cache of the instaniated room's BaseRoom script
                BaseRoom BaseRoomComponentRight = rightRoomSpawned.GetComponent<BaseRoom>();

                //fetch the room's BaseRoom component and set its coordinate
                GiveNewCoordinateForSpawnedRoom(BaseRoomComponentRight, roomCoordinatesToGive);

                levelManager.numberOfSpawnedAllRooms++;
                IncrementNumberOfRoomSpawned(roomType);

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;
        }


        //only spawn a room if this spawner hasn't already
        //and if we havent reached the cap on room spawns
        //if (levelManager.numberOfSpawnedRooms < levelManager.roomCap)
        //{
            
            //number of spawned rooms increments by 1 
            //levelManager.numberOfSpawnedRooms++;
            //Debug.Log(levelManager.numberOfSpawnedRooms);

            //set spawnedRoom to true to ensure this spawner only creates one room
            //spawnedRoom = true;

        //}
        
    }

    private void ChooseRandomRoomType()
    {
        int randomRoomType = Random.Range(1, 5);

        switch (randomRoomType)
        {
            case 1:
                if(levelManager.numberOfSpawnedNormalRooms < levelManager.maxNumberOfNormalRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.normal, levelManager.allNormalRooms);
                    
                    //levelManager.numberOfSpawnedNormalRooms++;
                }
                    
                break;
            case 2:
                if (levelManager.numberOfSpawnedTreasureRooms < levelManager.maxNumberOfTreasureRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.treasure, levelManager.allTreasureRooms);
                    //levelManager.numberOfSpawnedTreasureRooms++;
                }
                    
                break;
            case 3:
                if (levelManager.numberOfSpawnedShopRooms < levelManager.maxNumberOfShopRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.shop, levelManager.allShopRooms);
                    //levelManager.numberOfSpawnedShopRooms++;
                }
                    
                break;
            case 4:
                if (levelManager.numberOfSpawnedBossRooms < levelManager.maxNumberOfBossRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.boss, levelManager.allBossRooms);
                    //levelManager.numberOfSpawnedBossRooms++;
                }
                    
                break;
        }

    }

    private void IncrementNumberOfRoomSpawned(BaseRoom.RoomType roomType)
    {
        switch (roomType)
        {
            case BaseRoom.RoomType.normal:
                levelManager.numberOfSpawnedNormalRooms++;
                break;
            case BaseRoom.RoomType.treasure:
                levelManager.numberOfSpawnedTreasureRooms++;
                break;
            case BaseRoom.RoomType.shop:
                levelManager.numberOfSpawnedShopRooms++;
                break;
            case BaseRoom.RoomType.boss:
                levelManager.numberOfSpawnedBossRooms++;
                break;

        }
    }

    private Vector2 ChooseSpawnPosition(float xPosition, float yPosition)
    {
        //this multiplier is used to multiply to the xPosition & yPosition if there are rooms on top of each other
        //int positionMultiplier = 1;

        //generate a Vector2 for the new room based on this spawner's room's position
        // ex. if this spawner is supposed to spawn a room with a bottom door,
        // cont. then we know we must generate a room on top of this spawner's room (so we add a value to the transform.position.y (about 100))
        Vector2 newSpawnPosition = new Vector2(room.transform.position.x + xPosition, room.transform.position.y + yPosition);
        roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);

        //keep checking if the level manager's roomCoordinate dictionary contains the coordinate key
        if (levelManager.roomCoordinatesOccupied.ContainsKey(roomCoordinatesToGive))
        {
            return Vector2.zero;
            //increase the increment
            //positionMultiplier++;

            //newSpawnPosition = new Vector2(room.transform.position.x + ( xPosition * positionMultiplier), room.transform.position.y + ( yPosition * positionMultiplier));
            //roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);
        }

        return newSpawnPosition;
    }
    private void GiveNewCoordinateForSpawnedRoom(BaseRoom spawnedRoom, Vector2 coordinateToGive)
    {
        //add the coordinate of the spawned room as a key, to the dictionary 
        //add the spawned room (Basic Dungeon component) as a value of the coordinate key, to the dictionary
        levelManager.roomCoordinatesOccupied.Add(roomCoordinatesToGive, spawnedRoom);

        //set the coordinates of this new spawned room
        //spawnedRoom.GetComponent<BaseRoom>().SetCoordinates(roomCoordinatesToGive);
        spawnedRoom.SetCoordinates(roomCoordinatesToGive);

    }
}
