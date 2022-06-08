using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

//This script will contain states for the AI (ex: Idle, Hostile, Death)
//It also controls the values of variables inside of the enemy pathfinding scripts 
public class EnemyController : MonoBehaviour
{
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

    private bool canMove = false; // variable to see if enemy is allowed to move (will be set to true or false depending on if enemy is alive or attacked)
    //canMove being false also means enemy is allowed to be affected by attack forces because the pathfinding is basically turned off



    private void Start()
    {
        targetTransform = GameObject.Find("Player").transform;

        aiPath = GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();

        destinationSetter = GetComponent<AIDestinationSetter>();

        destinationSetter.target = targetTransform;


    }

    private void Update()
    {
        //updating the canMove variable inside of AIPath script
        aiPath.canMove = canMove;

        if(Vector2.Distance(transform.position,targetTransform.position) > hostileRange)
        {
            canMove = false;
            //set enemy state to idle (should set canMove to false)
        }
        else
        {
            canMove = true;

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

    public void SetEnemyCanMove(bool boolean)
    {
        canMove = boolean;
    }


}
    /*
    private Rigidbody2D rb;

    [SerializeField] private Transform target;

    [SerializeField] private float speed = 200f;

    [SerializeField] private float nextWayPointDistance = 3f;

    private Path path;
    private int currentWayPoint = 0;
    private bool reachEndOfPath = false;

    private Seeker seeker;

    //private AIPath aiPath;


    private void Start()
    {
        //Application.targetFrameRate = 30;

        //aiPath = GetComponent<AIPath>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        speed = speed * 1000;

        //use CancelInvoke when enemy goes idle, or if they are in hostile distance?
        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    private void Update()
    {
        //Debug.Log(currentWayPoint);

        //UpdatePath();
        //Debug.Log("RB Velocity : " + rb.velocity);
    }


    private void UpdatePath()
    {
        
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target.position, OnPathComplete);

        Debug.Log("INVOKE REPEAT!");
    }

    private void FixedUpdate()
    {

        //if enemy has no path, do nothing
        if (path == null)
            return;

        // if currentWayPoint is greater or equal to the total amount of waypoints along the path, then enemy reached their destination
        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachEndOfPath = true;
            //rb.velocity = Vector2.zero;
            //CancelInvoke();
            return;
        }
        else
        {
            reachEndOfPath = false;

        }
        //path.vectorPath[currentWayPoint] gives us the precision of our current way point
        //direction is a Vector2 that points from the enemy position to the next way point
        //normalized makes sure the length of this direction is always 1
        Vector2 direction = ((Vector2) path.vectorPath[currentWayPoint] - rb.position).normalized;

        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        //check when enemy needs to flip their sprite
        FlipSprite(force);
    }


    void OnPathComplete(Path p)
    {
        //if the path didn't get errors, set path to newly generated path, and then reset progress on path
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }


    private void FlipSprite(Vector2 force)
    {
        //if enemy is moving right... flip sprite to face the right direction
        if(rb.velocity.x >= 0.01f && force.x > 0f)
        {
            Vector3 theScale =  new Vector3(-1f, 1f, 1f);
            transform.localScale = theScale;
        }
        // if enemy is moving left.. flip sprite to face left direction
        else if(rb.velocity.x <= -0.01f && force.x < 0f)
        {
            Vector3 theScale = new Vector3(1f,1f,1f);
            transform.localScale = theScale;
        }
    }

}

    */
