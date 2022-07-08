using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicDungeon: MonoBehaviour
{
    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area

    private LevelManager templates;

    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onPlayerEnterNewArea += GiveNewSpawnLocations;

        templates = LevelManager.instance;

        //add this dungeon the "rooms" list inside of the level manager
        AddDungeon();

    }

    private void OnDestroy()
    {
        LevelManager.instance.onPlayerEnterNewArea -= GiveNewSpawnLocations;
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

    private void AddDungeon()
    {
        templates.rooms.Add(this.gameObject);
    }
}
