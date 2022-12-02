using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AttackAnimationBehavior requires the PlayerComponents script
[RequireComponent(typeof(PlayerComponents))]
public class AttackAnimationBehavior : StateMachineBehaviour, IDamageAttributes
{
    //I made this script because I want the transitions to use the same animation as the attacks that came before it (makes attacking animations stick out longer without awkwardly idling during attacks)
    //But I don't want hitboxes to appear twice, so I am tieing the enabling of hitboxes during attack animations to this script, if the animation has a hitbox during it, then enable it at start, and disable it when it ends


    // ONLY ANIMATIONS THAT HAVE A HITBOX (NOT HURTBOX) SHOULD USE THIS SCRIPT

    [Header("Damage Type")]
    //type of damage the attack will do (light -- > enemy flinches, heavy -- > enemy knocked back )
    public IDamageAttributes.DamageType damageType;

    private PlayerComponents playerComponentScript; //we will use the player component script in order to invoke the setCanInteract() function
    private PlayerCollisionLayerChange playerCollisionLayerScript;
    private Rigidbody2D rb;

    private BoxCollider2D hitbox;

    private bool playerFacingRight; // represents direction player is facing (retrieved from playerComponents script)

    [Header("Damage & Force Values")]
    [SerializeField] private float attackDamage; // damage of the attack
    [SerializeField] private float attackingPowerX; // amount of force applied to enemy that is hit by this attack in x-direction
    [SerializeField] private float attackingPowerY; // amount of force applied to enemy that is hit by this attack in y-direction

    [Header("Particle Effect Values")]
    [SerializeField] private Material particleEffectMaterial; // the material that the particle system will use for this animation (how the particle will look like)
    [SerializeField] private float particleEffectDuration; // duration that the attack particle effect will play for
    

    [Header("Screenshake Values")]
    [SerializeField] private float screenShakePower; // amount of screenshake to apply
    [SerializeField] private float screenShakeDuration; // duration of screenshake

    [Header("Hitstop Values")]
    //[SerializeField] private int hitStopRestoreTime; // how quickly it will take for the hitstop to recover or for timescale to reset (higher values = quicker hitstop duration)
    [SerializeField] private float hitStopDelay; // how long will hitstop delay last?

    [Header("Jolt Force Applied To Player")]
    [SerializeField] private ForceMode2D forceMode; // the force mode applied to the jolt force
    [SerializeField] private float joltForceX; //determines how far the player will 'jolt' forward in the x-direction when attacking (Should be a high value)
    [SerializeField] private float joltForceY; //determines how far the player will 'jolt' forward in the y-direction when attacking (Should be a high value)


    [Space(20)]
    [SerializeField] private float gravityDuringAttack; // how much gravity is applied to player during this attack?  

    private int isGroundedHash; //hash value for animator's isGrounded parameter (for performance)

    [SerializeField] private bool turnOffEnemyCollision; // should this attack tell player's colliders to ignore enemy collision?

    [SerializeField] private bool resetYVelocityOnAttack; // will the player's rigidbody Y velocity reset when they attack?


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetBool("isAttacking", true);

        // apply damage/ force multipliers from PlayerStats
        attackDamage *= PlayerStats.instance.GetDamageMultiplier();
        attackingPowerX *= PlayerStats.instance.GetAttackPowerMultiplier();
        attackingPowerY *= PlayerStats.instance.GetAttackPowerMultiplier();


        //improves performance
        isGroundedHash = Animator.StringToHash("isGrounded");

        //when animation begins, retrieve the player's hitbox from the PlayerComponent's script
        playerComponentScript = animator.transform.gameObject.GetComponent<PlayerComponents>();

        hitbox = playerComponentScript.GetHitBox();

        IDamageDealingCharacter damageDealingScript = hitbox.gameObject.GetComponent<IDamageDealingCharacter>();

        //invoke hitbox's function updates damage values
        damageDealingScript.UpdateAttackValues(damageType, attackDamage, attackingPowerX, attackingPowerY, screenShakePower, screenShakeDuration);

        damageDealingScript.SetParticleEffectDetails(particleEffectMaterial,particleEffectDuration);

        // set restore time based on the attack animation (ideally, a stronger attack like the ground slam or footdive should have a smaller restore time to make hitstop last longer)
        hitbox.gameObject.GetComponent<PlayerHitCollider>().SetHitStopValues(hitStopDelay);

        //retrive which way player is facing
        playerFacingRight = playerComponentScript.GetPlayerDirection();

        //grab hitbox and rigidbody component
        //hitbox = playerComponentScript.GetHitBox();
        rb = playerComponentScript.GetRB();

        playerComponentScript.SetCanFlip(false);
        playerComponentScript.SetCanClimb(false);

        //invoke jolt movement 
        JoltThisObject(playerFacingRight, joltForceX, joltForceY);


        // if gravity is set to 0 in inspector, then we're not trying to change player's gravity during the attack
        if (gravityDuringAttack != 0)
            SetEntityGravity(gravityDuringAttack);


        if (turnOffEnemyCollision)
        {
            playerCollisionLayerScript = animator.transform.GetComponent<PlayerCollisionLayerChange>();
            playerCollisionLayerScript.SetIgnoreEnemyLayer();
        }


    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if player is no longer grounded during attack animation, allow them to jump, otherwise don't
        if (!animator.GetBool(isGroundedHash))
            playerComponentScript.SetCanMove(true);
        else
            playerComponentScript.SetCanMove(false);

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playerCollisionLayerScript != null)
            playerCollisionLayerScript.ResetLayer();
    }

    public void JoltThisObject(bool directionIsRight, float powerX, float powerY)
    {
        // if this attack is supposed to reset y velocity on attack
        if (resetYVelocityOnAttack)
            playerComponentScript.GetRB().velocity = Vector2.zero;

        if (directionIsRight)
            rb.AddForce(new Vector2(powerX, powerY),forceMode);
        //if player is facing left, then multiply force by negative 1 to prevent player from jolting backwards
        else
            rb.AddForce(new Vector2(-powerX, powerY), forceMode);
    }

    // set the player's gravity to "gravityDuringAttack" amount
    public void SetEntityGravity(float amountOfGravity)
    {
        PlayerStats.instance.SetPlayerGravity(amountOfGravity);
    }
}
