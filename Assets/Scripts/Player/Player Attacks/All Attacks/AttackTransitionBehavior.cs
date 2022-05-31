using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransitionBehavior : StateMachineBehaviour
{
    public string attackName; //name of light attack
    public string heavyAttackName; //name of heavy attack
    public string backAttackName; //name of back attack

    [SerializeField] private bool allowMovementDuringAnim; //this bool determines if the player is allowed to move during this transition
    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCanInteract() function


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        // IF this animation allows movement during animation then allow player to move (instead the animation will move player a little)
        // we also set canAttack to false inside of "AttackAnimationBehavior.cs"
        if (allowMovementDuringAnim)
            playerComponentScript.SetCanMove(true);
        else
        {
            playerComponentScript.SetCanMove(false);
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if we press left click during an attack animation, play the corresponding light attack
        if (AttackController.instance.isAttacking)
        {
            if(attackName != "")
            {
                AttackController.instance.animator.Play(attackName);
                Debug.Log("Light ATTACK!");

            }
                
        }
        //if we press right click during an attack animation, play the corresponding heavy attack
        else if (AttackController.instance.isHeavyAttacking)
        {
            if (heavyAttackName != "")
            {
                AttackController.instance.animator.Play(heavyAttackName);
                
                Debug.Log("Heavy ATTACK!");
                


            }
                
        }
        // if we press back button + left click, play the corresponding light attack
        else if (AttackController.instance.isBackAttacking)
        {
            if (backAttackName != "")
            {
                AttackController.instance.animator.Play(backAttackName);
                Debug.Log("Back ATTACK!");
            }
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AttackController.instance.isAttacking)
        {
            AttackController.instance.isAttacking = false;
        }
            
        if (AttackController.instance.isHeavyAttacking)
        {
            AttackController.instance.isHeavyAttacking = false;

        }
        if (AttackController.instance.isBackAttacking)
        {
            AttackController.instance.isBackAttacking = false;

            //after back attacking, the player will need to turn around again (see CharacterController2D script)
            playerComponentScript.SetCanBackAttack(false);


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