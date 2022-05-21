using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransitionBehavior : StateMachineBehaviour
{
    public string attackName;
    public string heavyAttackName;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AttackController.instance.isAttacking)
        {
            if(attackName != "")
                AttackController.instance.animator.Play(attackName);
        }
        else if (AttackController.instance.isHeavyAttacking)
        {
            if (heavyAttackName != "")
                AttackController.instance.animator.Play(heavyAttackName);
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AttackController.instance.isAttacking)
            AttackController.instance.isAttacking = false;

        if (AttackController.instance.isHeavyAttacking)
            AttackController.instance.isHeavyAttacking = false;
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