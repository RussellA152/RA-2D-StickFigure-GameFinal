using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRoom : MonoBehaviour
{
    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area
    // Start is called before the first frame update
    void Start()
    {
        LevelManager.instance.onPlayerEnterNewArea += GiveNewSpawnLocations;
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
        LevelManager.instance.UpdateSpawnLocations(spawnLocationsOfThisLevel);
    }
}
