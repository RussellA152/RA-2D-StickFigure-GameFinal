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
    [SerializeField] private LevelManager levelManager;

    ObjectPool<EnemyController> enemyPool; //pool of regular enemies (fast, fat, ranged)
    ObjectPool<EnemyController> bossEnemyPool; //pool of boss enemies (might use?)

    [SerializeField] private int defaultCapacity; // the capacity of enemies (not entirely sure what this does)
    [SerializeField] private int maximumCapacity; // the maximum number of enemies

    [SerializeField] private int inactiveCount; // amount of inactive enemies
    [SerializeField] private int activeCount; // amount of active enemies

    [Header("AI To Be Pooled")]
    [SerializeField] private EnemyController enemyPrefab; //the enemy to pool/spawn


    private void Awake()
    {
        enemyPool = new ObjectPool<EnemyController>(CreateEnemy, OnTakeEnemyFromPool, OnReturnEnemyToPool, null, true, defaultCapacity, maximumCapacity);

    }

    private void Update()
    {
        inactiveCount = enemyPool.CountInactive;
        activeCount = enemyPool.CountActive;

        //if (enemyPool.CountActive < maximumCapacity)
        //{
            //var enemy = enemyPool.Get();
        //}

        
    }


    private void Start()
    {
        //var enemy = enemyPool.Get();
        SpawnAI();
        
    }

    public void SpawnAI()
    {
        while (enemyPool.CountActive < defaultCapacity)
        {
            var enemy = enemyPool.Get();
        }
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
        //spawn this enemy in a random location in some area
        enemy.gameObject.transform.position = ChooseRandomSpawnLocation();

        enemy.gameObject.SetActive(true);

        //Debug.Log("Give me a scriptable object!");
    }

    //this function performs actions on the enemy when they return to the pool
    void OnReturnEnemyToPool(EnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    Vector2 ChooseRandomSpawnLocation()
    {
        return levelManager.GetRandomSpawnLocation();
    }
}
