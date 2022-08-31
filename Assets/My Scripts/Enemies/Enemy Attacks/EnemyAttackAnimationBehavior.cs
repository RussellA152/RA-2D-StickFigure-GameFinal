using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackAnimationBehavior : StateMachineBehaviour, IDamageAttributes
{
    private BoxCollider2D hitbox;

    private Rigidbody2D rb;

    private IDamageDealing enemyHitColliderScript; //EnemyHitCollider.cs script (implements IDamageDealing)
    private EnemyController enemyControllerScript;

    [Header("Damage Type")]
    //type of damage the attack will do (light -- > player flinches, heavy -- > player knocked back )
    public IDamageAttributes.DamageType damageType;

    [Header("Damage & Force")]
    [SerializeField] private float attackDamage; //damage of the attack
    [SerializeField] private float attackingPowerX; //amount of force applied to enemy that is hit by this attack in x-direction
    [SerializeField] private float attackingPowerY; //amount of force applied to enemy that is hit by this attack in y-direction

    [Header("Jolt Force Applied To Enemy")]
    [SerializeField] private float joltForceX; //determines how far the player will 'jolt' forward in the x-direction when attacking (Should be a high value)
    [SerializeField] private float joltForceY; //determines how far the player will 'jolt' forward in the y-direction when attacking (Should be a high value)


    private bool enemyFacingRight; //need to know the direction of the sprite so we know where to apply the jolt force

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.transform.gameObject.GetComponent<Rigidbody2D>();
        //find the EnemyHitCollider script located in the hitbox gameobject
        enemyHitColliderScript = animator.transform.gameObject.GetComponentInChildren<IDamageDealing>();
        enemyControllerScript = animator.transform.gameObject.GetComponent<EnemyController>();

        //retrieve what direction the enemy was facing in
        enemyFacingRight = animator.transform.gameObject.GetComponent<EnemyMovement>().GetDirection();

        //get the enemy's hitbox
        hitbox = enemyHitColliderScript.GetHitBox();

        //invoke jolt movement 
        JoltThisObject(enemyFacingRight, joltForceX, joltForceY);

        hitbox.gameObject.GetComponent<IDamageDealingCharacter>().UpdateAttackValues(damageType, attackDamage, attackingPowerX, attackingPowerY);


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyControllerScript.StartAttackCooldown();

        //enemy is no longer attacking at the end of each animation
        enemyControllerScript.SetIsAttacking(false);

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

    public void JoltThisObject(bool directionIsRight, float powerX, float powerY)
    {
        if (directionIsRight)
            rb.AddForce(new Vector2(powerX, powerY));
        //if player is facing left, then multiply force by negative 1 to prevent player from jolting backwards
        else
            rb.AddForce(new Vector2(-powerX, -powerY));
    }
}

