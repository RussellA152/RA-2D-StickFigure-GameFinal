using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxEnabling : StateMachineBehaviour
{
    //I made this script because I want the transitions to use the same animation as the attacks that came before it (makes attacking animations stick out longer without awkwardly idling during attacks)
    //But I don't want hitboxes to appear twice, so I am tieing the enabling of hitboxes during attack animations to this script, if the animation has a hitbox during it, then enable it at start, and disable it when it ends


    // ONLY ANIMATIONS THAT HAVE A HITBOX (NOT HURTBOX) SHOULD USE THIS SCRIPT

    [SerializeField] private bool animHasHitbox; //determines if the animation has a hitbox tied to it

    [SerializeField] private BoxCollider2D hitbox; //the hitbox (used for attacking enemy) gameobject

    [SerializeField] private bool turnOffHitbox; //determines if we should turn off hitbox

    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCanInteract() function



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //when animation begins, retrieve the player's hitbox from the PlayerComponent's script
        // this WILL break if the hitbox has a new parent
        playerComponentScript = animator.transform.parent.gameObject.GetComponent<PlayerComponents>();
        hitbox = playerComponentScript.getHitBox();
        

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
        playerComponentScript.setCanMove(false);
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
}