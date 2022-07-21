using System.Collections;
using System.Collections.Generic;
using UnityEngine.Pool;
using UnityEngine;

//This class is responsible for spawning in AI and enemies whenever the player enters a new level/area (using event system for that)
//the idea is that the levels will not be new scenes, so we should re-use enemies that were killed/ignored
//when enemies spawn, they should be given a scriptable object to classify them
//enemies should not respawn in the same level, but only when entering a new area
public class AISpawner : MonoBehaviour
{
    //[SerializeField] private LevelManager levelManager;

    ObjectPool<EnemyController> enemyPool; //pool of regular enemies (fast, fat, ranged)
    ObjectPool<EnemyController> bossEnemyPool; //pool of boss enemies (might use?)

    private int defaultCapacity; // the capacity of enemies (not entirely sure what this does)

    [SerializeField] private int amountToSpawn; // the maximum number of enemies
    [SerializeField] private int inReserve;

    [SerializeField] private int inactiveCount; // amount of inactive enemies
    [SerializeField] private int activeCount; // amount of active enemies

    [Header("AI To Be Pooled")]
    [SerializeField] private EnemyController enemyPrefab; //the enemy to pool/spawn


    private void Awake()
    {
        enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, null, true, defaultCapacity, amountToSpawn);

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
        //var enemy = enemyPool.Get();
        LevelManager.instance.onPlayerEnterNewArea += SpawnAI;

    }

    public void SpawnAI()
    {
        //enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, null, true, defaultCapacity, amountToSpawn);
        //FIXME: this code makes it so that enemies will only spawn the "amountToSpawn"
        //will respawn enemies when active count drops below "amountToSpawn" number
        int numberOfEnemiesNeeded = LevelManager.instance.GetCurrentRoom().GetNumberOfEnemiesCanSpawnHere();

        for (int spawnIterator = 0; spawnIterator < numberOfEnemiesNeeded; spawnIterator++)
        {
            var enemy = enemyPool.Get();
        }

        //while (enemyPool.CountActive < amountToSpawn && inReserve > 0)
        //{
            //var enemy = enemyPool.Get();
        //}
    }

    EnemyController CreateEnemy()
    {
        //instantiates a new enemy
        var enemy = Instantiate(enemyPrefab);

        //set the enemy's pool equal to this pool
        enemy.SetPool(enemyPool);

        return enemy;

    }

    // this function performs actions on the enemy when they are spawned in 
    // this is where enemies will receive their scriptable object (to differentiate them *)
    void OnTakeEnemyFromPool(EnemyController enemy)
    {
        BasicDungeon roomToSpawnEnemiesInside = LevelManager.instance.GetCurrentRoom();

        //spawn this enemy in a some location in the area (retrieves spawn location from level manager)
        enemy.gameObject.transform.position = LevelManager.instance.GetCurrentRoom().GiveNewSpawnLocations();

        enemy.gameObject.SetActive(true);

        inReserve--;
    }

    //this function performs actions on the enemy when they return to the pool
    void OnReturnEnemyToPool(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);

        inReserve++;
    }

}
