using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleAnimationBehavior : StateMachineBehaviour
{
    private EnemyController enemyControllerScript;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyControllerScript = animator.transform.gameObject.GetComponent<EnemyController>();

        //when returning to the idle animation, tell EnemyController to return to idle state
        enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Idle);
        //enemyControllerScript.SetIsAttacking(false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
    }
}
