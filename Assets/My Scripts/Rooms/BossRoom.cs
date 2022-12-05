using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : BaseRoom
{
    private Vector2 positionToSpawnAt;
    [SerializeField] private Transform centerOfRoom;
    private BaseRoom potentialRoomOnTop;

    [Header("Potential Boss Spawn Locations")]
    // the spawn location of the boss depends on the door the player entered from (this is so that the boss does not spawn right next to the player)
    [SerializeField] private Transform rightSpawnLocation;
    [SerializeField] private Transform middleSpawnLocation;
    [SerializeField] private Transform leftSpawnLocation;


    //[SerializeField] private List<Transform> itemDisplayList = new List<Transform>();
    //[SerializeField] private int numberOfItems;
    //[SerializeField] private int amountOfItemDisplays;
    //[SerializeField] private Transform[] itemDisplayTransforms;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        roomEnemyCountState = BaseRoom.RoomEnemyCount.cleared;
        roomType = RoomType.boss;

        levelManager.onAllRoomsSpawned += CheckIfRoomIsOnTop;

        levelManager.onPlayerEnterNewArea += UpdateBossSpawnLocation;
    }

    private void CheckIfRoomIsOnTop()
    {
        // check if there is a room that has the same coordinates as the boss room
        foreach(BaseRoom room in levelManager.roomCoordinatesOccupied.Values)
        {
            if(room.localRoomCoordinate == localRoomCoordinate && room != this)
            {
                Debug.Log("THERE IS A ROOM THAT HAS THE SAME COORDINATE AS THE BOSS ROOM!");
                Destroy(room.gameObject);
            }
            //else
            //{
            //    Debug.Log("There is no room on top of the boss room.");
            //}
        }
    }

    // depending on which door the player entered the room from, change the spawn location of the boss to prevent it from spawning right next to player
    public void UpdateBossSpawnLocation()
    {
        // only update boss spawn location if the player is entering the boss room
        if (levelManager.GetCurrentRoom() != this)
            return;

        if (doorEnteredFrom == topDoor)
        {
            return;
        }
            

        else if (doorEnteredFrom == leftDoor)
        {
            spawnLocationsOfThisLevel.Remove(leftSpawnLocation);
        }
            

        else if (doorEnteredFrom == rightDoor)
        {
            spawnLocationsOfThisLevel.Remove(rightSpawnLocation);
        }

        else if (doorEnteredFrom == bottomDoor)
        {
            int randomSpawn = Random.Range(0, 2);
            spawnLocationsOfThisLevel.Remove(middleSpawnLocation);
        }
            
    }

    public Transform GetCenterOfRoom()
    {
        return centerOfRoom;
    }
    //public List<Transform> GetItemDisplayTransformList()
    //{
        //return itemDisplayList;
    //}

    //public int GetNumberOfItems()
    //{
        //return numberOfItems;
    //}

    //public void ModifyNumberOfItems(int amount)
    //{
        //numberOfItems += amount;
    //}

    /*
    IEnumerator SpawnBossRoom()
    {
        while (levelManager.dungeonGenerationState == LevelManager.GenerationProgress.incomplete)
        {
            //Debug.Log("Hi");
            yield return null;
        }
            

        //int randomIndex = Random.Range(0, levelManager.allBossRooms.Length);

        if(levelManager.numberOfSpawnedBossRooms < levelManager.maxNumberOfBossRooms)
            ReplaceRoom();

        //positionToSpawnAt = ReplaceRoom();

        //GameObject bossRoom = Instantiate(levelManager.allBossRooms[randomIndex], positionToSpawnAt, new Quaternion());
    }

    private void ReplaceRoom()
    {
        int randomIndex = Random.Range(0, levelManager.allBossRooms.Length);

        BaseRoom roomToReplace = levelManager.spawnedRooms[levelManager.spawnedRooms.Count - 1];

        Instantiate(levelManager.allBossRooms[randomIndex], roomToReplace.transform.position, new Quaternion());

        roomToReplace.gameObject.SetActive(false);

        Debug.Log("Boss room instaniated!");

        levelManager.numberOfSpawnedBossRooms

    }
    */
}
