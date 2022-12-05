using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideStart : StateMachineBehaviour
{

    private PlayerComponents playerCompScript;

    [SerializeField] private AudioClip slideSound;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCompScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //disable player's ability jump at start of slide
        playerCompScript.SetCanJump(false);

        ObjectSounds.instance.PlaySoundEffect(slideSound);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
