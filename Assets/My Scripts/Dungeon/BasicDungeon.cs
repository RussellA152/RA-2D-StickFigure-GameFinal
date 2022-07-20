using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDungeon: MonoBehaviour
{
    public RoomType roomType; // the type of room this is.. could be a boss room, treasure room, or just a regular room

    [SerializeField] private bool isStartingRoom; //Is this room, the starting room? If so, we will manually add the coordinates to the list

    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area

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
        //a room will be uncleared by default
        roomEnemyCountState = RoomEnemyCount.uncleared;

        LevelManager.instance.onPlayerEnterNewArea += GiveNewSpawnLocations;

        levelManager = LevelManager.instance;

        //add this room to the general "rooms" list inside of LevelManager
        AddDungeon();

        //starting room needs to create its own coordinate because it is not spawned by a dungeonSpawner
        if (isStartingRoom)
            CreateCoordinate();

        

    }

    //give the level manager the spawn locations of this area
    private void GiveNewSpawnLocations()
    {
        if (spawnLocationsOfThisLevel.Count == 0 || spawnLocationsOfThisLevel == null)
        {
            Debug.Log("This room doesn't contain any spawn locations! " + gameObject.name);
            return;
        }
        else
            LevelManager.instance.UpdateSpawnLocations(spawnLocationsOfThisLevel);
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

    private void OnDestroy()
    {
        LevelManager.instance.onPlayerEnterNewArea -= GiveNewSpawnLocations;
        //LevelManager.instance.spawnNewRooms -= AddDungeon;
    }
}
