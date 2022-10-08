using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance; 

    public List<EnemyScriptableObject> enemyScriptableObjects = new List<EnemyScriptableObject>();

    [SerializeField] private ItemManager itemManager;

    //public event Action onEnemyEvent;
    public event Action onBossKill; // an action that will occur when the boss is killed
    public event Action onEnemyKill; // an action that will occur when an enemy is killed (may include the boss)

    //private Vector2 recentDeadEnemyPosition; // the vector2 position of the most recently killed enemy
                                             // we will use this value to give to ItemManager so we can spawn an instant item when an enemy dies

    private void Awake()
    {
        if (enemyManagerInstance != null && enemyManagerInstance != this)
        {
            Destroy(this);
        }
        else
        {
            enemyManagerInstance = this;
        }
    }

    void Update()
    {

    }

    private void Start()
    {
        //onEnemyKill += EnemyItemDrop;
    }

    private void OnDisable()
    {
        //onEnemyKill -= EnemyItemDrop;
    }

    //public void RandomEventSystem()
    //{
    //if(onEnemyEvent != null)
    //{
    //onEnemyEvent();
    //}
    //}

    public void BossKilledEventSystem()
    {
        if (onBossKill != null)
            onBossKill();
    }
    public void EnemyKilledEventSystem()
    {
        if (onEnemyKill != null)
            onEnemyKill();
    }

    //private void EnemyItemDrop()
    //{
        // tell ItemManager to spawn an instant item at the position of the most recently killed enemy
        //itemManager.SpawnInstantItemAtLocation(recentDeadEnemyPosition);
    //}

    // set recentDeadEnemyPosition equal to the vector2 passed in
    //public void SetDeadEnemyPosition(Vector2 newPosition)
    //{
        //recentDeadEnemyPosition = newPosition;
    //}

    public EnemyScriptableObject GiveScriptableObject()
    {
        //picks a random number ranging from 0 to the enemyScriptableObject's list size
        //SHOULD HAVE HIGHER OR LOWER CHANCES TO SPAWN certain enemies
        int randomIndex = Random.Range(0, enemyScriptableObjects.Count);

        return enemyScriptableObjects[randomIndex];
    }

}
