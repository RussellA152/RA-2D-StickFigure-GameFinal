using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.AI;
using Random = UnityEngine.Random;


//This script is responsible for changing and setting the current state of the enemy this script is placed on
// we will invoke methods from our referenced scripts when states change
//This script also requires the EnemyHealth and EnemyMovement scripts to function
public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyState currentState;
    [SerializeField] private NavMeshAgent agent;

    private IObjectPool<EnemyController> myPool;

    [SerializeField] private Animator animator; // every enemy will have an animator, but the animator controller they use may differ

    [Header("Required Scripts")]
    [SerializeField] private EnemyMovement enemyMoveScript; //every enemy will have a movement script
    [SerializeField] private EnemyHealth enemyHpScript; // every enemy will have a health script
    [SerializeField] private EnemyHurt enemyHurtScript; // every enemy will have a hurt script
    [SerializeField] private EnemyScriptableObject enemyScriptableObject; //Every enemy will have an attacking script, but might not share the exact same behavior, so we will use an interface 


    [Header("Enemy v. Player Properties")]
    private Transform target; //enemy's target that they will chase and attack

    private float dropChance; // the chance for this enemy to drop an instant item upon death (retrieved from ScriptableObject)

    //private bool isAggressive; //if the enemy's aggression level has reached the threhold, then set this true
    //[SerializeField] private float aggressionLevel; //how aggressive the enemy currently is (increases and decreases depending on state of enemy)
    //private float aggressionLevelThreshold; //how aggressive the enemy must become to attack the player
    //private float minAggressionLevelIncrease; //the minimum the enemy's aggression will increase when not fighting
    //private float maxAggressionLevelIncrease; //the maximum the enemy's aggression will increase when not fighting
    //private float aggressionLevelIncreaseOnHurt; //how much the enemy's aggression level will increase when hit
    //private float aggressionLevelDecreaseOnAttack; //how much the enemy's aggression level will decrease when attacking

    private float distanceFromTargetX; // the distance from enemy and player in x-axis
    private float distanceFromTargetY; // the distance from enemy and player in y-axis

    private float followRangeX; //how far enemy can be to follow player in x direction
    private float followRangeY; //how far enemy can be to follow player in y direction
    private bool withinFollowRange;

    private float attackRangeX; //range that enemy can attack target (taken from Scriptable Object)
    private float attackRangeY; //range that enemy can attack target (taken from Scriptable Object)
    private bool withinAttackRange;
    private bool isAttacking; //bool representing whether the enemy is finished doing their attack animation

    [Header("State Transition Timers")]
    //How long will it take for the AI to change from their current state to one of the following states? (might differ with each enemy.. like with faster or slower enemies)
    // ex. changing from idle to chaseTarget might take 0.6 seconds
    [SerializeField] private float idleStateTransitionTimer;
    [SerializeField] private float chaseStateTransitionTimer;
    [SerializeField] private float attackStateTransitionTimer;
    private Coroutine stateTransitionCoroutine; //a reference to the state transition cooldown coroutine, we have this so that we can stop the coroutine on command
    private bool stateCooldownStarted = false;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Vector2 minimumVelocityUntilStopped; // how low should this enemy's velocity reached to be considered "stopped"?
    [SerializeField] private float gravityWhenIdle;

    private bool attackOnCooldown = false; //is the enemy on attack cooldown? If so, don't let them attack again

    [Header("Dedicated Room")]
    [SerializeField] private BaseRoom myRoom; //the room this enemy was spawned in

    private int walkingHash;
    private int deadHash;

    private bool isHurt = false;
    private int stoppedHash;

    public enum EnemyState
    {
        Idle, //enemy is staying still

        //Roaming, //enemy is walking around back and forth

        ChaseTarget, // enemy is moving towards their target

        Hurt, // enemy is hurt (flinching or knocked back)

        Attacking, // enemy is trying to attack player

        Dead //enemy is dead -> can't move and return to object pool
    }

    private void OnEnable()
    {
        // when enabled, set layer to "Enemy" (this is because enemies are set to "IgnorePlayer" layer when performing death animation)
        SetLayer("Enemy");

        //if the enemy does not already have a scriptable object attached, give them a random one from the EnemyManager (generates random scriptable object from list)
        if (enemyScriptableObject == null)
            enemyScriptableObject = EnemyManager.enemyManagerInstance.GiveScriptableObject();

        //enemy is in idle state when spawning in
        currentState = EnemyState.Idle;

        //tell other scripts to get their values
        SetUpEnemyConfiguration(enemyScriptableObject);

        //the enemy's room would always be the current room inside of OnEnable
        myRoom = LevelManager.instance.GetCurrentRoom();

        //enemy's aggression level is reset or set to 0 when spawning in
        //aggressionLevel = 0;

    }

    private void Start()
    {
        walkingHash = Animator.StringToHash("Walking");
        stoppedHash = Animator.StringToHash("Stopped");
        deadHash = Animator.StringToHash("Dead");

    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    private void Update()
    {

        //check if the enemy is alive
        //bool isAlive = enemyHpScript.CheckIfAlive();

        //if the enemy is dead, change their parameter "Dead" to true
        //they should not be able to change to another state
        //we won't use the ChangeEnemyState because then the coroutine could be canceled, which would prevent enemy from dying
        //if (!isAlive)
        //animator.SetBool(deadHash, true);


        if (rb.velocity.x <= minimumVelocityUntilStopped.x && rb.velocity.y <= minimumVelocityUntilStopped.y)
            animator.SetBool(stoppedHash, true);
        else
            animator.SetBool(stoppedHash, false);
        /*
        //if the enemy's aggression level becomes higher than the threshold, then set it equal to the threshold
        if (aggressionLevel >= aggressionLevelThreshold)
        {
            aggressionLevel = aggressionLevelThreshold;
            isAggressive = true;
        }
        else
        {
            isAggressive = false;
        }
        */


        //calculate the distance between enemy and player
        //we will need this value to determine when to switch to idle, attacking, or chasing
        if (target != null)
        {
            distanceFromTargetX = Mathf.Abs(transform.position.x - target.position.x);
            distanceFromTargetY = Mathf.Abs(transform.position.y - target.position.y);
        }

        //if enemy is close to player (in x & y direction), they are within attacking range and can change to "attacking" state
        //otherwise, they cannot change to "attacking" state
        if (distanceFromTargetX <= attackRangeX && distanceFromTargetY <= attackRangeY)
            withinAttackRange = true;
        else
            withinAttackRange = false;

        if (distanceFromTargetX <= followRangeX && distanceFromTargetY <= followRangeY)
            withinFollowRange = true;
        else
            withinFollowRange = false;

        //only switch states if we have a target ( if there is no target, just remain in Idle )
        if (target != null)
            switch (currentState)
            {
                default:

                case EnemyState.Idle:
                    EnemyIdleBehavior();

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
                    EnemyDeathBehavior();
                    break;
            }
        else
            currentState = EnemyState.Idle;
    }

    //we will tell the other scripts to begin setting up inside of EnemyController.cs because we need an order of execution
    private void SetUpEnemyConfiguration(EnemyScriptableObject scriptableObject)
    {
        //set the enemy's animator controller to the scriptable object's animator controller
        animator.runtimeAnimatorController = enemyScriptableObject.enemyAnimatorController;

        enemyHpScript.InitializeHealthProperties(scriptableObject);

        enemyMoveScript.InitializeMovementProperties(scriptableObject);

        enemyHurtScript.InitializeEnemyHurt(scriptableObject);

        //retrieve the enemy's current target from movement script
        target = enemyMoveScript.GetEnemyTarget();

        //retrieve the following range from the movement script
        followRangeX = enemyMoveScript.GetEnemyFollowRangeX();
        followRangeY = enemyMoveScript.GetEnemyFollowRangeY();

        //retrieve the attacking ranges from the attacking script
        attackRangeX = enemyScriptableObject.GetAttackRangeX();
        attackRangeY = enemyScriptableObject.GetAttackRangeY();

        dropChance = enemyScriptableObject.dropChance;
        /*
        aggressionLevelThreshold = enemyScriptableObject.GetAggressionLevelThreshold();
        minAggressionLevelIncrease = enemyScriptableObject.GetMinAggressionLevelIncrease();
        maxAggressionLevelIncrease = enemyScriptableObject.GetMaxAggressionLevelIncrease();
        aggressionLevelDecreaseOnAttack = enemyScriptableObject.GetAggressionLevelDecreaseOnAttack();
        aggressionLevelIncreaseOnHurt = enemyScriptableObject.GetAggressionLevelIncreaseOnHurt();
        */
    }

    private void EnemyIdleBehavior()
    {
        isHurt = false;

        //Debug.Log("Idle behavior!");
        animator.SetBool(walkingHash, false);
        enemyMoveScript.DisableMovement();
        //don't let enemy move in idle state
        //goes to EnemyMovement script and sets isStopped to true
        enemyMoveScript.StopMovement(true);

        //the enemy is allowed to turn around when they are idle
        enemyMoveScript.SetCanFlip(true);

        enemyHurtScript.SetIsKnockedDown(false);
        enemyHurtScript.SetRBGravity(gravityWhenIdle);

        //IncreaseAggressionLevelIdle();

        //check distance between enemy and player
        //if enemy is close to player, chase them (within follow range)
        if (!stateCooldownStarted)
        {
            if (withinFollowRange && !withinAttackRange && !isHurt) //&& isAggressive)
            {
                ChangeEnemyState(chaseStateTransitionTimer, EnemyState.ChaseTarget);
            }

            // if the player is within attacking range, attack them instead of chase
            else if (withinAttackRange && !isHurt)// && isAggressive)
            {
                ChangeEnemyState(attackStateTransitionTimer, EnemyState.Attacking);
            }
        }


    }

    private void EnemyChaseBehavior()
    {
        //Debug.Log("Chase behavior!");

        //enemyMoveScript.CheckIfFrozen();

        animator.SetBool(walkingHash, enemyMoveScript.GetIsMoving());

        //goes to EnemyMovement script and sets canMove to true (allowing enemy to walk to enemy)
        //also sets isStopped back to false
        enemyMoveScript.StopMovement(false);

        // the enemy is allowed to turn around when they are chasing their target
        enemyMoveScript.SetCanFlip(true);

        enemyMoveScript.AllowMovement();

        //check distance between enemy and player
        //if enemy and player are too far from each other, return to idle
        // also check if player is within the enemy's attacking range, if so, change state into Attacking
        if (!stateCooldownStarted)
        {
            if (!withinFollowRange && !withinAttackRange)
            {
                ChangeEnemyState(idleStateTransitionTimer, EnemyState.Idle);
            }
            else if (withinAttackRange)
            {
                ChangeEnemyState(attackStateTransitionTimer, EnemyState.Attacking);
            }
        }

    }

    private void EnemyAttackingBehavior()
    {
        //Debug.Log("Attack behavior!");

        //if (isHurt)
        //ChangeEnemyState(0f, EnemyState.Hurt);

        animator.SetBool(walkingHash, false);

        //don't let enemy move when trying to attack
        //goes to EnemyMovement script and sets isStopped to true
        enemyMoveScript.StopMovement(true);

        enemyMoveScript.DisableMovement();

        //the enemy is not allowed to turn around until they return to idle
        enemyMoveScript.SetCanFlip(false);

        //invoke the scriptable object's AttackTarget function (is abstract since enemies might have different attack behaviors)
        //don't let enemy attack if their attack is on cooldown
        if (!attackOnCooldown)
        {
            SetIsAttacking(true);
            enemyScriptableObject.AttackTarget(animator, target);
        }



        if (!stateCooldownStarted && !isAttacking)
        {
            if (withinFollowRange && !withinAttackRange)
            {
                ChangeEnemyState(chaseStateTransitionTimer, EnemyState.ChaseTarget);
            }
            else if (!withinFollowRange && !withinAttackRange)
            {
                ChangeEnemyState(idleStateTransitionTimer, EnemyState.Idle);
            }
        }


    }

    private void EnemyHurtBehavior()
    {
        //IncreaseAggressionLevelHurt();

        isHurt = true;

        animator.SetBool(walkingHash, false);
        //don't let enemy move at all
        enemyMoveScript.DisableMovement();

        //the enemy is not allowed to turn around until they return to idle
        enemyMoveScript.SetCanFlip(false);



    }

    private void EnemyDeathBehavior()
    {
        //Debug.Log("Enemy Died! Inside EnemyController.");

        //when this enemy dies, their room's "numberOfEnemiesAliveHere" should decrease by 1
        if (myRoom != null)
            myRoom.DecrementNumberOfEnemiesAliveInHere();

        // if this enemy should drop an item, tell ItemManager to spawn an instant item at this dead enemy's position (random chance)
        if (Random.value <= dropChance)
        {
            Debug.Log("Drop an item on my dead body!");
            ItemManager.instance.SpawnInstantItemAtLocation(this.transform.position);
        }


        // invoke onEnemyKill eventsystem when any enemy dies
        EnemyManager.enemyManagerInstance.EnemyKilledEventSystem();

        //if this enemy has a pool, return this enemy back to the pool
        if (myPool != null)
        {
            myPool.Release(this);
            //Debug.Log("Return me to the pool!");
        }
        else
        {
            Destroy(gameObject);
            //Debug.Log("Destroy me");
        }


    }




    //this function is meant for when other scripts want to change the enemy's state
    public void ChangeEnemyState(float cooldownTimer, EnemyState state)
    {

        //Debug.Log("Starting transition coroutine");
        //if there is already a coroutine going, cancel it, then start a new one
        if (stateCooldownStarted)
        {
            //cancel the current transition coroutine
            StopCoroutine(stateTransitionCoroutine);
            stateCooldownStarted = false;
        }

        //set this coroutine variable to StartCoroutine()
        //this is so that we can cancel it when we need to
        stateTransitionCoroutine = StartCoroutine(StateTransitionCooldown(cooldownTimer, state));

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
    /*
    private void IncreaseAggressionLevelIdle()
    {
        if (aggressionLevel <= aggressionLevelThreshold)
        {
            // create a random number to increase the enemy's aggression level by
            float aggressionLevelIncrease = Random.Range(minAggressionLevelIncrease, maxAggressionLevelIncrease);
            // add the increase to the aggression level
            aggressionLevel += aggressionLevelIncrease * Time.deltaTime;
        }
    }
    */
    /*
    public void IncreaseAggressionLevelHurt()
    {
        if (aggressionLevel <= aggressionLevelThreshold)
        {
            aggressionLevel += aggressionLevelIncreaseOnHurt;
        }
    }
    /*
    /*

    public void DecreaseAggressionLevelOnAttack()
    {
        if (aggressionLevel >= aggressionLevelDecreaseOnAttack)
            aggressionLevel -= aggressionLevelDecreaseOnAttack;
        else
            aggressionLevel = 0f;
    }
    */

    //this function will start the Attack Cooldown of the enemy
    //it is invoked whenever the enemy is finished attacking
    //also don't start another coroutine if attack cooldown is ongoing
    public void StartAttackCooldown()
    {
        if (!attackOnCooldown)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    public void SetLayer(string layer)
    {
        this.gameObject.layer = LayerMask.NameToLayer(layer);
    }

    //the attack cooldown coroutine needs to be inside EnemyController because we shouldn't change variable values in scriptable objects during runtime
    //also, the attack cooldown is not different between enemies, all enemies will have the same cooldown behavior with exception to the cooldown timer
    public IEnumerator AttackCooldown()
    {
        attackOnCooldown = true;
        //the cooldown timer depends on the scriptable object the enemy has
        yield return new WaitForSeconds(enemyScriptableObject.attackCooldownTimer);
        attackOnCooldown = false;
    }
    //sets this enemy's pool equal to the pool passed into the function (comes from AISpawner)
    public void SetPool(IObjectPool<EnemyController> pool)
    {
        myPool = pool;

        //Debug.Log("My pool is " + myPool);
    }

    public void GiveScriptableObject(EnemyScriptableObject scriptableObject)
    {
        enemyScriptableObject = scriptableObject;
    }

    //return the current state of this enemy
    public EnemyState GetEnemyState()
    {
        return currentState;
    }

    public void SetIsAttacking(bool boolean)
    {
        isAttacking = boolean;
    }

    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }


}