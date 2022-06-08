using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAttackTransitionBehavior : StateMachineBehaviour
{
    public string backAttackName;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AttackController.instance.isBackAttacking)
        {
            if (backAttackName != "")
                AttackController.instance.animator.Play(backAttackName);
            Debug.Log("BACK ATTACK ANIMATION!");
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        AttackController.instance.isBackAttacking = false;
    }
}
