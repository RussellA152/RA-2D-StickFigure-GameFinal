using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathAnimationStart : StateMachineBehaviour
{
    private EnemyController enemyControllerScript;

    [Header("Layers That Will Ignore Each Other")]
    [SerializeField] private int playerLayer;
    [SerializeField] private int enemyLayer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyControllerScript = animator.transform.gameObject.GetComponent<EnemyController>();

        // change to hurt state at the start of the death animation (to prevent enemy from switching to another state)
        enemyControllerScript.ChangeEnemyState(0f, EnemyController.EnemyState.Hurt);

        //turn off layer collision between "Player" and "Enemy", so that this enemy corpse doesn't block player
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
