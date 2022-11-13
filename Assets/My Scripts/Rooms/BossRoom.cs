using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : BaseRoom
{
    private Vector2 positionToSpawnAt;
    [SerializeField] private Transform centerOfRoom;


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
        //StartCoroutine("SpawnBossRoom");
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
