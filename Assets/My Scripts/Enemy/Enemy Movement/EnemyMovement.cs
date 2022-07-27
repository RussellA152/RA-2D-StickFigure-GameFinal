using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//This script will contain states for the AI (ex: Idle, Hostile, Death)
//It also controls the values of variables inside of the enemy pathfinding scripts 

// EnemyMovement.cs requires the GameObject to have a Rigidbody and the AI pathfinding components
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
public class EnemyMovement : MonoBehaviour
{

    [Header("Required Components")]
    [SerializeField] private AIPath aiPath;

    [SerializeField] private AIDestinationSetter destinationSetter; //destination setter script inside of enemy
    private Vector3 startingPosition; //the position of wherever the enemy spawned at

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    private float enemyMass; //the mass value of this enemy's rigidbody (DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyWalkingSpeed; // the walking speed of this enemy (using the aiPathing) (DERIVED FROM SCRIPTABLEOBJECT)

    private float followRange; //how far enemy can be to follow player
    private float minimumDistanceY = 1.5f; //how much distance between enemy and player allowed until enemy's auto-repath is turned off (prevents enemies from flying towards player)

    [Header("Target Properties")]
    [SerializeField] private Transform targetTransform; //the target that the enemy will path towards

    [Header("Sprite Direction Properties")]
    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction
    [SerializeField] private bool enemyFacingRight; //is the enemy facing the right direction? true if so, false if facing left

    private Rigidbody2D rb;

    [Header("Sprite Flipping Properties")]
    private bool canFlip = true; // is the enemy allow to turn around?
    private bool flipCoroutineStarted = false; //has the coroutine for sprite flipping started?

    
    private float curFreezeTime = 0f;
    private float maxFreezeTime = 1f;

    private float lerpTimer = 0;
    private float lerpTimerDivider = 10f;

    public bool isMoving = false; //is this AI stuck and can't move?

    private Vector3 curPos;
    private Vector3 lastPos;

    private void Start()
    {
        //set target as enemy's destintation
        SetNewTarget(targetTransform);
    }

    private void Update()
    {
        //Debug.Log("My desired velocity is " + aiPath.desiredVelocity);
        //Debug.Log("reached destination = " + aiPath.reachedEndOfPath);
        


        //if the Y distance between the enemy and player gets too high..
        // then the enemy will not re-calculate their pathfinding (they walk towards the last spot where the player was standing)
        if (Mathf.Abs(transform.position.y - targetTransform.position.y) >= minimumDistanceY)
        {
            aiPath.autoRepath.mode = AutoRepathPolicy.Mode.Never; 
        }
        //when the y distance between the enemy and player is close enough
        // then the enemy can dynamically calculate their pathfinding again
        else
        {
            aiPath.autoRepath.mode = AutoRepathPolicy.Mode.Dynamic;

        }
        // always check if enemy needs to flip their sprite/ turn around
        FlipSpriteAutomatically();
    }

    public void CheckIfFrozen(EnemyController.EnemyState state)
    {
        lerpTimer += Time.deltaTime;

        //the current position of this enemy
        curPos = this.transform.position;

        //if the enemy hasn't move positions
        //and they are in the chase target state
        //then they are not moving and we need to start the freeze timer
        if (curPos == lastPos && state == EnemyController.EnemyState.ChaseTarget)
        {
            isMoving = false;
            curFreezeTime += Time.deltaTime;
        }
        //otherwise they are moving and the freeze timer should reset
        else
        {
            isMoving = true;
            curFreezeTime = 0;
        }

        //set the latest position to the current positon
        lastPos = curPos;

        //if the enemy hasn't been moving for some time (about 3 seconds)
        //give them a small nudge to "unstuck" them
        if(curFreezeTime > maxFreezeTime)
        {
            if (enemyFacingRight)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x + 0.5f, transform.position.y), lerpTimer / lerpTimerDivider);

            }
                            
            else
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x - 0.5f, transform.position.y), lerpTimer / lerpTimerDivider);
            }
                

            curFreezeTime = 0;
            isMoving = true;
            Debug.Log("Give me a nudge!");
        }
    }

    //enemy will automatically turn around when moving left or right
    // can only automatically turn around if canMove and canFlip are true
    private void FlipSpriteAutomatically()
    {
        //enemy must be allowed to move to flip sprite
        //if enemy is moving right... flip sprite to face the right direction
        if (aiPath.desiredVelocity.x >= 0.01f && aiPath.canMove && canFlip)
        {
            transform.localScale = facingRightVector;
            enemyFacingRight = true;
        }
        // if enemy is moving left.. flip sprite to face left direction
        else if (aiPath.desiredVelocity.x <= -0.01f && aiPath.canMove && canFlip)
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
    public void SetNewTarget(Transform target)
    {
        destinationSetter.SetTarget(target);
    }

    //canMove being true means the AI is allowed to pathfind (enemy is not affected by forces when canMove is true)
    public void AllowMovement()
    {
        
        aiPath.SetAICanMove(true);
    }

    //canMove being false means the AI is not allowed to pathfind (enemy is NOW affected by forces)
    //we call this function when enemy is hit by an attack so that they are affected by the attack power of attacks
    public void DisableMovement()
    {
        
        aiPath.SetAICanMove(false);
        
    }
    //enemy will come to a stop if shouldStop is true, but is allowed to pathfind still (not able to be affected by forces in this mode)
    // if shouldStop is false, the enemy will resume their movement
    public void StopMovement(bool shouldStop)
    {
        aiPath.isStopped = shouldStop;
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

    //return the following range of this enemy
    public float GetEnemyFollowRange()
    {
        return followRange;
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

        rb = GetComponent<Rigidbody2D>();

        //set basic values equal to the ScriptableObject's values
        if (enemyScriptableObject != null)
        {
            enemyMass = enemyScriptableObject.rbMass;
            enemyWalkingSpeed = enemyScriptableObject.walkingSpeed;

            //set enemy's sprite equal to ScriptableObject's sprite
            //spriteRenderer.sprite = enemyScriptableObject.sprite;

            followRange = enemyScriptableObject.followRange;
        }
        else
        {
            Debug.Log("This enemy doesn't have a scriptable object! Inside Movement Script*");
        }

        //set enemy's rigidbody mass equal to enemyMass
        rb.mass = enemyMass;

        //set enemy's maxSpeed to enemyWalkingSpeed;
        aiPath.maxSpeed = enemyWalkingSpeed;
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

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}