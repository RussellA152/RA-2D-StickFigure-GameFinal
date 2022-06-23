using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//This script will contain states for the AI (ex: Idle, Hostile, Death)
//It also controls the values of variables inside of the enemy pathfinding scripts 

// EnemyDamageHandler requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{

    private Vector3 startingPosition; //the position of wherever the enemy spawned at


    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    private float enemyMass; //the mass value of this enemy's rigidbody (DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyWalkingSpeed; // the walking speed of this enemy (using the aiPathing) (DERIVED FROM SCRIPTABLEOBJECT)
    private Sprite enemySprite; //the sprite of this enemy WHEN SPAWNED (DERIVED FROM SCRIPTABLEOBECT)


    [Header("Pathing Attributes")]
    [SerializeField] private float followRange; //how far enemy can be to follow player

    [Header("Target Properties")]
    [SerializeField] private Transform targetTransform; //the target that the enemy will path towards

    [Header("Sprite Direction Properties")]
    private bool enemyFacingRight; //is the enemy facing the right direction? true if so, false if facing left
    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction

    private Rigidbody2D rb;

    private AIPath aiPath;

    private AIDestinationSetter destinationSetter; //destination setter script inside of enemy


    private void Start()
    {
        //set target as enemy's destintation
        destinationSetter.SetTarget(targetTransform);

    }

    private void Update()
    {
        //check if enemy needs to flip their sprite
        FlipSprite();
    }

    private void EnemyJump()
    {

    }

    private void FlipSprite()
    {
        //if enemy is moving right... flip sprite to face the right direction
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = facingRightVector;
            enemyFacingRight = true;
        }
        // if enemy is moving left.. flip sprite to face left direction
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = facingLeftVector;
            enemyFacingRight = false;
        }
    }

    public void AllowMovement()
    {
        
        aiPath.SetAICanMove(true);
    }

    public void DisableMovement()
    {
        
        aiPath.SetAICanMove(false);
        
    }


    public Transform GetEnemyTarget()
    {
        return targetTransform;
    }

    public float GetEnemyFollowRange()
    {
        return followRange;
    }

    public bool GetDirection()
    {
        return enemyFacingRight;
    }

    // can be virtual, but it won't be for now... (if virtual, then enemies could override this function for subtyping)
    //sets all base values equal to 
    public void InitializeMovementProperties()
    {
        // find target (the Player)
        targetTransform = GameObject.Find("Player").transform;


        //set starting position to where the enemy spawned
        startingPosition = transform.position;


        //cache components required (enemies spawned in during runtime will need this)
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        //retrieve rigidbody
        rb = GetComponent<Rigidbody2D>();

        enemySprite = GetComponentInChildren<SpriteRenderer>().sprite;

        //set basic values equal to the ScriptableObject's values
        enemyMass = enemyScriptableObject.rbMass;
        enemyWalkingSpeed = enemyScriptableObject.walkingSpeed;

        //set enemy's sprite equal to ScriptableObject's sprite
        enemySprite = enemyScriptableObject.sprite;

        //set enemy's rigidbody mass equal to enemyMass
        rb.mass = enemyMass;

        //set enemy's maxSpeed to enemyWalkingSpeed;
        aiPath.maxSpeed = enemyWalkingSpeed;
    }
}