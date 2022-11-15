using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : BaseRoom
{
    private Vector2 positionToSpawnAt;
    [SerializeField] private Transform centerOfRoom;
    private BaseRoom potentialRoomOnTop;


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
        //itemDisplayTransforms = new Transform[amountOfItemDisplays];
        //StartCoroutine("SpawnBossRoom")

        //localRoomCoordinate = new Vector2(0, 0);
        //transform.position = new Vector2(0f, 0f);
        levelManager.onAllRoomsSpawned += CheckIfRoomIsOnTop;
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
