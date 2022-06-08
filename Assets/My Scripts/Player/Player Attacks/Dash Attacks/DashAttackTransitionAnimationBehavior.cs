using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// EXIT Time for transition animations refer to how long it will take to revert back to Idle state
//There are only dash attacks when pressing heavy attack button, not for light attack button
public class DashAttackTransitionAnimationBehavior : StateMachineBehaviour
{
    public string attackName; //name of light attack
    public string heavyAttackName; //name of heavy attack
    public string backAttackName; //name of back attack
    public string backHeavyAttackName; //name of back heavy attack
    public string dashAttackName; //name of dash attack

    [SerializeField] private float dashAttackVelocityRequirement; //amount of velocity required to perform dash attack instead of regular heavy attack

    private int animVelocityParameter; //animator's velocity parameter 

    [SerializeField] private bool allowMovementDuringAnim; //this bool determines if the player is allowed to move during this transition
    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCan"action" functions



    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //Improves performance to do this first, instead of doing GetFloat or GetBool with a string every StateUpdate
        animVelocityParameter = Animator.StringToHash("Velocity");

        // IF this animation allows movement during animation then allow player to move (instead the animation will move player a little)
        // we also set canAttack to false inside of HitBoxEnabling ***
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

        //Debug.Log(animator.GetFloat(velocityParameter));

        // if we press back button + left click, play the corresponding back light attack
        
        if (AttackController.instance.isBackAttacking)
        {
            if (backAttackName != "")
            {
                AttackController.instance.animator.Play(backAttackName);

            }

        }
        else if (AttackController.instance.isBackHeavyAttacking)
        {
            if (backHeavyAttackName != "")
            {
                AttackController.instance.animator.Play(backHeavyAttackName);
                //Debug.Log("Light ATTACK!");

            }
        }
        //if we press right click during an attack animation, play the corresponding heavy attack
        else if (AttackController.instance.isHeavyAttacking && animator.GetFloat(animVelocityParameter) > dashAttackVelocityRequirement)
        {
            if (dashAttackName != "")
            {
                AttackController.instance.animator.Play(dashAttackName);


            }

        }
        //if we press right click and we are near the required velocity speed, play the corresponding dash attack
        else if (AttackController.instance.isHeavyAttacking && animator.GetFloat(animVelocityParameter) < dashAttackVelocityRequirement){
            if (heavyAttackName != "")
            {
                AttackController.instance.animator.Play(heavyAttackName);

            }
        }
        //if we press left click during an attack animation, play the corresponding light attack
        else if (AttackController.instance.isAttacking)
        {
            if (attackName != "")
            {
                AttackController.instance.animator.Play(attackName);
                //Debug.Log("Back ATTACK!");
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (AttackController.instance.isAttacking)
        {
            AttackController.instance.isAttacking = false;
            //if player did a regular attack, dont let them do a back attack
            playerComponentScript.SetCanBackAttack(false);
            //playerComponentScript.setCanMove(true);

            //playerComponentScript.setCanInteract(true);
        }

        if (AttackController.instance.isHeavyAttacking)
        {
            AttackController.instance.isHeavyAttacking = false;
            playerComponentScript.SetCanBackAttack(false);
            //playerComponentScript.setCanMove(true);
            //playerComponentScript.setCanInteract(true);

        }
        if (AttackController.instance.isBackAttacking)
        {
            AttackController.instance.isBackAttacking = false;
            playerComponentScript.SetCanBackAttack(false);

        }

        if (AttackController.instance.isBackHeavyAttacking)
        {
            AttackController.instance.isBackHeavyAttacking = false;

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

