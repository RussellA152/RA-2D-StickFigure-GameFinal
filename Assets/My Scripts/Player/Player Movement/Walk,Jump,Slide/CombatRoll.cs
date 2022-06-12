using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoll : StateMachineBehaviour
{

    [Header("Speed Properties")]
    [SerializeField] private float rollDistance; // How far the player will roll

    private bool directionIsRight;

    private PlayerComponents playerCompScript;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //retrieve player components
        playerCompScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //disable player's ability walk & jump & attack during roll
        playerCompScript.SetCanMove(false);
        playerCompScript.SetCanAttack(false);

        rb = playerCompScript.GetRB();
        directionIsRight = playerCompScript.GetPlayerDirection();

        //apply force to Vector2.right or Vector2.left depending on which way player is facing
        if (directionIsRight)
            rb.AddForce(Vector2.right * rollDistance);
        else
            rb.AddForce(Vector2.left * rollDistance);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //allow player is move and attack again
        playerCompScript.SetCanMove(true);
        playerCompScript.SetCanAttack(true);
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
