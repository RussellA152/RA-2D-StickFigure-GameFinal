using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class DungeonSpawner : MonoBehaviour
{
    [SerializeField] private int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door

    private DungeonTemplates templates; //a reference to the DungeonTemplates script

    private int rand; //a random number that will be used to index through the room lists

    private bool roomsSpawned = false; // a bool that is true or false depending on if all the rooms have finished spawning


    private void Start()
    {
        //finds the Dungeon manager's "Dungeon Templates" script component
        templates = GameObject.FindGameObjectWithTag("Dungeons").GetComponent<DungeonTemplates>();

        Invoke("SpawnRooms",0.1f);
    }

    private void Update()
    {
        

    }

    private void SpawnRooms()
    {
        //only spawn rooms while roomsSpawned is false
        if (!roomsSpawned)
        {
            switch (openingDirection)
            {
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], gameObject.transform.position, new Quaternion());
                    break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], gameObject.transform.position, new Quaternion());
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], gameObject.transform.position, new Quaternion());
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], gameObject.transform.position, new Quaternion());
                    break;
            }
        }

        //set roomsSpawned to true so that we can stop spawning rooms
        roomsSpawned = true;
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we collide with another spawn point, then we know another room has been spawned at this spawn point
        //thus we should destory this spawn point to prevent more than one room spawning here
        if (collision.CompareTag("SpawnPoint"))
        {
            Destroy(gameObject);
        }
    }
}
