using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossRoom : BaseRoom
{
    private Vector2 positionToSpawnAt;


    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        roomEnemyCountState = BaseRoom.RoomEnemyCount.cleared;
        roomType = RoomType.boss;

        //StartCoroutine("SpawnBossRoom");
    }
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
