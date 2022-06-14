using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AttackTransitionBehavior handles the activation of scripts (if the player presses a heavy, light, back, etc.. attack within the time interval (transition's exit time) the corresponding attack animation will play
// EXIT Time for transition animations refer to how long it will take to revert back to Idle state

// AttackTransitionBehavior requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class AttackTransitionBehavior : StateMachineBehaviour
{

    [Header("Name of Attack Animation To Be Played")]
    public string attackName; //name of light attack
    public string heavyAttackName; //name of heavy attack
    public string backAttackName; //name of back attack
    public string backHeavyAttackName; //name of back heavy attack

    [Header("Allow Movement During This Transition?")]
    [SerializeField] private bool allowMovementDuringAnim; //this bool determines if the player is allowed to move during this transition (used for Idle animation, otherwise player can't move in Idle state)
    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCan"action" functions


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //retrieve component script
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //let player flip during attack transition period
        playerComponentScript.SetCanFlip(true);

        // IF this animation allows movement during animation then allow player to move (instead the animation will move player a little)
        // we also set canAttack to false inside of "AttackAnimationBehavior.cs"
        if (allowMovementDuringAnim)
            playerComponentScript.SetCanMove(true);
        else
            playerComponentScript.SetCanMove(false);

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If player fails to meet any of the following conditions below, the player will return to the Idle state 

        // if we press back button (could be 'a' or 'd') + left click, play the corresponding back light attack
        if (AttackController.instance.isBackAttacking)
        {
            if(backAttackName != "")
            {
                AttackController.instance.animator.Play(backAttackName);
                //Debug.Log("Light ATTACK!");

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
        else if (AttackController.instance.isHeavyAttacking)
        {
            if (heavyAttackName != "")
            {
                AttackController.instance.animator.Play(heavyAttackName);
                
                //Debug.Log("Heavy ATTACK!");
                


            }
                
        }
        //if we press left click during an attack animation, play the corresponding light attack
        else if (AttackController.instance.isLightAttacking)
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
        

        if (AttackController.instance.isLightAttacking)
        {
            AttackController.instance.isLightAttacking = false;
            //if player did a regular attack, dont let them do a back attack
            playerComponentScript.SetCanBackAttack(false);
        }
            
        if (AttackController.instance.isHeavyAttacking)
        {
            AttackController.instance.isHeavyAttacking = false;
            playerComponentScript.SetCanBackAttack(false);

        }
        if (AttackController.instance.isBackAttacking)
        {
            AttackController.instance.isBackAttacking = false;

            //after back attacking, the player will need to turn around again (see CharacterController2D script)
            playerComponentScript.SetCanBackAttack(false);

        }
        if (AttackController.instance.isBackHeavyAttacking)
        {
            AttackController.instance.isBackHeavyAttacking = false;

            //after back attacking, the player will need to turn around again (see CharacterController2D script)
            playerComponentScript.SetCanBackAttack(false);

        }


    }
}