using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SlideLoop requires the PlayerComponents script
[RequireComponent(typeof(PlayerComponents))]
public class SlideLoop : StateMachineBehaviour
{
    [Header("Speed Properties")]
    [SerializeField] private float slideSpeed; // How fast the player will slide (slow decreases & must be a high value ex. 34,000)
    [SerializeField] private float slowdownSpeed; // How fast the player's slide will slow down until they have to stand up

    private float tempSlideSpeed;

    private bool directionIsRight;

    private PlayerComponents playerCompScript;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tempSlideSpeed = slideSpeed;

        //retrieve player components
        playerCompScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //disable player's ability walk & jump & attack during slide
        playerCompScript.SetCanMove(false);
        playerCompScript.SetCanAttack(false);

        //retrieve the player's rigidbody
        rb = playerCompScript.GetRB();

        //retrieve the direction the player is facing
        directionIsRight = playerCompScript.GetPlayerDirection();

        //apply force to Vector2.right or Vector2.left depending on which way player is facing
        if (directionIsRight)
            rb.AddForce(Vector2.right * tempSlideSpeed * Time.deltaTime);
        else
            rb.AddForce(Vector2.left * tempSlideSpeed * Time.deltaTime);


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //apply force to Vector2.right or Vector2.left depending on which way player is facing
        if (directionIsRight)
            rb.AddForce(Vector2.right * tempSlideSpeed  * Time.deltaTime);
        else
            rb.AddForce(Vector2.left * tempSlideSpeed * Time.deltaTime);

        //while slide Speed is greater than 0, decrease it
        if (tempSlideSpeed > 0)
            tempSlideSpeed -= slowdownSpeed * Time.deltaTime;

        //if slide speed becomes lower than 0, then reset it to 0
        else if (tempSlideSpeed < 0)
            tempSlideSpeed = 0f;

        //once slide speed reaches 0, play getting up animation
        if (tempSlideSpeed == 0)
        {
            animator.SetBool("isSliding", false);
            animator.Play("P_Slide_GetUp");
        }

        //Debug.Log("Slide speed is: " + tempSlideSpeed);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //allow player is move and attack again
        //playerCompScript.setCanMove(true);
        playerCompScript.SetCanMove(true);

        playerCompScript.SetCanAttack(true);
    }
}