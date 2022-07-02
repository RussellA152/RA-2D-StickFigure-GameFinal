using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager enemyManagerInstance; 

    public List<EnemyController> enemies = new List<EnemyController>();

    public event Action onEnemyEvent;

    [SerializeField] private float minimumEnemyDistance; // the minimum distance between each enemy (higher values mean enemies should be farther apart from each other)

    [SerializeField] private Transform playerTransform; //we need the player's transform to find which enemy is the closest to the player

    private bool isClosestToPlayer; // true for the enemy that is closest to player

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

    //this function will prevent the enemies from "flocking" or clumping up next to each other to reach the player
    // we will try to add some space between each enemy
    private void PreventFlocking()
    {


        foreach(var enemy in enemies)
        {


           
        }
        
    }
}
