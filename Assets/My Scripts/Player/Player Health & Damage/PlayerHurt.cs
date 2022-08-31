using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour, IDamageable
{
    private IHealth healthScript;
    [SerializeField] private PlayerComponents playerComponentScript;

    private Rigidbody2D rb;
    private Animator animator;

    //Animations to play when player is hit by a light attack (depends on the direction of the light attack)
    [Header("Player Light Attack Hurt Animation Names")]
    [SerializeField] private string lightHurtAnimFront;
    [SerializeField] private string lightHurtAnimBehind;

    //Animations to play when player is hit by a heavy attack (depends on the direction of the heavy attack)
    [Header("Player Heavy Attack Hurt Animation Names")]
    [SerializeField] private string heavyHurtAnimFront;
    [SerializeField] private string heavyHurtAnimBehind;

    private int baseLayerInt = 0;

    //String to hash values for the animations to save performance
    private int lightHurtAnimFrontHash;
    private int lightHurtAnimBehindHash;
    private int heavyHurtAnimFrontHash;
    private int heavyHurtAnimBehindHash;

    private void Start()
    {
        healthScript = GetComponent<IHealth>();
        //playerComponentScript = GetComponent<PlayerComponents>();

        rb = playerComponentScript.GetRB();
        animator = playerComponentScript.GetAnimator();

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

        //get the direction the player is facing
        bool playerFacingRight = playerComponentScript.GetPlayerDirection();

        //do something depending on what attack was performed on the player and what direction the attack came from
        switch (damageType)
        {
            case IDamageAttributes.DamageType.none:
                break;

            case IDamageAttributes.DamageType.light:

                //if the attacker's sprite is facing the right direction, and the player's sprite is also facing right
                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Backward light hit! Player facing right!");

                    PlayHurtAnimation(lightHurtAnimBehindHash);

                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if(attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Forward light hit! Player facing left!");

                    PlayHurtAnimation(lightHurtAnimFrontHash);

                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Forward light hit! Player facing right!");

                    PlayHurtAnimation(lightHurtAnimFrontHash);

                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Backward light hit! Player facing left!");

                    PlayHurtAnimation(lightHurtAnimBehindHash);

                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            case IDamageAttributes.DamageType.heavy:
                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Backward heavy hit! Player facing right!");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Forward heavy hit! Player facing left!");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Forward heavy hit! Player facing right!");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Backward heavy hit! Player facing left!");
                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            case IDamageAttributes.DamageType.air:
                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Backward air hit! Player facing right!");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Forward air hit! Player facing left!");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    //Debug.Log("Forward air hit! Player facing right!");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    //Debug.Log("Backward air hit! Player facing left!");
                    TakeDamage(damage, -attackPowerX, attackPowerY);
                }
                break;

            default:
                break;
        }
    }

    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //this GameObjects's health is subtracted by damage dealt
        healthScript.ModifyHealth(-1f * damage);

        //Debug.Log("PLAYER WAS HIT!");

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }


    public void PlayHurtAnimation(int animationHash)
    {
        //if the animation exists in the base layer...
        //play it, otherwise log that it doesn't exist
        if (animator.HasState(baseLayerInt, animationHash) == true)
        {
            animator.Play(animationHash);
            Debug.Log("Enemy hurt animation played!");
        }
        else
            Debug.Log("This animation does not exist!");
    }
}
