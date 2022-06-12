using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//This script will contain states for the AI (ex: Idle, Hostile, Death)
//It also controls the values of variables inside of the enemy pathfinding scripts 

// EnemyDamageHandler requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [HideInInspector]
    public float enemyHealth; //the health value of this enemy (DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyMass; //the mass value of this enemy's rigidbody (DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyWalkingSpeed; // the walking speed of this enemy (using the aiPathing) (DERIVED FROM SCRIPTABLEOBJECT)
    private Sprite enemySprite; //the sprite of this enemy WHEN SPAWNED (DERIVED FROM SCRIPTABLEOBECT)


    [Header("Pathing Attributes")]
    [SerializeField] private float hostileRange; //how far enemy can be to follow player

    [Header("Target Properties")]
    [SerializeField] private Transform targetTransform; //the target that the enemy will path towards

    [Header("Sprite Direction Vectors")]
    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction

    private Rigidbody2D rb;

    private AIPath aiPath;

    private AIDestinationSetter destinationSetter; //destination setter script inside of enemy


    private void OnEnable()
    {
        //we are setting up the values for the enemy inside of OnEnable because we will use object pooling for killing enemies
        SetupEnemyFromConfiguration();

    }

    private void Start()
    {
        // find target (the Player)
        targetTransform = GameObject.Find("Player").transform;

        //set target as enemy's destintation
        destinationSetter.SetTarget(targetTransform);

        //Debug.Log("My health is " + enemyHealth);
        //Debug.Log("My mass is " + enemyMass);
        //Debug.Log("My walking speed is " + enemyWalkingSpeed);

    }

    private void Update()
    {

        //check if enemy was killed..
        CheckHealth();

        if(Vector2.Distance(transform.position,targetTransform.position) > hostileRange)
        {
            aiPath.SetAICanMove(false);
            //set enemy state to idle (should set canMove to false)
        }
        else
        {
            aiPath.SetAICanMove(true);

        }
        CheckStoppingDistance();

        //check if enemy needs to flip their sprite
        FlipSprite();
    }

    private void FlipSprite()
    {
        //if enemy is moving right... flip sprite to face the right direction
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = facingRightVector;
}
        // if enemy is moving left.. flip sprite to face left direction
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = facingLeftVector;
        }
    }

    private void CheckStoppingDistance()
    {
        //if enemy gets too close to player, set canMove to false (this will allow enemy to be affected by forces)
        //if (Vector2.Distance(transform.position, targetTransform.position) <= aiPath.endReachedDistance)
        //{
            //canMove = false;
            //set enemy state to idle (should set canMove to false)
        //}
        //else
        //{
            //canMove = true;

        //}
    }

    // can be virtual, but it won't be for now... (if virtual, then enemies could override this function for subtyping)
    //sets all base values equal to 
    public void SetupEnemyFromConfiguration()
    {
        //cache components required (enemies spawned in during runtime will need this)
        aiPath = GetComponent<AIPath>();
        destinationSetter = GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        enemySprite = GetComponentInChildren<SpriteRenderer>().sprite;

        //set basic values equal to the ScriptableObject's values
        enemyHealth = enemyScriptableObject.maxHealth;
        enemyMass = enemyScriptableObject.rbMass;
        enemyWalkingSpeed = enemyScriptableObject.walkingSpeed;

        //set enemy's sprite equal to ScriptableObject's sprite
        enemySprite = enemyScriptableObject.sprite;

        //set enemy's rigidbody mass equal to enemyMass
        rb.mass = enemyMass;
        //set enemy's maxSpeed to enemyWalkingSpeed;
        aiPath.maxSpeed = enemyWalkingSpeed;
    }

    public void CheckHealth()
    {
        //if enemy reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if(enemyHealth <= 0f)
        {
            Debug.Log(this.gameObject.name + " has died!");
            gameObject.SetActive(false);

        }
    }
}