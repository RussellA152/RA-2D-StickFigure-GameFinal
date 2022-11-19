using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimationEnd : StateMachineBehaviour
{
    //private EnemyController enemyControllerScript;

    //[SerializeField] private float timeUntilDeath; // how long does it take (after this enemy starts, for this enemy to change to death state in EnemyController)

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemyControllerScript = animator.transform.gameObject.GetComponent<EnemyController>();
        
        
        //enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Dead);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if(timeUntilDeath <= 0f)
        //enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Dead);
        //timeUntilDeath -= Time.deltaTime;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Dead);
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
