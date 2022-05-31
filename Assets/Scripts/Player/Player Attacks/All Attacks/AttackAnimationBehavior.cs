using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAnimationBehavior : StateMachineBehaviour
{
    //I made this script because I want the transitions to use the same animation as the attacks that came before it (makes attacking animations stick out longer without awkwardly idling during attacks)
    //But I don't want hitboxes to appear twice, so I am tieing the enabling of hitboxes during attack animations to this script, if the animation has a hitbox during it, then enable it at start, and disable it when it ends


    // ONLY ANIMATIONS THAT HAVE A HITBOX (NOT HURTBOX) SHOULD USE THIS SCRIPT

    [SerializeField] private bool animHasHitbox; //determines if the animation has a hitbox tied to it

    [SerializeField] private BoxCollider2D hitbox; //the hitbox (used for attacking enemy) gameobject

    [SerializeField] private bool turnOffHitbox; //determines if we should turn off hitbox

    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCanInteract() function

    private Rigidbody2D rb;

    private bool playerFacingRight; // represents direction player is facing (retrieved from playerComponents script)

    [SerializeField] private float joltForceX; //determines how far the player will 'jolt' forward in the x direction when attacking (Should be a high value)
    [SerializeField] private float joltForceY; //determines how far the player will 'jolt' forward in the y direction when attacking (Should be a high value)



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //when animation begins, retrieve the player's hitbox from the PlayerComponent's script
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();
        //retrive which way player is facing
        playerFacingRight = playerComponentScript.GetPlayerDirection();

        //grab hitbox and rigidbody component
        hitbox = playerComponentScript.GetHitBox();
        rb = playerComponentScript.GetRB();

        //invoke jolt movement 
        joltPlayer(playerFacingRight,joltForceX, joltForceY);
        


        //if this animation contains a hitbox, then enable it at start of animation
        if (animHasHitbox && hitbox != null)
        {
            hitbox.enabled = true;
        }

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Don't let player move during attack animation
        playerComponentScript.SetCanMove(false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if the hitbox is active, and we should turn it off, then set it to false
        if (hitbox.enabled && turnOffHitbox && animHasHitbox)
        {
            hitbox.enabled = false;
        }
    }

    //will move by player using force by powerX and powerY 
    //checks for direction player is facing
    private void joltPlayer(bool directionIsRight,float powerX, float powerY)
    {
        if(directionIsRight)
            rb.AddForce(new Vector2(powerX, powerY));
        //if player is facing left, then multiply force by negative 1 to prevent player from jolting backwards
        else
            rb.AddForce(new Vector2(-powerX, -powerY));
    }
}
