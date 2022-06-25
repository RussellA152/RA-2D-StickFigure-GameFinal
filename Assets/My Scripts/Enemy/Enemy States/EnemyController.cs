using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script is responsible for changing and setting the current state of the enemy this script is placed on
// we will invoke methods from our referenced scripts when states change
//This script also requires the EnemyHealth and EnemyMovement scripts to function
public class EnemyController : MonoBehaviour
{
    [Header("Required Scripts")]
    [SerializeField] private EnemyMovement enemyMoveScript; //every enemy will have a movement script

    [SerializeField] private EnemyHealth enemyHpScript; // every enemy will have a health script

    private IAIAttacks enemyAttackScript; //Every enemy will have an attacking script, but might not share the exact same behavior, so we will use an interface 

    private Transform target; //enemy's target that they will chase and attack

    private float distanceFromTarget; // the distance from enemy and player

    private float followRange; //range that enemy can chase target (taken from enemymovescript)
    private float attackRange; //range that enemy can attack target (taken from enemyattack script)

    private EnemyState currentState;


    [Header("State Transition Timers")]
    //How long will it take for the AI to change from their current state to one of the following states? (might differ with each enemy.. like with faster or slower enemies)
    // ex. changing from idle to chaseTarget might take 0.6 seconds
    [SerializeField] private float idleStateTransitionTimer;
    [SerializeField] private float chaseStateTransitionTimer;
    [SerializeField] private float attackStateTransitionTimer;
    

    private bool stateCooldownStarted = false;


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
        //since the enemyAttackScript implements an interface (*enemy attack behaviors are abstract*) we have to getComponent the script because serializefield doesn't work on interfaces
        enemyAttackScript = GetComponent<IAIAttacks>();

        //enemy is in idle state when spawning in
        currentState = EnemyState.Idle;

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
        //Debug.Log("Enemy state is currently = " + currentState);


        //calculate the distance between enemy and player
        // we will need this value to determine when to switch to idle, attacking, or chasing
        if(target != null)
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
        if (!stateCooldownStarted)
        {
            if (distanceFromTarget <= followRange && distanceFromTarget > attackRange)
            {
                ChangeEnemyState(chaseStateTransitionTimer, EnemyState.ChaseTarget);
            }

            // if the player is within attacking range, attack them instead of chase
            else if (Vector2.Distance(transform.position, target.position) <= attackRange)
            {
                ChangeEnemyState(attackStateTransitionTimer, EnemyState.Attacking);
            }
        }
            

    }

    private void EnemyChaseBehavior()
    {
        //goes to EnemyMovement script and sets canMove to true (allowing enemy to walk to enemy)
        enemyMoveScript.AllowMovement();

        //check distance between enemy and player
        //if enemy and player are too far from each other, return to idle
        // also check if player is within the enemy's attacking range, if so, change state into Attacking

        if (!stateCooldownStarted)
        {
            if (distanceFromTarget > followRange && Vector2.Distance(transform.position, target.position) > attackRange)
            {
                ChangeEnemyState(idleStateTransitionTimer, EnemyState.Idle);
            }
            else if (Vector2.Distance(transform.position, target.position) <= attackRange)
            {
                ChangeEnemyState(attackStateTransitionTimer, EnemyState.Attacking);
            }
        }
        
    }

    private void EnemyAttackingBehavior()
    {
        //check if enemy is within attacking range, disable their movement and play attacks or something
        //probably make an enemy attack script

        //don't let enemy move when trying to attack
        //goes to EnemyMovement script and sets canMove to false
        enemyMoveScript.DisableMovement();

        //check distance between enemy and player
        //if player is no longer in attacking range, but still in follow range, follow them
        //if (distanceFromTarget <= followRange && distanceFromTarget > attackRange)
        //{
            //currentState = EnemyState.ChaseTarget;
        //}
        //if player is too far from enemy, return to idle
        //else if (distanceFromTarget > attackRange && distanceFromTarget > followRange)
        //{
            //currentState = EnemyState.Idle;
        //}

        enemyAttackScript.AttackTarget(target);
    }

    private void EnemyHurtBehavior()
    {
        enemyMoveScript.DisableMovement();

        //disable flip here too

        //if(!hurtCoroutineStarted)
            //StartCoroutine(GetBackUp());

    }

    //this function is meant for when other scripts want to change the enemy's state
    public void ChangeEnemyState(float cooldownTimer,EnemyState state)
    {
        //if there is already a coroutine going, cancel it, then start a new one
        if (stateCooldownStarted)
            StopAllCoroutines();

        StartCoroutine(StateTransitionCooldown(cooldownTimer, state));
    }

    //we will tell the other scripts to begin setting up inside of EnemyController.cs because we need an order of execution
    private void SetUpEnemyConfiguration()
    {
        enemyMoveScript.InitializeMovementProperties();

        enemyHpScript.InitializeHealthProperties();

        enemyAttackScript.InitializeAttackProperties();
    }

    //how long should ai wait until they change to the given state?
    //changing from idle to chase would take about 0.8 seconds so that enemy is not constantly moving (they have brief pauses)
    // a 0 second timer would mean the ai instantly changes to that state
    IEnumerator StateTransitionCooldown(float timeToChangeState, EnemyState state)
    {

        stateCooldownStarted = true;

        //wait a few seconds, then change the state
       yield return new WaitForSeconds(timeToChangeState);

        stateCooldownStarted = false;

        currentState = state;


    }
}

