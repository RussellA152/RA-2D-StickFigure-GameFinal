using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAvoidance : MonoBehaviour
{
    [SerializeField] private EnemyMovement enemyMoveScript;
    [SerializeField] private EnemyController enemyControllerScript;

    public BoxCollider2D collider;
    private bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //if colliding with another enemy
        // and NOT colliding with another's avoidance box, (basically enemies are facing each other)
        // then force the enemy into idle

        if (!collision.gameObject.CompareTag("EnemyAvoidanceBox"))
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //enemyMoveScript.DisableMovement();
                enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Idle);
                Debug.Log("Stop moving!");
            }
        }
        //if colliding with another's avoidance box, then turn around
        // since enemy has turned around, their OnTriggerExit will invoke, which prevent them from getting stuck facing each other
        // however, enemies can get stuck in a flip loop if the avoidance box is too big...
        else if(collision.gameObject.CompareTag("EnemyAvoidanceBox"))
        {
            enemyMoveScript.FlipSpriteManually(0.1f);

            //prevent enemy from getting stuck in a flip loop by turning off the avoidance box for a bit
            if (!started)
                StartCoroutine(TurnOff());

        }
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //when the enemy is no longer colliding with another enemy
        // change the enemy's state to be chaseTarget
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            //enemyMoveScript.AllowMovement();
            enemyControllerScript.ChangeEnemyState(0.4f, EnemyController.EnemyState.ChaseTarget);

            
        }
    }

    IEnumerator TurnOff()
    {
        started = true;
        collider.enabled = false;
        yield return new WaitForSeconds(.3f);
        collider.enabled = true;
        started = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
