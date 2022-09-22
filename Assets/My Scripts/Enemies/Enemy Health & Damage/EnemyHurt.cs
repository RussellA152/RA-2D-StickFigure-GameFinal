using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour, IDamageable
{
    [Header("Required Scripts")]
    [SerializeField] private EnemyController enemyControlScript; //every enemy will have a movement script
    [SerializeField] private EnemyScriptableObject enemyScriptableObject; //every enemy will have a scriptable object
    [SerializeField] private EnemyMovement enemyMovementScript;

    private IHealth healthScript;
    
    [Header("Required Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float gravityWhenFlinching;
    [SerializeField] private float gravityWhenKnockedDown;

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

    private bool isKnockedDown = false; // is this enemy in a "knocked down" state? 

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

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Forward light hit! enemy facing left!");

                    if (!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimFrontHash);

                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Forward light hit! enemy facing right!");

                    if(!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimFrontHash);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Backward light hit! enemy facing left!");
                    if (!isKnockedDown)
                        PlayHurtAnimation(lightHurtAnimBehindHash);

                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            case IDamageAttributes.DamageType.heavy:

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

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward heavy animation
                    //Debug.Log("Forward heavy hit! enemy facing left!");

                    PlayHurtAnimation(heavyHurtAnimFrontHash);

                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play forward heavy animation
                    //Debug.Log("Forward heavy hit! enemy facing right!");

                    PlayHurtAnimation(heavyHurtAnimFrontHash);

                    //call the TakeDamage function to subtract the health of enemy 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play backward heavy animation
                    //Debug.Log("Backward heavy hit! enemy facing left!");

                    PlayHurtAnimation(heavyHurtAnimBehindHash);

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


    //plays the hurt animation (depending on the damage type and direction of the attack)
    public void PlayHurtAnimation(int animationHash)
    {
        //if the animation exists in the base layer...
        //play it, otherwise log that it doesn't exist
        if (animator.HasState(baseLayerInt, animationHash) == true)
        {
            animator.Play(animationHash);
            Debug.Log("Playing an enemy hurt animation");
        }
            
        else
            Debug.Log("This animation does not exist!");
    }

    IEnumerator WaitUntilEnemyIsHurt(float attackPowerX, float attackPowerY)
    {
        //Wait until the enemy has changed to the hurt state to apply a force on them
        while (enemyControlScript.GetEnemyState() != EnemyController.EnemyState.Hurt)
            yield return null;

        if (isKnockedDown)
        {
            rb.velocity = Vector2.zero;
        }

        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }

    public void SetIsKnockedDown(bool boolean)
    {
        isKnockedDown = boolean;
    }

    public void SetRBGravity(float amount)
    {
        rb.gravityScale = amount;
    }
}
