using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class NewRoomSpawner : MonoBehaviour
{
    [Header("Room properties")]
    [SerializeField] private BaseRoom firstRoom;
    private bool spawnedFirstRoom = false;

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

    //private float destroyTime = 4f; //time until this spawner is destroyed

    private int xCoordinateAdder = 155; //how much we add to the newly spawned room's transform position.x for a spawn location
    private int yCoordinateAdder = 110; //how much we add to the newly spawned room's transform position.y for a spawn location

    private int xCoordinateDivider = 155; //how much we divide the newly spawned room's transform position.x by.. to get a coordinate
    private int yCoordinateDivider = 110; //how much we divide the newly spawned room's transform position.y by.. to get a coordinate

    //these bools allow this spawner to check coordinates in each direction
    //if they become false, the spawner can't check in that direction
    //will prevent this spawner from unneccesarily checking level manager's coordinate dictionary 
    //private bool canCheckBelow = true;
    //private bool canCheckAbove = true;
    //private bool canCheckLeft = true;
    //private bool canCheckRight = true;

    [SerializeField] private BaseRoom previouslySpawnedRoom;
    //private Vector2 currentCoordinate;
    private Vector2 spawnPosition;
    private Vector2 roomCoordinatesToGive = new Vector2(0f, 0f); // initially set to vector2.zero because first room will always spawn at (0,0)


    private void Start()
    {
        // the first room is the previouslySpawnedRoom at the start of the game
        //previouslySpawnedRoom = firstRoom;

        //sets "levelManager" equal to the LevelManager singleton so we can access the list of potential rooms
        levelManager = LevelManager.instance;
        

        InvokeRepeating("ChooseRandomRoomType", 0.1f, 0.1f);

        //while(levelManager.numberOfSpawnedAllRooms != levelManager.GetRoomCap())
        //{

            //ChooseRandomRoomType();
        //}

        

    }

    private void Update()
    {
        //if(levelManager.numberOfSpawnedAllRooms == levelManager.GetRoomCap())
        //{
            //CancelInvoke();
            //this.enabled = false;
        //}
    }

    private void ChooseRandomRoomType()
    {
        //spawn the first room before spawning other rooms
        if (!spawnedFirstRoom)
        {   
            SpawnFirstRoom();
            spawnedFirstRoom = true;
            return;
        }

        


        int randomRoomType = Random.Range(1, 5);

        switch (randomRoomType)
        {
            case 1:
                if (levelManager.numberOfSpawnedNormalRooms < levelManager.maxNumberOfNormalRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.normal, levelManager.allNormalRooms);

                    //levelManager.numberOfSpawnedNormalRooms++;
                    //Debug.Log("call invoke");
                }

                break;
            case 2:
                if (levelManager.numberOfSpawnedTreasureRooms < levelManager.maxNumberOfTreasureRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.treasure, levelManager.allTreasureRooms);
                    //Debug.Log("call invoke");
                    //levelManager.numberOfSpawnedTreasureRooms++;
                }

                break;
            case 3:
                if (levelManager.numberOfSpawnedShopRooms < levelManager.maxNumberOfShopRooms)
                {
                    SpawnRooms(BaseRoom.RoomType.shop, levelManager.allShopRooms);
                    //Debug.Log("call invoke");
                    //levelManager.numberOfSpawnedShopRooms++;
                }

                break;
            case 4:
                //if this room is trying to spawn a boss, it needs to be the last room
                //typically, only 1 boss room is allowed to spawn
                if (levelManager.numberOfSpawnedBossRooms < levelManager.maxNumberOfBossRooms && levelManager.numberOfSpawnedAllRooms == levelManager.GetRoomCap() - 1)
                {
                    SpawnRooms(BaseRoom.RoomType.boss, levelManager.allBossRooms);
                    //Debug.Log("call invoke");
                    //levelManager.numberOfSpawnedBossRooms++;
                }

                break;
        }


    }

    private void SpawnFirstRoom()
    {
        Debug.Log("What is this spawn coordinate?: " + spawnPosition);
        //spawn the random bottom room at generated spawn position
        GameObject firstRoomSpawned = Instantiate(firstRoom.gameObject, spawnPosition, new Quaternion());

        //local cache of the instaniated room's BaseRoom script
        BaseRoom firstBaseRoomComponent = firstRoomSpawned.GetComponent<BaseRoom>();

        //fetch the room's BaseRoom component and set its coordinate
        GiveNewCoordinateForSpawnedRoom(firstBaseRoomComponent, roomCoordinatesToGive);
        Debug.Log("What is this coordinate?: " + roomCoordinatesToGive);

        levelManager.numberOfSpawnedAllRooms++;

        IncrementNumberOfRoomSpawned(firstBaseRoomComponent.roomType);

        previouslySpawnedRoom = firstBaseRoomComponent;

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

                spawnPosition = ChooseSpawnPosition(0f, yCoordinateAdder);
                //pick a random number from 0 to the length of the room list
                randomRoom = Random.Range(0, arrayToUse.Length);

                //spawn the random bottom room at generated spawn position
                GameObject topRoomSpawned = Instantiate(arrayToUse[randomRoom], spawnPosition, new Quaternion());

                //local cache of the instaniated room's BaseRoom script
                BaseRoom BaseRoomComponentTop = topRoomSpawned.GetComponent<BaseRoom>();

                //fetch the room's BaseRoom component and set its coordinate
                GiveNewCoordinateForSpawnedRoom(BaseRoomComponentTop, roomCoordinatesToGive);

                levelManager.numberOfSpawnedAllRooms++;
                IncrementNumberOfRoomSpawned(roomType);

                previouslySpawnedRoom = BaseRoomComponentTop;

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 2:

                spawnPosition = ChooseSpawnPosition(0f, -yCoordinateAdder);
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

                previouslySpawnedRoom = BaseRoomComponentBottom;


                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 3:

                spawnPosition = ChooseSpawnPosition(xCoordinateAdder, 0f);
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

                previouslySpawnedRoom = BaseRoomComponentRight;

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;

            case 4:

                spawnPosition = ChooseSpawnPosition(-xCoordinateAdder, 0f);

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

                previouslySpawnedRoom = BaseRoomComponentLeft;

                //break out of this loop, and set "spawnedRoom" to true to prevent more rooms from spawning
                break;
        }

        if (levelManager.numberOfSpawnedAllRooms == levelManager.GetRoomCap())
        {
            CancelInvoke();
            this.enabled = false;

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
        Vector2 newSpawnPosition = new Vector2(previouslySpawnedRoom.transform.position.x + xPosition, previouslySpawnedRoom.transform.position.y + yPosition);
        roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);

        //keep checking if the level manager's roomCoordinate dictionary contains the coordinate key
        //return Vector2.zero if the room coordinate is already occupied
        if (levelManager.roomCoordinatesOccupied.ContainsKey(roomCoordinatesToGive))
        {
            newSpawnPosition = new Vector2(previouslySpawnedRoom.transform.position.x + xPosition, previouslySpawnedRoom.transform.position.y + yPosition);
            roomCoordinatesToGive = new Vector2(newSpawnPosition.x / xCoordinateDivider, newSpawnPosition.y / yCoordinateDivider);
            //return Vector2.zero;
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

    private void OnDisable()
    {
        CancelInvoke();
    }
    private void OnDestroy()
    {
        CancelInvoke();
    }
}
