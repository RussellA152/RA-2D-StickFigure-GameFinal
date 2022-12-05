using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;
using UnityEngine.AI;

//This class is responsible for spawning in AI and enemies whenever the player enters a new level/area (using event system for that)
//the idea is that the levels will not be new scenes, so we should re-use enemies that were killed/ignored
//when enemies spawn, they should be given a scriptable object to classify them
//enemies should not respawn in the same level, but only when entering a new area
public class AISpawner : MonoBehaviour
{
    //[SerializeField] private LevelManager levelManager;

    ObjectPool<EnemyController> enemyPool; //pool of regular enemies (fast, fat, ranged)

    ObjectPool<EnemyController> bossEnemyPool; //pool of boss enemies (might use?)

    [SerializeField] private int enemyDefaultCapacity; // the capacity of enemies (not entirely sure what this does)
    [SerializeField] private int bossDefaultCapacity; // the capacity of enemies (not entirely sure what this does)

    //[SerializeField] private int amountToSpawn; // the maximum number of enemies

    [SerializeField] private int inReserve = 0;
    [SerializeField] private int inactiveCount; // amount of inactive enemies
    [SerializeField] private int activeCount; // amount of active enemies

    [Header("Regular AI To Be Pooled")]
    [SerializeField] private EnemyController enemyPrefab; //the enemy to pool/spawn
    [Header("Boss AI To Be Pooled")]
    [SerializeField] private EnemyController bossEnemyPrefab; //the boss enemy to pool/spawn


    private void Awake()
    {
        enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, null, true, enemyDefaultCapacity);
        bossEnemyPool = new ObjectPool<EnemyController>(CreateBossEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, null, true, bossDefaultCapacity);

        inReserve = enemyDefaultCapacity;

    }

    private void OnDestroy()
    {
        LevelManager.instance.onPlayerEnterNewArea -= SpawnAI;
    }

    private void Update()
    {
        if(enemyPool != null)
        {
            inactiveCount = enemyPool.CountInactive;
            activeCount = enemyPool.CountActive;
        }


        //FIXME: this code makes it so that enemies will only spawn the "amountToSpawn"
        //will respawn enemies when active count drops below "amountToSpawn" number
        //if (enemyPool.CountActive < amountToSpawn && inReserve > 0)
        //{
            //var enemy = enemyPool.Get();
        //}

        
    }


    private void Start()
    {
        LevelManager.instance.onPlayerEnterNewArea += SpawnAI;

    }

    public void SpawnAI()
    {
        // if we're going to the boss room, then spawn a boss instead of regular enemies
        if(LevelManager.instance.GetCurrentRoom().roomType == BaseRoom.RoomType.boss)
        {
            SpawnBossAI();
            return;
        }


        //find the number of enemies the spawner will need to create (depends on the current room)
        int numberOfEnemiesNeeded = LevelManager.instance.GetCurrentRoom().GetNumberOfEnemiesCanSpawnHere();

        if(numberOfEnemiesNeeded <= inReserve)
        {
            for (int spawnIterator = 0; spawnIterator < numberOfEnemiesNeeded; spawnIterator++)
            {
                var enemy = enemyPool.Get();
            }
        }
        else if(numberOfEnemiesNeeded > inReserve)
        {
            for (int spawnIterator = 0; spawnIterator <= inReserve; spawnIterator++)
            {
                var enemy = enemyPool.Get();
            }
        }
        

    }

    public void SpawnBossAI()
    {
        var bossEnemy = bossEnemyPool.Get();
    }

    EnemyController CreateEnemy()
    {
        //instantiates a new enemy
        var enemy = Instantiate(enemyPrefab);

        //set the enemy's pool equal to this pool
        enemy.SetPool(enemyPool);

        return enemy;

    }
    EnemyController CreateBossEnemy()
    {
        //instantiates a new enemy
        var boss = Instantiate(bossEnemyPrefab);

        //set the enemy's pool equal to this pool
        boss.SetPool(bossEnemyPool);

        return boss;

    }

    // this function performs actions on the enemy when they are spawned in 
    // this is where enemies will receive their scriptable object (to differentiate them *)
    void OnTakeEnemyFromPool(EnemyController enemy)
    {

        enemy.gameObject.SetActive(true);

        BaseRoom roomToSpawnEnemiesInside = LevelManager.instance.GetCurrentRoom();

        //retrieve the NavMeshAgent component from the enemy (helps with performance)
        var navMeshAgent = enemy.GetNavMeshAgent();

        //spawn this enemy in a some location in the area (retrieves spawn location from a room)
        //have to use Warp instead of editing the transform.position, otherwise enemies will not spawn in the correct position
        navMeshAgent.Warp(LevelManager.instance.GetCurrentRoom().GiveNewSpawnLocations());

        //enemy.gameObject.transform.position = LevelManager.instance.GetCurrentRoom().GiveNewSpawnLocations();

        //enemy.gameObject.SetActive(true);

        //number of enemies that can spawn in that room should decrease so an excess amount of enemies won't spawn
        roomToSpawnEnemiesInside.DecrementNumberOfEnemiesCanSpawnHere();

        inReserve--;
    }

    //this function performs actions on the enemy when they return to the pool
    void OnReturnEnemyToPool(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);

        inReserve++;
    }

}
