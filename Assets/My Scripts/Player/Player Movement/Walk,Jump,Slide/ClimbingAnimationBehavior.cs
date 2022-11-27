using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbingAnimationBehavior : StateMachineBehaviour
{
    private PlayerComponents playerComponentScript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        playerComponentScript.SetCanJump(false);
        playerComponentScript.SetCanRoll(false);
        playerComponentScript.SetCanAttack(false);
        playerComponentScript.SetCanSlide(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerComponentScript.SetCanJump(true);
        playerComponentScript.SetCanRoll(true);
        playerComponentScript.SetCanAttack(true);
        playerComponentScript.SetCanSlide(true);
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
