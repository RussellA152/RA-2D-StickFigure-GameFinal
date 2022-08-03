using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this class is responsible for making an enemy turn around when player is behind them
//basically, there is a collider behind each enemy that when the player touches it, they will turn around
//this way, the enemy won't be stuck attacking while facing the opposite direction of the player

public class EnemyTurnAround : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private EnemyMovement enemyMoveScript;

    private bool isCollidingWithPlayer = false;

    private float turnAroundTimer = 0.5f;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (this.enabled == false)
            return;
        //if player goes behind the enemy and collides with the turn around trigger, then invoke the flip sprite function

        if (collision.gameObject.CompareTag("Player") && !isCollidingWithPlayer)
        {
            enemyMoveScript.FlipSpriteManually(turnAroundTimer);
            isCollidingWithPlayer = true;

        }

        //Debug.Log("Back collider code invoked");
    }

    void Update()
    {
        isCollidingWithPlayer = false;
    }


}

