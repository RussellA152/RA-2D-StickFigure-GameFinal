using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for spawning in AI and enemies whenever the player enters a new level/area 
//the idea is that the levels will not be new scenes, so we should re-use enemies that were killed/ignored
//when enemies spawn, they should be given a scriptable object to classify them
//enemies should not respawn in the same level, but only when entering a new area
public class AIObjectPooler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
