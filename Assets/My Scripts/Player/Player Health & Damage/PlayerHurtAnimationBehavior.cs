using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtAnimationBehavior : StateMachineBehaviour
{

    private PlayerComponents playerComponentScript; //we will use the player component script in order to disable movement

    [SerializeField] private bool endOfKnockdownState; // will this animation be the end of the player's knockdown state?

    [SerializeField] private bool allowMovementOnStateExit; // this is used for when we have multiple chains of hurt animations (for the heavy hurt for example)

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

        //if player is hurt, they cannot use their equipment item
        PlayerStats.instance.SetCanUseEquipment(false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (allowMovementOnStateExit)
        {
            playerComponentScript.SetCanMove(true);

            playerComponentScript.SetCanAttack(true);

            playerComponentScript.SetCanBackAttack(true);

            playerComponentScript.SetCanFlip(true);

            playerComponentScript.SetCanSlide(true);

            playerComponentScript.SetCanRoll(true);

            
        }

        if (endOfKnockdownState)
        {
            animator.transform.gameObject.GetComponent<PlayerHurt>().SetIsKnockedDown(false);
        }
        
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
