using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance; 

    public List<EnemyController> enemies = new List<EnemyController>();

    public event Action onEnemyEvent;

    private void Awake()
    {
        enemyManagerInstance = this;
    }

    void Start()
    {
        //InvokeRepeating("PreventFlocking", 0.1f, 0.5f);
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

}
