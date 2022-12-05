using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour, IDamageable
{

    [Header("Required Scripts")]
    [SerializeField] private EnemyController enemyControlScript; //every enemy will have a movement script
    [SerializeField] private EnemyScriptableObject enemyScriptableObject; //every enemy will have a scriptable object
    [SerializeField] private EnemyMovement enemyMovementScript;
    [SerializeField] private EnemyHealth enemyHealthScript;
    [SerializeField] private EnemyGroundCheck enemyGroundCheckScript; // will use this to disable the ground check when an enemy is knocked into the air (prevents them from getting back up mid animation)

    private IHealth healthScript;
    
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    //[SerializeField] private BoxCollider2D hurtBox;

    [SerializeField] private float gravityWhenFlinching;
    [SerializeField] private float gravityWhenKnockedDown;
    [SerializeField] private float immunityTimer; // if enemy is hit too many times mid-air, eventually they need to be able to get back up
    //private bool isTempImmune; //


    [Header("Timers")]
    [SerializeField] private float timeGroundCheckIsDisabled; // how long will the ground check be disabled when an enemy FIRST goes into a knockdown state (to be juggled...)


    [Header("Hurt Sounds")]
    [SerializeField] private AudioClip lightHurtFrontSound;
    [SerializeField] private AudioClip lightHurtBehindSound;
    [SerializeField] private AudioClip heavyHurtFrontSound;
    [SerializeField] private AudioClip heavyHurtBehindSound;

    //Animations to play when enemy is hit by a light attack (depends on the direction of the light attack)
    //[Header("Enemy Light Attack Hurt Animation Names")]
    private string lightHurtAnimFront;
    private string lightHurtAnimBehind;

    //Animations to play when enemy is hit by a heavy attack (depends on the direction of the heavy attack)
    //[Header("Enemy Heavy Attack Hurt Animation Names")]
    private string heavyHurtAnimFront;
    private string heavyHurtAnimBehind;

    private int baseLayerInt = 0;

    //String to hash values for the animations to save performance
    private int lightHurtAnimFrontHash;
    private int lightHurtAnimBehindHash;
    private int heavyHurtAnimFrontHash;
    private int heavyHurtAnimBehindHash;


    private float knockdownBuildUpResistance = 0.0f; // value used to slowly resist amount of force when juggled (meant to prevent enemies from being juggled permanently)
    private bool isKnockedDown = false; // is this enemy in a "knocked down" state? (if so, do not play any other hurt animations like the flinch if in knockdown state)

    /*
    private void OnEnable()
    {
        healthScript = GetComponent<IHealth>();
        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        //retrieve the animations to play from the scriptable object
        lightHurtAnimFront = enemyScriptableObject.lightHurtAnimFront;
        lightHurtAnimBehind = enemyScriptableObject.lightHurtAnimBehind;
        heavyHurtAnimFront = enemyScriptableObject.heavyHurtAnimFront;
        heavyHurtAnimBehind = enemyScriptableObject.heavyHurtAnimBehind;

        lightHurtAnimFrontHash = Animator.StringToHash(lightHurtAnimFront);
        lightHurtAnimBehindHash = Animator.StringToHash(lightHurtAnimBehind);

        heavyHurtAnimFrontHash = Animator.StringToHash(heavyHurtAnimFront);
        heavyHurtAnimBehindHash = Animator.StringToHash(heavyHurtAnimBehind);
    }
    */

    private void Update()
    {
        //Debug.Log("is enemy knocked down?  " + isKnockedDown);
    }

    public void InitializeEnemyHurt(EnemyScriptableObject enemyScriptableObjectParameter)
    {
        healthScript = GetComponent<IHealth>();
        //rb = GetComponent<Rigidbody2D>();

        //animator = GetComponent<Animator>();

        enemyScriptableObject = enemyScriptableObjectParameter;

        //retrieve the animations to play from the scriptable object
        lightHurtAnimFront = enemyScriptableObject.lightHurtAnimFront;
        lightHurtAnimBehind = enemyScriptableObject.lightHurtAnimBehind;
        heavyHurtAnimFront = enemyScriptableObject.heavyHurtAnimFront;
        heavyHurtAnimBehind = enemyScriptableObject.heavyHurtAnimBehind;

        lightHurtAnimFrontHash = Animator.StringToHash(lightHurtAnimFront);
        lightHurtAnimBehindHash = Animator.StringToHash(lightHurtAnimBehind);

        heavyHurtAnimFrontHash = Animator.StringToHash(heavyHurtAnimFront);
        heavyHurtAnimBehindHash = Animator.StringToHash(heavyHurtAnimBehind);

        //isTempImmune = false;
        //hurtBox.enabled = true;
    }

    public void OnHurt(Vector3 attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        


        //find the direction the attacker is facing
        Vector3 directionOfAttacker = attacker - transform.position;

        //is the attacker facing the right direction?
        bool attackerFacingRight = directionOfAttacker.x < 0;

        //get the direction the enemy is facing
        bool enemyFacingRight = enemyMovementScript.GetDirection();

        //do something depending on what attack was performed on this enemy and what direction the attack came from
        switch (damageType)
        {
            // if the damageType was "none" don't play an animation (could be something like DOT, if we were to make that)
            case IDamageAttributes.DamageType.none:
                break;

            case IDamageAttributes.DamageType.light:

                SetRBGravity(gravityWhenFlinching);

                //if the attacker's sprite is facing the right direction, and the enemy's sprite is also facing right
                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Backward light hit! enemy facing right!");
                    if (!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimBehindHash);

                    ObjectSounds.instance.PlaySoundEffect(lightHurtBehindSound);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Forward light hit! enemy facing left!");

                    if (!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimFrontHash);

                    ObjectSounds.instance.PlaySoundEffect(lightHurtFrontSound);

                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Forward light hit! enemy facing right!");

                    if(!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimFrontHash);

                    ObjectSounds.instance.PlaySoundEffect(lightHurtFrontSound);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    // only if this enemy isnt "knockedDown"
                    if (!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimBehindHash);

                    ObjectSounds.instance.PlaySoundEffect(lightHurtBehindSound);

                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            case IDamageAttributes.DamageType.heavy:

                // before setting the enemy in a knockdown state, disable the ground check for a small amount of time to prevent them getting back on their feet mid air (mid animation)
                if (!isKnockedDown)
                    enemyGroundCheckScript.DisableGroundCheck(timeGroundCheckIsDisabled);

                SetRBGravity(gravityWhenKnockedDown);

                if (isKnockedDown)
                {
                    Debug.Log("Hit mid air by heavy!");
                }

                

                SetIsKnockedDown(true);

                

                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward heavy animation
                    //Debug.Log("Backward heavy hit! enemy facing right!");

                    PlayHurtAnimation(heavyHurtAnimBehindHash);

                    ObjectSounds.instance.PlaySoundEffect(heavyHurtBehindSound);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward heavy animation
                    //Debug.Log("Forward heavy hit! enemy facing left!");

                    PlayHurtAnimation(heavyHurtAnimFrontHash);

                    ObjectSounds.instance.PlaySoundEffect(heavyHurtFrontSound);

                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play forward heavy animation
                    //Debug.Log("Forward heavy hit! enemy facing right!");

                    PlayHurtAnimation(heavyHurtAnimFrontHash);

                    ObjectSounds.instance.PlaySoundEffect(heavyHurtFrontSound);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play backward heavy animation
                    //Debug.Log("Backward heavy hit! enemy facing left!");

                    PlayHurtAnimation(heavyHurtAnimBehindHash);

                    ObjectSounds.instance.PlaySoundEffect(heavyHurtBehindSound);

                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            case IDamageAttributes.DamageType.air:
                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward air animation
                    //Debug.Log("Backward air hit! enemy facing right!");
                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward air animation
                    //Debug.Log("Forward air hit! enemy facing left!");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play forward air animation
                    //Debug.Log("Forward air hit! enemy facing right!");
                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play backward air animation
                    //Debug.Log("Backward air hit! enemy facing left!");
                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            default:
                break;
        }
    }
    //this function takes in 3 parameters, the damage of the attack dealt to this gameobject, the attack power forces (x & y direction) applied to this gameobject
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //need to getComponent each time enemy is attacked because we can't cache this in Start() because the enemy will be enabled/disabled constantly during runtime
        healthScript.ModifyHealth(-1f * damage);

        //change enemy's current state to the Hurt state (they can't move or flip their sprite)
        enemyControlScript.ChangeEnemyState(0f, EnemyController.EnemyState.Hurt);


        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        //rb.AddForce(new Vector2(attackPowerX, attackPowerY));

        StartCoroutine(WaitUntilEnemyIsHurt(attackPowerX, attackPowerY));
    }

    public void PlayParticleEffect()
    {

    }


    //plays the hurt animation (depending on the damage type and direction of the attack)
    public void PlayHurtAnimation(int animationHash)
    {
        //if the animation exists in the base layer...
        //play it, otherwise log that it doesn't exist
        // also, enemy must be alive to play a new animation
        if (animator.HasState(baseLayerInt, animationHash) == true && enemyHealthScript.CheckIfAlive())
        {
            animator.Play(animationHash);
            //Debug.Log("Playing an enemy hurt animation");
        }
            
        //else
            //Debug.Log("This animation does not exist!");
    }

    IEnumerator WaitUntilEnemyIsHurt(float attackPowerX, float attackPowerY)
    {
        //Wait until the enemy has changed to the hurt state to apply a force on them

        while (enemyControlScript.GetEnemyState() != EnemyController.EnemyState.Hurt && enemyControlScript.GetEnemyState() != EnemyController.EnemyState.Dying)
        {
            //Debug.Log("Null, wait until I'm not hurt or dying");
            yield return null;
        }
            

        if (isKnockedDown)
        {
            rb.velocity = Vector2.zero;
        }

        // attack power decreases each time enemy is hit mid air
        //attackPowerX = attackPowerX - (attackPowerX * knockdownBuildUpResistance);

        attackPowerY = attackPowerY - (attackPowerY * knockdownBuildUpResistance);
        

        rb.AddForce(new Vector2(attackPowerX, attackPowerY), ForceMode2D.Impulse);

        // resistance builds up by 5% with each hit taken while knocked down
        knockdownBuildUpResistance += 0.05f;

        if (knockdownBuildUpResistance >= 1.0f)
        {
            knockdownBuildUpResistance = 1.0f;
        }

        //if(knockdownBuildUpResistance == 1.0f)
        //{
        //    if (enemyGroundCheckScript.GetIsGrounded() && !isTempImmune)
        //    {

        //        StartCoroutine(TemporaryImmunity(immunityTimer));
        //    }
        //}
            
    }

    public void SetIsKnockedDown(bool boolean)
    {
        isKnockedDown = boolean;
    }


    public void ResetKnockDownResistance()
    {
        knockdownBuildUpResistance = 0.0f;
    }

    // return "isKnockedDown" needed by EnemyBackColliderAttack script
    public bool GetIsKnockedDown()
    {
        return isKnockedDown;
    }

    public void SetRBGravity(float amount)
    {
        rb.gravityScale = amount;
    }

    //IEnumerator TemporaryImmunity(float timer)
    //{
    //    Debug.Log("Started temp immune!");
    //    isTempImmune = true;
    //    hurtBox.enabled = false;
    //    yield return new WaitForSeconds(timer);
    //    hurtBox.enabled = true;
    //    isTempImmune = false;
    //}
}
