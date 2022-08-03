using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using Pathfinding;

//This script will contain states for the AI (ex: Idle, Hostile, Death)
//It also controls the values of variables inside of the enemy pathfinding scripts 

// EnemyMovement.cs requires the GameObject to have a Rigidbody and the AI pathfinding components
//[RequireComponent(typeof(Rigidbody2D))]
//[RequireComponent(typeof(AIPath))]
//[RequireComponent(typeof(AIDestinationSetter))]
public class EnemyMovement : MonoBehaviour
{

    [Header("Required Components")]
    [SerializeField] private NavMeshAgent agent;
    //[SerializeField] private AIPath aiPath;

    //[SerializeField] private AIDestinationSetter destinationSetter; //destination setter script inside of enemy
    private Vector3 startingPosition; //the position of wherever the enemy spawned at

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    private float enemyMass; //the mass value of this enemy's rigidbody (DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyWalkingSpeed; // the walking speed of this enemy (using the aiPathing) (DERIVED FROM SCRIPTABLEOBJECT)

    private float followRangeX; //how far enemy can be to follow player in x direction
    private float followRangeY; //how far enemy can be to follow player in y direction
    private float minimumDistanceY = 1.5f; //how much distance between enemy and player allowed until enemy's auto-repath is turned off (prevents enemies from flying towards player)
    private bool canAutoSetTarget;
    private bool canSetTemporaryPath = false;

    [Header("Target Properties")]
    [SerializeField] private Transform targetTransform; //the target that the enemy will path towards

    [Header("Sprite Direction Properties")]
    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction
    [SerializeField] private bool enemyFacingRight; //is the enemy facing the right direction? true if so, false if facing left

    [SerializeField] private Rigidbody2D rb;

    [Header("Sprite Flipping Properties")]
    private bool canFlip = true; // is the enemy allow to turn around?
    private bool flipCoroutineStarted = false; //has the coroutine for sprite flipping started?

    //There is a bug where the enemy stops moving permanently unless they move a little
    //these values are needed to prevent the enemy from staying frozen 
    //[Header("Prevent Enemy Freeze Properties")]
    //private float differenceInPosition = 0.01f;
    //private float curFreezeTime = 0f; //how long this enemy has been frozen for
    //private float maxFreezeTime = 0.35f;//how long the enemy can be frozen for until they are given a small push to unfreeze them
    //private float pushAmount = 0.001f; //value added to the enemy's current position when they are stuck/frozen

    private bool isMoving = false; //is this AI moving (if their desired velocity is greater than 0)

    private void Start()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        canAutoSetTarget = true;

        //set target as enemy's destintation
        SetNewTarget(targetTransform.position);

        //lastPos = transform.position;
    }

    private void Update()
    {

        if (Mathf.Abs(agent.desiredVelocity.x) > 0)
            isMoving = true;
        else
            isMoving = false;


        //always check the y distance between the target and enemy
        CheckYDistanceFromTarget();
        
        //enemy's navmeshagent component must be enabled to set target
        //also, the bool "canSetTarget" must be true, which is set to false whenever the y distance between the 
        //player and enemy is too high
        if (agent.enabled && canAutoSetTarget)
            SetNewTarget(targetTransform.position);

        // always check if enemy needs to flip their sprite/ turn around
        FlipSpriteAutomatically();

    }

    private void FixedUpdate()
    {
        
    }

    private void CheckYDistanceFromTarget()
    {
        //if the Y distance between the enemy and player gets too high..
        //then the enemy will not set a destintation to the player's exact position
        //instead, they will walk towards the last spot where the player was standing (around there..)
        if (Mathf.Abs(transform.position.y - targetTransform.position.y) >= minimumDistanceY)
        {
            canAutoSetTarget = false;

            //the enemy will move to the last position the player was at before the y distance between the two grew too high
            //if the player jumps over the enemy, the enemy will most likely move towards the last place the player was at before they jumped
            if (canSetTemporaryPath && agent.enabled && !agent.isStopped)
                SetNewTarget(new Vector3(targetTransform.position.x, targetTransform.position.y - minimumDistanceY, targetTransform.position.z));

            //we set this bool to false so that enemy only sets destination once 
            canSetTemporaryPath = false;
        }

        //when the player and enemy get close to each other again
        //the enemy will be able to set that temporary path again
        else
        {
            canSetTemporaryPath = true;

            //enemy is allowed to set destination each frame again
            canAutoSetTarget = true;

        }
    }

    //enemy will automatically turn around when moving left or right
    // can only automatically turn around if canMove and canFlip are true
    private void FlipSpriteAutomatically()
    {
        //enemy must be allowed to move to flip sprite
        //if enemy is moving right... flip sprite to face the right direction
        if (agent.desiredVelocity.x >= 0.01f && canFlip && isMoving)
        {
            transform.localScale = facingRightVector;
            enemyFacingRight = true;
        }
        // if enemy is moving left.. flip sprite to face left direction
        else if (agent.desiredVelocity.x <= -0.01f && canFlip && isMoving)
        {
            transform.localScale = facingLeftVector;
            enemyFacingRight = false;
        }
    }

    //we can call this function whenever we want to manually flip the sprite, instead of letting AI path determine when to flip
    public void FlipSpriteManually(float flipSpriteTimer)
    {
        //if the AI is already set to flip, don't start the coroutine again
        if(!flipCoroutineStarted)
            StartCoroutine(FlipSpriteManuallyCoroutine(flipSpriteTimer));
    }


    //The AI will change to a new target
    //they will now chase this target and attack this target based on distance
    public void SetNewTarget(Vector3 pathPosition)
    {
        agent.SetDestination(pathPosition);
        //destinationSetter.SetTarget(target);
    }

    //canMove being true means the AI is allowed to pathfind (enemy is not affected by forces when canMove is true)
    public void AllowMovement()
    {
        agent.enabled = true;
    }

    //canMove being false means the AI is not allowed to pathfind (enemy is NOW affected by forces)
    //we call this function when enemy is hit by an attack so that they are affected by the attack power of attacks
    public void DisableMovement()
    {

        agent.enabled = false;

    }
    //enemy will come to a stop if shouldStop is true, but is allowed to pathfind still (not able to be affected by forces in this mode)
    // if shouldStop is false, the enemy will resume their movement
    public void StopMovement(bool shouldStop)
    {
        if(agent.enabled)
            agent.isStopped = shouldStop;
    }
    public void SetCanFlip(bool boolean)
    {
        canFlip = boolean;
    }

    //return the target of this enemy
    public Transform GetEnemyTarget()
    {
        return targetTransform;
    }

    //return the following range x of this enemy
    public float GetEnemyFollowRangeX()
    {
        return followRangeX;
    }
    //return the following range y of this enemy
    public float GetEnemyFollowRangeY()
    {
        return followRangeY;
    }
    
    //return the direction this enemy is facing
    public bool GetDirection()
    {
        return enemyFacingRight;
    }

    // can be virtual, but it won't be for now... (if virtual, then enemies could override this function for subtyping)
    //sets all base values equal to the values inside the scriptable object
    public void InitializeMovementProperties(EnemyScriptableObject scriptableObject)
    {
        enemyScriptableObject = scriptableObject;

        // find target (the Player... ideally) (unfortunately we have to do two gameobject.find)
        if(GameObject.Find("Player") != null)
            targetTransform = GameObject.Find("Player").transform;

        //set starting position to where the enemy spawned
        startingPosition = transform.position;

        //rb = GetComponent<Rigidbody2D>();

        //set basic values equal to the ScriptableObject's values
        if (enemyScriptableObject != null)
        {
            enemyMass = enemyScriptableObject.rbMass;
            enemyWalkingSpeed = enemyScriptableObject.walkingSpeed;

            //set enemy's sprite equal to ScriptableObject's sprite
            //spriteRenderer.sprite = enemyScriptableObject.sprite;

            followRangeX = enemyScriptableObject.followRangeX;
            followRangeY = enemyScriptableObject.followRangeY;
        }
        else
        {
            Debug.Log("This enemy doesn't have a scriptable object! Inside Movement Script*");
        }

        //set enemy's rigidbody mass equal to enemyMass
        rb.mass = enemyMass;

        //set enemy's maxSpeed to enemyWalkingSpeed;
        agent.speed = enemyWalkingSpeed;
        //aiPath.maxSpeed = enemyWalkingSpeed;
    }

    //this coroutine is meant to prevent the enemy from flipping immediately when they need to
    //this timer allows movement actions like the roll not feel to punishing to use
    IEnumerator FlipSpriteManuallyCoroutine(float timeToFlip)
    {
        flipCoroutineStarted = true;

        //while canFlip is false, don't let this enemy turn around (until canFlip is true)
        while (!canFlip)
            yield return null;

        //wait some time.. then allow enemy to turn around
        yield return new WaitForSeconds(timeToFlip);


        //if enemy is facing right direction, turn them left
        if (enemyFacingRight)
        {
            enemyFacingRight = false;
            transform.localScale = facingLeftVector;
        }

        //else if enemy is facing left direction, turn them right
        else if (!enemyFacingRight)
        {
            enemyFacingRight = true;
            transform.localScale = facingRightVector;
        }

        flipCoroutineStarted = false;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}