using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is responsible for changing and setting the current state of the enemy this script is placed on
// we will invoke methods from our referenced scripts when states change
public class EnemyStateManager : MonoBehaviour
{
    public enum EnemyState
    {
        Idle, //enemy is staying still

        Roaming, //enemy is walking around back and forth

        ChaseTarget, // enemy is moving towards their target

        Hurt, // enemy is hurt (flinching or knocked back)

        Attacking, // enemy is trying to attack player

        Dead

    }

    private EnemyMovement enemyMoveScript;
    private EnemyHealth enemyHpScript;

    private float attackRange = 10f; //temporary variable, put this inside of a EnemyAttack

    private bool hurtCoroutineStarted; // TEMPORARY VARIABLE, REMOVE LATER

    private EnemyState currentState;

    private void OnEnable()
    {
        currentState = EnemyState.Idle;
    }

    private void Start()
    {
        enemyMoveScript = GetComponent<EnemyMovement>();
        enemyHpScript = GetComponent<EnemyHealth>();
    }

    private void Update()
    {

        switch (currentState)
        {
            default:

            case EnemyState.Idle:

                EnemyIdleBehavior();

                break;

            //doesn't do anything for now
            case EnemyState.Roaming:
                break;

            case EnemyState.ChaseTarget:

                EnemyChaseBehavior();

                break;

            case EnemyState.Attacking:

                EnemyAttackingBehavior();
                break;

            case EnemyState.Hurt:
                EnemyHurtBehavior();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    private void EnemyIdleBehavior()
    {
        //don't let enemy move in idle state
        //goes to EnemyMovement script and sets canMove to false (allowing enemy to walk to enemy)
        enemyMoveScript.DisableMovement();

        //find enemy's current target
        Transform target = enemyMoveScript.GetEnemyTarget();
        float followRange = enemyMoveScript.GetEnemyFollowRange();

        //Debug.Log("Enemy is idle!");

        //check distance between enemy and player
        //if enemy is close to player, chase them (within follow range)

        if (Vector2.Distance(transform.position, target.position) <= followRange && Vector2.Distance(transform.position, target.position) > attackRange)
        {
            currentState = EnemyState.ChaseTarget;
        }

        // if the player is within attacking range, attack them instead of chase
        else if (Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }

    }

    private void EnemyChaseBehavior()
    {
        //goes to EnemyMovement script and sets canMove to true (allowing enemy to walk to enemy)
        enemyMoveScript.AllowMovement();


        //find enemy's current target
        Transform target = enemyMoveScript.GetEnemyTarget();
        float followRange = enemyMoveScript.GetEnemyFollowRange();

        //Debug.Log("Enemy is chasing!");

        //check distance between enemy and player
        //if enemy and player are too far from each other, return to idle
        // also check if player is within the enemy's attacking range, if so, change state into Attacking
        if (Vector2.Distance(transform.position, target.position) > followRange && Vector2.Distance(transform.position, target.position) > attackRange)
        {
            currentState = EnemyState.Idle;
        }
        else if(Vector2.Distance(transform.position, target.position) <= attackRange)
        {
            currentState = EnemyState.Attacking;
        }
    }

    private void EnemyAttackingBehavior()
    {
        //check if enemy is within attacking range, disable their movement and play attacks or something
        //probably make an enemy attack script

        //don't let enemy move when trying to attack
        //goes to EnemyMovement script and sets canMove to false
        enemyMoveScript.DisableMovement();


        //find enemy's current target
        Transform target = enemyMoveScript.GetEnemyTarget();
        float followRange = enemyMoveScript.GetEnemyFollowRange();



        //check distance between enemy and player
        //if player is no longer in attacking range, but still in follow range, follow them
        if (Vector2.Distance(transform.position, target.position) <= followRange && Vector2.Distance(transform.position, target.position) > attackRange)
        {
            currentState = EnemyState.ChaseTarget;
        }
        //if player is too far from enemy, return to idle
        else if (Vector2.Distance(transform.position, target.position) > attackRange && Vector2.Distance(transform.position, target.position) > followRange)
        {
            currentState = EnemyState.Idle;
        }

        //Debug.Log("Enemy tries to attack!");

        //check when done attacking so we can go back to chase target or something?
    }

    private void EnemyHurtBehavior()
    {
        enemyMoveScript.DisableMovement();

        //disable flip here too

        if(!hurtCoroutineStarted)
            StartCoroutine(GetBackUp());

    }

    //sets enemy's current state to Hurt
    public void ChangeEnemyStateToHurt()
    {
        currentState = EnemyState.Hurt;
    }
    
    //sets enemy's current state to Idle
    public void ChangeEnemyStateToIdle()
    {
        currentState = EnemyState.Idle;
    }

    //TESTING HURT BEHAVIOR
    IEnumerator GetBackUp()
    {
        Debug.Log("Enemy hurt coroutine!");
        hurtCoroutineStarted = true;
        yield return new WaitForSeconds(3f);
        currentState = EnemyState.Idle;
        hurtCoroutineStarted = false;
        
    }
}

