using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidance : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMoveScript;
    [SerializeField] private EnemyController enemyControllerScript;


    public BoxCollider2D avoidanceCollider;

    private bool turnOffCoroutineStarted = false;
    private float turnOffColliderTimer = 0.3f;
    
    private float flipTimer = 0.1f;


    private void OnTriggerStay2D(Collider2D collision)
    {
        //OnTriggerStay2D continues to run even when the script component is disabled
        if (!this.enabled)
            return;

        //if colliding with another enemy
        // and NOT colliding with another's avoidance box, (basically enemies are facing each other)
        // then force the enemy into idle

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Idle);

            if (!turnOffCoroutineStarted)
                StartCoroutine(TurnOffCollider());

            //Debug.Log("Stop moving!");
        }

        //if (!collision.gameObject.CompareTag("EnemyAvoidanceBox"))
        //{
            
       // }
       
    }

    //when this enemy's avoidance box collides with an obstacle or another enemy...
    // turn off their avoidance box (THIS IS to prevent enemy from getting frozen in place when two enemy's avoidance boxes are colliding **)
    // and then wait until the enemy returns to their ChaseTarget state (automatically happens during Idle)..
    // and then re-enable the avoidance collider
    IEnumerator TurnOffCollider()
    {
        //coroutine is running, don't start another
        turnOffCoroutineStarted = true;
        //turn off avoidance box collider
        avoidanceCollider.enabled = false;

        //while the enemy is not in chaseTarget state, don't re-enable the hitbox
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
