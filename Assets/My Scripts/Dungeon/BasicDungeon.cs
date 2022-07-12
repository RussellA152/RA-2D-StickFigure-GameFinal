using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDungeon: MonoBehaviour
{
    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area

    private LevelManager templates;

    private int xCoordinateDivider = 140;
    private int yCoordinateDivider = 100;

    [HideInInspector]
    public Vector2 roomCoordinate;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onPlayerEnterNewArea += GiveNewSpawnLocations;

        templates = LevelManager.instance;

        //add this room to the general "rooms" list inside of LevelManager
        AddDungeon();

        Debug.Log(gameObject.name + " coordinate is " + roomCoordinate);

    }

    private void OnDestroy()
    {
        LevelManager.instance.onPlayerEnterNewArea -= GiveNewSpawnLocations;
        //LevelManager.instance.spawnNewRooms -= AddDungeon;
    }
    private void OnDisable()
    {
        //LevelManager.instance.spawnNewRooms -= AddDungeon;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        templates.rooms.Add(this.gameObject);
    }

    private void CreateCoordinate()
    {
        //roomCoordinate = new Vector2(transform.position.x / xCoordinateDivider, transform.position.y / yCoordinateDivider);
        //templates.roomCoordinatesOccupied.Add(roomCoordinate);
        //templates.roomCoordinates.Push(roomCoordinate);
    }
}
