using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private IHealth healthScript;

    private PlayerComponents playerComponentScript;

    private void Start()
    {
        healthScript = GetComponent<IHealth>();
        rb = GetComponent<Rigidbody2D>();
        playerComponentScript = GetComponent<PlayerComponents>();
    }

    public void OnHurt(Vector3 attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        //find the direction the attacker is facing
        Vector3 directionOfAttacker = attacker - transform.position;
        
        //is the attacker facing the right direction?
        bool attackerFacingRight = directionOfAttacker.x < 0;

        //get the direction the player is facing
        bool playerFacingRight = playerComponentScript.GetPlayerDirection();

        //do something depending on what attack was performed on this enemy and what direction the attack came from
        switch (damageType)
        {
            case DamageType.none:
                break;

            case DamageType.light:

                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! light 1");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if(attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! light 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! light 2");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Backward hit! light 2");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
                }
                break;

            case DamageType.heavy:
                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! heavy 1");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! heavy 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! heavy 2");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Backward hit! heavy 2");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
                }
                break;

            case DamageType.air:
                if (attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! air 1");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! air 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && playerFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! air 2");
                    //call the TakeDamage function to subtract the health of player 
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !playerFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Backward hit! air 2");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
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

        Debug.Log("PLAYER WAS HIT!");

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }
}
