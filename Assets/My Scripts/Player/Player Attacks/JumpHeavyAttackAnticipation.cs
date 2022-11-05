using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpHeavyAttackAnticipation : StateMachineBehaviour
{
    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCan"action" functions
    private PlayerCollisionLayerChange playerCollisionLayerScript;

    private Rigidbody2D rb;

    private bool playerFacingRight; // represents direction player is facing (retrieved from playerComponents script)

    [SerializeField] private float jumpHeightRequirementToAttack; // how high should the player be in the air to do this move?

    [Header("Jolt Force Applied To Player")]
    [SerializeField] private float joltForceX; //determines how far the player will 'jolt' forward in the x-direction when attacking (Should be a high value)
    [SerializeField] private float joltForceY; //determines how far the player will 'jolt' forward in the y-direction when attacking (Should be a high value)

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(animator.transform.)

        //AttackController.instance.star
        //AttackController.instance.isGroundSlamming = true;

        
        playerCollisionLayerScript = animator.transform.GetComponent<PlayerCollisionLayerChange>();

        // Prevent player from colliding with enemy
        playerCollisionLayerScript.SetIgnoreEnemyLayer();

        //retrieve component script
        if (playerComponentScript == null)
            playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        rb = playerComponentScript.GetRB();

        playerComponentScript.SetCanJump(false);

        //retrive which way player is facing
        playerFacingRight = playerComponentScript.GetPlayerDirection();

        if (playerFacingRight)
            rb.AddForce(new Vector2(joltForceX, joltForceY));
        //if player is facing left, then multiply force by negative 1 to prevent player from jolting backwards
        else
            rb.AddForce(new Vector2(-joltForceX, joltForceY));

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //playerCollisionLayerScript.ResetLayer();
        //Debug.Log("Exit jump heavy loop!");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
