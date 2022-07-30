using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BasicDungeon: MonoBehaviour
{
    public RoomType roomType; // the type of room this is.. could be a boss room, treasure room, or just a regular room

    [SerializeField] private bool isStartingRoom; //Is this room, the starting room? If so, we will manually add the coordinates to the list

    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area

    private Dictionary<Transform, bool> spawnLocations = new Dictionary<Transform, bool>();

    [SerializeField] private int numberOfEnemiesCanSpawnHere; //the number of enemies that WILL spawn in this room
                                                              // SHOULD ideally be the number of spawn locations of the room, otherwise enemies will just spawn in a random spot

    private int numberOfEnemiesAliveInRoom; //the number of enemies current alive inside of this room
                                            //when this value goes to 0, the room will be considered "cleared" and doors will be available to use


    [HideInInspector]
    public RoomEnemyCount roomEnemyCountState; //What is the state of the amount of enemies in the room? Is it clear? Or are some enemies still alive?

    private LevelManager levelManager;

    private int xCoordinateDivider = 150;
    private int yCoordinateDivider = 110;

    [Header("All Doors")]
    public Door bottomDoor;
    public Door topDoor;
    public Door leftDoor;
    public Door rightDoor;

    [Header("X & Y Coordinates")]
    public Vector2 localRoomCoordinate; //NOT TO BE CONFUSED with the room coordinate in the Level Manager's dictionary (this is a local version of the same vector2 value*)

    public enum RoomEnemyCount
    {
        uncleared, //if any enemy dedicated to this room is still alive (doors won't let player exit)

        cleared //if ALL enemies dedicated to this room are dead (doors will allow player to exit)
    }

    public enum RoomType
    {
        regularRoom,

        treasureRoom,

        shopRoom,

        bossRoom
    }


    // Start is called before the first frame update
    void Start()
    {
        //numberOfEnemiesAliveInRoom should be equal to the numberOfEnemiesCanSpawnHere AT START
        //it will decrease later when player kills the enemies in this room
        numberOfEnemiesAliveInRoom = numberOfEnemiesCanSpawnHere;

        //a room will be uncleared by default (cleared when debugging)
        roomEnemyCountState = RoomEnemyCount.uncleared;

        //reference to level manager singleton
        levelManager = LevelManager.instance;

        //add this room to the general "rooms" list inside of LevelManager
        AddDungeon();

        //starting room needs to create its own coordinate because it is not spawned by a dungeonSpawner
        if (isStartingRoom)
            CreateCoordinate();

        //update the spawn location dictionary with the spawn locations provided in the editor
        UpdateSpawnLocationDictionary();

        

    }

    private void Update()
    {
        //if no more enemies are alive in here, the room is cleared
        //otherwise, it is uncleared
        if (numberOfEnemiesAliveInRoom == 0)
            roomEnemyCountState = RoomEnemyCount.cleared;
        else
            roomEnemyCountState = RoomEnemyCount.uncleared;
    }

    //give the level manager the spawn locations of this area
    public Vector2 GiveNewSpawnLocations()
    {
        int iterator = 0;
        foreach (Transform location in spawnLocations.Keys)
        {
            if (location != null)
            {
                //Debug.Log("for each loop start");
                if (spawnLocations.ElementAt(iterator).Value == false)
                {
                    iterator++;
                    spawnLocations[location] = true;

                    Debug.Log("Location I spawned at was " + location.position);
                    //Debug.Log("Return a random loc!");
                    return location.position;
                }
                else
                    iterator++;
            }

        }
        //Transform randomLocation = spawnLocations.ElementAt(randomIndex).Key;
        Debug.Log("Not enough spawn locations for this enemy. Or some other room has an empty list. Enemy will spawn at (0,0)");

        return new Vector2(0, 0);

    }
    private void UpdateSpawnLocationDictionary()
    {
        //clears the dictionary if it was already populated for whatever reason (kinda obsolete)
        if (spawnLocations.Count != 0 || spawnLocations != null)
            spawnLocations.Clear();

        //add new spawn locations to this list
        foreach (Transform loc in spawnLocationsOfThisLevel)
        {
            //sets the keys to false because no enemy has spawned there yet
            spawnLocations.Add(loc, false);
        }
    }

    //This function must be inside of the "BasicDungeon" script because
    // if this function was inside of DungeonSpawner, then rooms would be added in multiple times
    private void AddDungeon()
    {
        levelManager.spawnedRooms.Add(this.gameObject);
    }

    //creates a room coordinate for this room
    private void CreateCoordinate()
    {
        localRoomCoordinate = new Vector2(transform.position.x / xCoordinateDivider, transform.position.y / yCoordinateDivider);

        levelManager.roomCoordinatesOccupied.Add(localRoomCoordinate,this.gameObject);
    }

    //sets the room coordinates equal to the parameter
    public void SetCoordinates(Vector2 newCoordinate)
    {
        localRoomCoordinate = newCoordinate;
        
    }

    //return the numberOfEnemiesCanSpawnHere (needed by AISpawner)
    public int GetNumberOfEnemiesCanSpawnHere()
    {
        return numberOfEnemiesCanSpawnHere;
    }

    //decreases "numberOfEnemiesCanSpawnHere" by 1, preventing rooms from spawning more enemies than it is allowed to
    public void DecrementNumberOfEnemiesCanSpawnHere()
    {
        numberOfEnemiesCanSpawnHere--;
    }

    //decreases "numberOfEnemiesAliveInHere" by 1
    //when this value becomes 0, the room will considered cleared
    public void DecrementNumberOfEnemiesAliveInHere()
    {
        numberOfEnemiesAliveInRoom--;
    }
}
