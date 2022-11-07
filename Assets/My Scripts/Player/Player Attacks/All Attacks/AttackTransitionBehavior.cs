using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AttackTransitionBehavior handles the activation of scripts (if the player presses a heavy, light, back, etc.. attack within the time interval (transition's exit time) the corresponding attack animation will play
// EXIT Time for transition animations refer to how long it will take to revert back to Idle state

// AttackTransitionBehavior requires the PlayerComponents script in order to access keybinds
[RequireComponent(typeof(PlayerComponents))]
public class AttackTransitionBehavior : StateMachineBehaviour
{

    //private PlayerCollisionLayerChange playerCollisionLayerScript;

    [Header("Name of Attack Animation To Be Played")]
    public string attackName; //name of light attack
    public string heavyAttackName; //name of heavy attack
    public string backAttackName; //name of back attack
    public string backHeavyAttackName; //name of back heavy attack
    public string jumpLightAttackName; //name of jump light attack
    public string jumpHeavyAttackName; //name of jump heavy attack
    public string groundSlamAttackName; //name of ground slam attack

    [Header("Allow Movement During This Transition?")]
    [SerializeField] private bool allowMovementDuringAnim; //this bool determines if the player is allowed to move during this transition (used for Idle animation, otherwise player can't move in Idle state)
    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCan"action" functions

    [Space(20)]
    [SerializeField] private bool resetGravityOnStateExit; // should the player's gravity reset OnStateExit?

    //[SerializeField] private bool turnOffEnemyCollision;

    //[Header("Allow Attack in Mid-Air?")]

    //[SerializeField] private bool allowAttackMidAir; //this bool determines if the player is allowed to use this attack during this transition even when not grounded

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //retrieve component script
        if(playerComponentScript == null)
            playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //let player flip during attack transition period
        playerComponentScript.SetCanFlip(true);

        playerComponentScript.SetCanClimb(true);

        // IF this animation allows movement during animation then allow player to move (instead the animation will move player a little)
        // we also set canAttack to false inside of "AttackAnimationBehavior.cs"
        if (allowMovementDuringAnim)
            playerComponentScript.SetCanMove(true);
        else
            playerComponentScript.SetCanMove(false);

        //Debug.Log("Get component");

        //if (turnOffEnemyCollision)
        //{
        //playerCollisionLayerScript = animator.transform.GetComponent<PlayerCollisionLayerChange>();
        //playerCollisionLayerScript.SetIgnoreEnemyLayer();
        //}

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

        else if (AttackController.instance.isJumpLightAttacking)
        {
            if (jumpLightAttackName != "")
            {
                AttackController.instance.animator.Play(jumpLightAttackName);
                //Debug.Log("Back ATTACK!");
            }
        }
        else if (AttackController.instance.isJumpHeavyAttacking)
        {
            if (jumpHeavyAttackName != "")
            {
                AttackController.instance.animator.Play(jumpHeavyAttackName);
                //Debug.Log("Back ATTACK!");
            }
        }
        //else if (AttackController.instance.isGroundSlamming)
        //{
        //    if (groundSlamAttackName != "")
        //    {
        //        AttackController.instance.animator.Play(groundSlamAttackName);
        //        //Debug.Log("Back ATTACK!");
        //    }
        //}



    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // reset the player's gravity after attack animation ends (this might be bad code if we ever decide to change the player's gravity from stuff like items or other animations)
        // reset the value because AttackAnimationBehavior modifies it, now it should be set back to normal
        if(resetGravityOnStateExit)
            PlayerStats.instance.ResetGravity();

        //if(playerCollisionLayerScript != null)
            //playerCollisionLayerScript.ResetLayer();

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

        if (AttackController.instance.isJumpLightAttacking)
        {
            AttackController.instance.isJumpLightAttacking = false;
            //if player did a regular attack, dont let them do a back attack
            playerComponentScript.SetCanBackAttack(false);

        }

        if (AttackController.instance.isGroundSlamming)
        {
            AttackController.instance.isGroundSlamming = false;
            Debug.Log("Start Ground Slam cooldown");
            AttackController.instance.StartGroundSlamCooldown();
        }

        if (AttackController.instance.isJumpHeavyAttacking)
        {
            AttackController.instance.isJumpHeavyAttacking = false; ;
            //Debug.Log("Heavy attack!");
            //if player did a regular attack, dont let them do a back attack
            playerComponentScript.SetCanBackAttack(false);

        }

        // player is no longer attacking (is set to true, when player starts to attack or transitions to another attack)
        //animator.SetBool("isAttacking", false);


    }
}