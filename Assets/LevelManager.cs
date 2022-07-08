using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    private Dictionary<Transform, bool> spawnLocations = new Dictionary<Transform, bool>();  // a dictionary with the keys being the spawn location
                                                                                             // and the values being the "occupied" boolean, which is true or false when an enemy has spawned there or not

    public event Action onPlayerEnterNewArea;

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
        //TEMPORARY
        //This event system should be called when the player has entered a new area
        EnteringNewAreaEvent();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnteringNewAreaEvent()
    {
        if (onPlayerEnterNewArea != null)
        {
            onPlayerEnterNewArea();
        }
    }

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
            //Debug.Log("added new locations?");
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
        Debug.Log("Not enough spawn locations for this enemy");

        return new Vector2(0,0);
    }
}
