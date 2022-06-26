using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtAnimationBehavior : StateMachineBehaviour
{

    private PlayerComponents playerComponentScript; //we will use the player component script in order to disable movement

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //disable all types of movement or attack when hurt (jumping, walking, sliding, rolling, and all attacks)
        playerComponentScript.SetCanMove(false);

        playerComponentScript.SetCanAttack(false);

        playerComponentScript.SetCanBackAttack(false);

        playerComponentScript.SetCanFlip(false);

        playerComponentScript.SetCanSlide(false);

        playerComponentScript.SetCanRoll(false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerComponentScript.SetCanMove(true);

        playerComponentScript.SetCanAttack(true);

        playerComponentScript.SetCanBackAttack(true);

        playerComponentScript.SetCanFlip(true);

        playerComponentScript.SetCanSlide(true);

        playerComponentScript.SetCanRoll(true);
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