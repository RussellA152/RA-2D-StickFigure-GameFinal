using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public BaseRoom currentRoom; // the room the player is inside of

    public event Action onPlayerEnterNewArea;
    public event Action spawnNewRooms;

    public DungeonSize dungeonSize; //the "DungeonSize" state determines how many rooms we will have

    [HideInInspector]
    public GenerationProgress dungeonGenerationState; //the state of the dungeon generation.. is it complete or not?

    public GameObject[] allNormalRooms; //all room available to spawn
    public GameObject[] allTreasureRooms; //all room available to spawn
    public GameObject[] allShopRooms; //all room available to spawn
    public GameObject[] allBossRooms; //all room available to spawn

    public List<GameObject> spawnedRooms; //list of currently spawned in rooms (always 1 higher than spawned rooms because starting room is included)

    [Header("Current Amount Spawned")]
    public int numberOfSpawnedAllRooms; //number of rooms that have been spawned
    public int numberOfSpawnedNormalRooms; //number of rooms that have been spawned
    public int numberOfSpawnedTreasureRooms; //number of rooms that have been spawned
    public int numberOfSpawnedShopRooms; //number of rooms that have been spawned
    public int numberOfSpawnedBossRooms; //number of rooms that have been spawned

    
    public int roomCap; //max number of rooms that can spawn (random based on the "DungeonSize" state)

    [Header("Max Number Can Spawn")]
    public int maxNumberOfNormalRooms;
    public int maxNumberOfTreasureRooms;
    public int maxNumberOfShopRooms;
    public int maxNumberOfBossRooms;

    public Dictionary<Vector2,BaseRoom> roomCoordinatesOccupied = new Dictionary<Vector2,BaseRoom>();


    public enum DungeonSize
    {
        // different states of a level determine how the room cap
        // ex. small levels will have a smaller number of rooms than medium or large levels
        small,
        medium,
        large
    }

    public enum GenerationProgress
    {
        incomplete, //all rooms have NOT finished spawning in
        complete //all rooms have finished spawning in
    }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

    }
    private void OnEnable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
        
        RandomRoomCap();
    }

    // Update is called once per frame
    void Update()
    {
        // if the number of spawned rooms is equal to the room cap
        // then we know we have reached the max number of rooms
        // and rooms will stop spawning
        if (numberOfSpawnedAllRooms == roomCap)
            dungeonGenerationState = GenerationProgress.complete;
        else
            dungeonGenerationState = GenerationProgress.incomplete;

        //Debug.Log("The current room is " + currentRoom);

    }

    public void EnteringNewAreaEvent()
    {
        if (onPlayerEnterNewArea != null)
        {
            onPlayerEnterNewArea();
        }
    }

    public void SpawnNewRoomsEvent()
    {
        if(spawnNewRooms != null)
        {
            spawnNewRooms();
        }
    }

    public void RandomRoomCap()
    {
        //set the room cap based on the "DungeonSize" state value
        switch (dungeonSize)
        {
            case DungeonSize.small:

                maxNumberOfNormalRooms = Random.Range(5, 7);
                maxNumberOfTreasureRooms = 1;
                maxNumberOfShopRooms = 1;
                maxNumberOfBossRooms = 1;
                break;
            case DungeonSize.medium:

                maxNumberOfNormalRooms = Random.Range(7, 9);
                maxNumberOfTreasureRooms = Random.Range(1, 3);
                maxNumberOfShopRooms = 1;
                maxNumberOfBossRooms = 1;
                break;
            case DungeonSize.large:

                maxNumberOfNormalRooms = Random.Range(9, 12);
                maxNumberOfTreasureRooms = Random.Range(1, 3);
                maxNumberOfShopRooms = Random.Range(1, 3);
                maxNumberOfBossRooms = 1;

                break;

        }
        //number of boss rooms not included
        roomCap = maxNumberOfNormalRooms + maxNumberOfTreasureRooms + maxNumberOfShopRooms + maxNumberOfBossRooms;

        Debug.Log("Room cap is " + roomCap);
        Debug.Log("Normal Room Count: " + maxNumberOfNormalRooms);
        Debug.Log("Treasure Room Count: " + maxNumberOfTreasureRooms);
        Debug.Log("Shop Room Count: " + maxNumberOfShopRooms);
        Debug.Log("Boss Room Count: " + maxNumberOfBossRooms);
    }


    //sets the "currentRoom" to the room passed in the function
    //used when player enters another room from a door
    public void UpdateCurrentRoom(BaseRoom newCurrentRoom)
    {
        currentRoom = newCurrentRoom;
    }

    public BaseRoom GetCurrentRoom()
    {
        return currentRoom;
    }

    //return a room based on the value of its coordinate
    public BaseRoom GetRoomByCoordinate(Vector2 roomCoordinate)
    {
        return roomCoordinatesOccupied[roomCoordinate];
    }
}
