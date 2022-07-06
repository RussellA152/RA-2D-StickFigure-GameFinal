using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Spawn Locations")]
    public List<Transform> spawnLocationsOfThisLevel = new List<Transform>(); //a list of spawn locations dedicated to this level
                                                                              // the spawn locations will be overriden when the player enters a new area

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 GetRandomSpawnLocation()
    {
        int randomIndex = Random.Range(0, spawnLocationsOfThisLevel.Count);
        Transform randomLocation = spawnLocationsOfThisLevel[randomIndex];

        if (randomLocation != null)
            return spawnLocationsOfThisLevel[randomIndex].position;
        else
            return new Vector2(0,0);
    }
}
