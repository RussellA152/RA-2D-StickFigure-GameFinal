using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is responsible for changing and setting the current state of the enemy this script is placed on
// we will invoke methods from our referenced scripts when states change
public class EnemyController : MonoBehaviour
{
    private EnemyMovement enemyMoveScript; //every enemy will have a movement script

    private EnemyHealth enemyHpScript; // every enemy will have a health script

    private IEnemyAttacks enemyAttackScript; //Every enemy will have an attacking script, but might not share the exact same behavior, so we will use an interface 

    private Transform target; //enemy's target that they will chase and attack

    private float distanceFromTarget; // the distance from enemy and player

    private float followRange; //range that enemy can chase target (taken from enemymovescript)
    private float attackRange; //range that enemy can attack target (taken from enemyattack script)

    private bool hurtCoroutineStarted; // TEMPORARY VARIABLE, REMOVE LATER

    private EnemyState currentState;

    public enum EnemyState
    {
        Idle, //enemy is staying still

        Roaming, //enemy is walking around back and forth

        ChaseTarget, // enemy is moving towards their target

        Hurt, // enemy is hurt (flinching or knocked back)

        Attacking, // enemy is trying to attack player

        Dead
    }

    private void OnEnable()
    {
        currentState = EnemyState.Idle;

        //Get the enemy's movement script 
        enemyMoveScript = GetComponent<EnemyMovement>();

        //Get the enemy's movement script
        enemyHpScript = GetComponent<EnemyHealth>();

        //Get the enemy's attacking script (has a type of IEnemyAttacks)
        enemyAttackScript = GetComponent<IEnemyAttacks>();

        //tell other scripts to get their values
        SetUpEnemyConfiguration();


        //retrieve the enemy's current target from movement script
        target = enemyMoveScript.GetEnemyTarget();

        //retrieve the following range from the movement script
        followRange = enemyMoveScript.GetEnemyFollowRange();

        //retrieve the attacking range from the attacking script
        attackRange = enemyAttackScript.GetAttackRange();
    }

    private void Update()
    {
        //calculate the distance between enemy and player
        // we will need this value to determine when to switch to idle, attacking, or chasing
        distanceFromTarget = Vector2.Distance(transform.position, target.position);

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

        //check distance between enemy and player
        //if enemy is close to player, chase them (within follow range)

        if (distanceFromTarget <= followRange && distanceFromTarget > attackRange)
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

        //check distance between enemy and player
        //if enemy and player are too far from each other, return to idle
        // also check if player is within the enemy's attacking range, if so, change state into Attacking
        if (distanceFromTarget > followRange && Vector2.Distance(transform.position, target.position) > attackRange)
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

        //Debug.Log("Attack state!");


        //check distance between enemy and player
        //if player is no longer in attacking range, but still in follow range, follow them
        if (distanceFromTarget <= followRange && distanceFromTarget > attackRange)
        {
            currentState = EnemyState.ChaseTarget;
        }
        //if player is too far from enemy, return to idle
        else if (distanceFromTarget > attackRange && distanceFromTarget > followRange)
        {
            currentState = EnemyState.Idle;
        }

        enemyAttackScript.AttackTarget(target);

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
        Debug.Log("Enemy hurt coroutine STARTED!");
        hurtCoroutineStarted = true;
        yield return new WaitForSeconds(3f);
        currentState = EnemyState.Idle;
        hurtCoroutineStarted = false;

        Debug.Log("Enemy hurt coroutine FINISHED!");

    }

    //we will tell the other scripts to begin setting up inside of EnemyController.cs because we need an order of execution
    private void SetUpEnemyConfiguration()
    {
        enemyMoveScript.SetupEnemyMovementFromConfiguration();

        enemyHpScript.SetupEnemyHealthFromConfiguration();

        enemyAttackScript.SetUpEnemyAttackConfiguration();
    }
}

