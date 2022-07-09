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

    private LevelManager templates; //a reference to the DungeonTemplates script

    private int rand; //a random number that will be used to index through the room lists

    [HideInInspector]
    public bool spawned = false; // a bool that is true or false depending on if this spawner spawned a room

    private float destroyTime = 4f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        //sets "templates" equal to the LevelManager singleton so we can access the list of potential rooms
        templates = LevelManager.instance;

        Invoke("SpawnRooms",0.4f);
    }

    private void Update()
    {
        

    }

    private void SpawnRooms()
    {
        //only spawn rooms while roomsSpawned is false
        if (!spawned && templates.numberOfSpawnedRooms < templates.roomCap)
        {
            switch (openingDirection)
            {
                case 1:
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], gameObject.transform.position, new Quaternion());
                    templates.numberOfSpawnedRooms++;
                    break;
                case 2:
                    rand = Random.Range(0, templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand], gameObject.transform.position, new Quaternion());
                    templates.numberOfSpawnedRooms++;
                    break;
                case 3:
                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], gameObject.transform.position, new Quaternion());
                    templates.numberOfSpawnedRooms++;
                    break;
                case 4:
                    rand = Random.Range(0, templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand], gameObject.transform.position, new Quaternion());
                    templates.numberOfSpawnedRooms++;
                    break;
            }
            //set roomsSpawned to true so that we can stop spawning rooms
            spawned = true;

            //destroy the spawner after it has finished spawning rooms
            //Destroy(gameObject);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we collide with another spawn point, then we know another room has been spawned at this spawn point
        //thus we should destory this spawn point to prevent more than one room spawning here
        if (collision.CompareTag("SpawnPoint"))
        {
            //check if what we collided with, has a "spawned" bool equal to false
            // also if this spawner has a "spawned" bool equal to false
            // then spawn a wall blocking off any opening
            if(collision.gameObject.GetComponent<DungeonSpawner>().spawned == false && spawned == false)
            {
                //Instantiate(templates.closedRoom, gameObject.transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            spawned = true;
            
        }
    }
}