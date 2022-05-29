using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideLoop : StateMachineBehaviour
{
    [SerializeField] private float slideSpeed;
    private float tempSlideSpeed;
    private bool directionIsRight;

    private PlayerComponents playerCompScript;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tempSlideSpeed = slideSpeed;

        playerCompScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        rb = playerCompScript.getRB();
        directionIsRight = playerCompScript.getPlayerDirection();

        rb.AddForce(Vector2.right * tempSlideSpeed);


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(directionIsRight)
            rb.AddForce(Vector2.right * tempSlideSpeed);
        else
            rb.AddForce(Vector2.left * tempSlideSpeed);

        if (tempSlideSpeed > 0)
            tempSlideSpeed -= .8f;
        else if (tempSlideSpeed < 0)
            tempSlideSpeed = 0f;

        if (tempSlideSpeed == 0)
        {
            animator.SetBool("isSliding", false);
            animator.Play("P_Slide_GetUp");
        }

        Debug.Log(tempSlideSpeed);
            
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCompScript.setCanMove(true);
        playerCompScript.setCanAttack(true);
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
