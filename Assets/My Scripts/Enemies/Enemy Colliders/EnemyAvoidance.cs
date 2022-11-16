using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is meant to prevent enemies from clumping and pushing each other to reach their target
//Enemies will stop moving (go to Idle state) when they get too close to other enemies or obstacles
public class EnemyAvoidance : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMoveScript;
    [SerializeField] private EnemyController enemyControllerScript;


    public BoxCollider2D avoidanceCollider;

    private bool turnOffCoroutineStarted = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //OnTriggerStay2D continues to run even when the script component is disabled
        if (!this.enabled)
            return;

        //if colliding with another enemy
        // then force the enemy into idle

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //if the enemy was in the hurt state, don't make them change to idle
            if(enemyControllerScript.GetEnemyState() != EnemyController.EnemyState.Hurt)
                enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Idle);

            if (!turnOffCoroutineStarted)
                StartCoroutine(TurnOffCollider());
        }

        //if (!collision.gameObject.CompareTag("EnemyAvoidanceBox"))
        //{      
       // }
       
    }


    // when this enemy's avoidance box collides with an obstacle or another enemy...
    // turn off their avoidance box (THIS IS to prevent enemy from getting frozen in place when two enemy's avoidance boxes are colliding **)
    // and then wait until the enemy returns to their ChaseTarget state (automatically happens during Idle)..
    // and then re-enable the avoidance collider
    IEnumerator TurnOffCollider()
    {
        //coroutine is running, don't start another
        turnOffCoroutineStarted = true;
        //turn off avoidance box collider
        avoidanceCollider.enabled = false;

        //while the enemy is not in chaseTarget or attacking state, don't re-enable the hitbox
        while (enemyControllerScript.GetEnemyState() != EnemyController.EnemyState.ChaseTarget && enemyControllerScript.GetEnemyState() != EnemyController.EnemyState.Attacking)
            yield return null;

        //re-enable the avoidance box when enemy returns to chaseTarget
        avoidanceCollider.enabled = true;

        turnOffCoroutineStarted = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
