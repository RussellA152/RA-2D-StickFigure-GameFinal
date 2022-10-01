using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatRoll : StateMachineBehaviour
{

    [Header("Speed Properties")]
    [SerializeField] private float minimumRollDistance; // minimum distance the player will roll
    [SerializeField] private float rollDistance; // How far the player will roll

    [Header("Layers That Will Ignore Each Other")]
    [SerializeField] private int playerLayer;
    [SerializeField] private int enemyLayer;

    private bool directionIsRight;

    [SerializeField] private float rollCancellationValue; // how much does TurnRight or TurnLeft binding need to reach to cancel this roll

    



    private PlayerComponents playerCompScript;
    private Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //retrieve player components
        playerCompScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        //set PlayerMovementInput's "rolling" boolean to false so that the player is not stuck infinite rolling (we must call this in this script so that users will be able to roll properly at any frame rate)
        animator.transform.gameObject.GetComponent<PlayerMovementInput>().SetRolling(false);

        //disable player's ability walk & jump & attack during roll
        //playerCompScript.SetCanMove(false);
        playerCompScript.SetCanFlip(false);
        playerCompScript.SetCanAttack(false);

        rb = playerCompScript.GetRB();
        directionIsRight = playerCompScript.GetPlayerDirection();

        //turn off layer collision between "Player" and "Enemy", player will roll behind enemies 
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // if the player is facing right, and tries to move backwards (left), then cancel the roll's force
        if (directionIsRight)
        {
            if (playerCompScript.GetTurnLeft().ReadValue<float>() > 0)
            {
                rb.AddForce(new Vector2(minimumRollDistance * Time.deltaTime, 0f));
                OnStateExit(animator, stateInfo, layerIndex);
            }
                
            else
                rb.AddForce(new Vector2(rollDistance * Time.deltaTime, 0f));

        }
        else
        {
            if (playerCompScript.GetTurnRight().ReadValue<float>() > 0)
            {
                rb.AddForce(new Vector2(-minimumRollDistance * Time.deltaTime, 0f));
                OnStateExit(animator, stateInfo, layerIndex);
            }
                
            else
                rb.AddForce(new Vector2(-rollDistance * Time.deltaTime, 0f));
        }



        //apply force to Vector2.right or Vector2.left depending on which way player is facing
        //if (directionIsRight)
            //rb.AddForce(new Vector2(rollDistance * Time.deltaTime, 0f));
        //else
            //rb.AddForce(new Vector2(-rollDistance * Time.deltaTime, 0f));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        //allow player is move and attack again
        //playerCompScript.SetCanMove(true);
        playerCompScript.SetCanAttack(true);
        playerCompScript.SetCanFlip(true);
    }
}
