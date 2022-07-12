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
    public bool spawned = false; // a bool that is true or false depending on if this spawner spawned any rooms

    private float destroyTime = 4f; //time until this spawner is destroyed

    private Vector2 spawnPosition;
    private Vector2 spawnPositionCheck;

    private void Start()
    {
        Destroy(gameObject, destroyTime);

        //sets "templates" equal to the LevelManager singleton so we can access the list of potential rooms
        templates = LevelManager.instance;

        Invoke("SpawnRooms", 0.1f);

    }

    private void SpawnRooms()
    {
        //STARTING ROOM NEEDS TO BE ADDED TO LIST, OTHERWISE ANOTHER ROOM WILL SPAWN ON TOP OF IT
        //BUT WE DONT ADD STARTING ROOM IN HERE
        int iterator = 1;

        //only spawn rooms while roomsSpawned is false
        if (!spawned && templates.numberOfSpawnedRooms < templates.roomCap)
        {
            //Debug.Log(gameObject.name + "   is   Spawned  =  " + spawned);
            switch (openingDirection)
            {
                case 1:
                    spawnPosition = new Vector2(transform.parent.position.x, transform.parent.position.y + 100);
                    spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                    while (templates.roomCoordinatesOccupied.Contains(spawnPositionCheck))
                    {
                        iterator++;
                        spawnPosition = new Vector2(transform.parent.position.x, transform.parent.position.y + (100 * iterator));
                        spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                        Debug.Log("contained that spawn spot UP");
                    }     
                    rand = Random.Range(0, templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand], spawnPosition, new Quaternion());

                    templates.roomCoordinatesOccupied.Add(spawnPositionCheck);

                    iterator = 1;
                    break;
                case 2:
                    spawnPosition = new Vector2(transform.parent.position.x, transform.parent.position.y - 100);
                    spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                    while (templates.roomCoordinatesOccupied.Contains(spawnPositionCheck))
                    {
                        iterator++;
                        spawnPosition = new Vector2(transform.parent.position.x, transform.parent.position.y - (100 * iterator));
                        spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);

                        Debug.Log("contained that spawn spot DOWN");
                    }

                    rand = Random.Range(0, templates.topRooms.Length);

                    Instantiate(templates.topRooms[rand], spawnPosition, new Quaternion());

                    templates.roomCoordinatesOccupied.Add(spawnPositionCheck);

                    iterator = 1;
                    
                    break;
                case 3:
                    spawnPosition = new Vector2(transform.parent.position.x + 140, transform.parent.position.y);
                    spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);

                    while (templates.roomCoordinatesOccupied.Contains(spawnPositionCheck))
                    {
                        iterator++;
                        spawnPosition = new Vector2(transform.parent.position.x + (140 * iterator), transform.parent.position.y);
                        spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                        Debug.Log("contained that spawn spot RIGHT");
                    }

                    rand = Random.Range(0, templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand], spawnPosition, new Quaternion());
                    iterator = 1;

                    templates.roomCoordinatesOccupied.Add(spawnPositionCheck);

                    break;
                case 4:
                    spawnPosition = new Vector2(transform.parent.position.x - 140, transform.parent.position.y);
                    spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                    while (templates.roomCoordinatesOccupied.Contains(spawnPositionCheck))
                    {
                        iterator++;
                        spawnPosition = new Vector2(transform.parent.position.x - (140 * iterator), transform.parent.position.y);
                        spawnPositionCheck = new Vector2(spawnPosition.x / 140, spawnPosition.y / 100);
                        Debug.Log("contained that spawn spot LEFT");
                    }
                        

                    rand = Random.Range(0, templates.rightRooms.Length);             
                    Instantiate(templates.rightRooms[rand], spawnPosition, new Quaternion());
                    iterator = 1;

                    templates.roomCoordinatesOccupied.Add(spawnPositionCheck);

                    break;
            }
            //number of spawned rooms increments by 1 
            templates.numberOfSpawnedRooms++;

            //set roomsSpawned to true so that we can stop spawning rooms
            spawned = true;

            //Debug.Log(transform.parent.gameObject.name + "   is   Spawned  =  " + spawned);

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if we collide with another spawn point, then we know another room has been spawned at this spawn point
        //thus we should destory this spawn point to prevent more than one room spawning here
        //if (collision.CompareTag("SpawnPoint"))
        //{
            //check if what we collided with, has a "spawned" bool equal to false
            // also if this spawner has a "spawned" bool equal to false
            // then spawn a wall blocking off any opening
            //if(collision.gameObject.GetComponent<DungeonSpawner>().spawned == false && spawned == false)
            //{
                //Instantiate(templates.closedRoom, gameObject.transform.position, Quaternion.identity);
                //Debug.Log("Destroy a colliding room!");
                //Destroy(gameObject);
            //}
            //spawned = true;
            //Debug.Log("Collided with another room!");
            
        //}
    }

    private void OnDestroy()
    {
        //CancelInvoke();
        templates.spawnNewRooms -= SpawnRooms;
    }

    private void OnDisable()
    {
        //CancelInvoke();
        templates.spawnNewRooms -= SpawnRooms;
    }
}
