using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public BasicDungeon currentRoom; // the room the player is inside of

    //private Dictionary<Transform, bool> spawnLocations = new Dictionary<Transform, bool>();  // a dictionary with the keys being the spawn location
                                                                                             // and the values being the "occupied" boolean, which is true or false when an enemy has spawned there or not
    public event Action onPlayerEnterNewArea;
    public event Action spawnNewRooms;

    public DungeonSize dungeonSize; //the "DungeonSize" state determines how many rooms we will have

    [HideInInspector]
    public GenerationProgress dungeonGenerationState; //the state of the dungeon generation.. is it complete or not?

    public GameObject[] allRooms; //all room available to spawn
    //public GameObject[] bottomRooms; //array of all rooms with a bottom door
    //public GameObject[] topRooms; //array of all rooms with a top door
    //public GameObject[] leftRooms; //array of all rooms with a left door
    //public GameObject[] rightRooms; //array of all rooms with a right door

    //public GameObject closedRoom; // a "wall" that is about the size of a room that prevents player from leaving dungeon

    public List<GameObject> spawnedRooms; //list of currently spawned in rooms (always 1 higher than spawned rooms because starting room is included)

    [HideInInspector]
    public int numberOfSpawnedRooms; //number of rooms that have been spawned

    [HideInInspector]
    public int roomCap; //max number of rooms that can spawn (random based on the "DungeonSize" state)

    public Dictionary<Vector2,GameObject> roomCoordinatesOccupied = new Dictionary<Vector2,GameObject>();

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
        //TEMPORARY *
        //This event system should be called when the player has entered a new area
        
        
        RandomRoomCap();
    }

    // Update is called once per frame
    void Update()
    {
        // if the number of spawned rooms is equal to the room cap
        // then we know we have reached the max number of rooms
        // and rooms will stop spawning
        if (numberOfSpawnedRooms == roomCap)
            dungeonGenerationState = GenerationProgress.complete;
        else
            dungeonGenerationState = GenerationProgress.incomplete;

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
                roomCap = Random.Range(6, 8);
                break;
            case DungeonSize.medium:
                roomCap = Random.Range(8, 11);
                break;
            case DungeonSize.large:
                roomCap = Random.Range(14, 17);
                break;

        }
        Debug.Log("Room cap is " + roomCap);

    }

    /*
    public void UpdateSpawnLocations(List<Transform> spawnLocList)
    {

        //clear the spawn locations of the previous room
        if (spawnLocations.Count != 0 || spawnLocations != null)
            spawnLocations.Clear();

        //add new spawn locations to this list
        foreach(Transform loc in spawnLocList)
        {
            //sets the keys to false because no enemy has spawned there yet
            spawnLocations.Add(loc, false);
        }
    }

    public Vector2 GetSpawnLocation()
    {
        int iterator = 0;
        foreach(Transform location in spawnLocations.Keys)
        {
            if(location != null)
            {
                //Debug.Log("for each loop start");
                if (spawnLocations.ElementAt(iterator).Value == false)
                {
                    iterator++;
                    spawnLocations[location] = true;

                    //Debug.Log("Return a random loc!");
                    return location.position;
                }
                else
                    iterator++;
            }
            
        }
        //Transform randomLocation = spawnLocations.ElementAt(randomIndex).Key;
        Debug.Log("Not enough spawn locations for this enemy. Or some other room has an empty list. Enemy will spawn at (0,0)");

        return new Vector2(0,0);
    }
    */

    //sets the "currentRoom" to the room passed in the function
    //used when player enters another room from a door
    public void UpdateCurrentRoom(BasicDungeon newCurrentRoom)
    {
        currentRoom = newCurrentRoom;
    }

    public BasicDungeon GetCurrentRoom()
    {
        return currentRoom;
    }

    //return a room based on the value of its coordinate
    public GameObject GetRoomByCoordinate(Vector2 roomCoordinate)
    {
        return roomCoordinatesOccupied[roomCoordinate];
    }
}
