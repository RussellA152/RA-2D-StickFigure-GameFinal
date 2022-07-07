using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance; 

    public List<EnemyScriptableObject> enemyScriptableObjects = new List<EnemyScriptableObject>();

    public event Action onEnemyEvent;

    private void Awake()
    {
        enemyManagerInstance = this;
    }

    void Update()
    {

    }

    public void RandomEventSystem()
    {
        if(onEnemyEvent != null)
        {
            onEnemyEvent();
        }
    }

    public EnemyScriptableObject GiveScriptableObject()
    {
        //picks a random number ranging from 0 to the enemyScriptableObject's list size
        //SHOULD HAVE HIGHER OR LOWER CHANCES TO SPAWN certain enemies
        int randomIndex = Random.Range(0, enemyScriptableObjects.Count);

        return enemyScriptableObjects[randomIndex];
    }

}
