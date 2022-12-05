using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLand : StateMachineBehaviour
{
    private int isRollingHash; //setting the isRolling parameter to a hash value to save performance

    //[SerializeField] private AudioClip landSound;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isRollingHash = Animator.StringToHash("isRolling");
        //ObjectSounds.instance.PlaySoundEffect(landSound);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // allow player to roll JUST after landing on the ground (if they try to roll)
        if (animator.GetBool(isRollingHash) == true)
        {
            animator.Play("Combat Roll");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
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
