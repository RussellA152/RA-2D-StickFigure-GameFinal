using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHurt : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private IHealth healthScript;
    private EnemyMovement enemyMovementScript;


    void Start()
    {
        healthScript = GetComponent<IHealth>();
        rb = GetComponent<Rigidbody2D>();
        enemyMovementScript = GetComponent<EnemyMovement>();

    }

    public void OnHurt(Vector3 attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
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
            case DamageType.none:
                break;

            case DamageType.light:

                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! light 1");
                    //call the TakeDamage function to subtract the health of player or enemy's
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! light 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! light 2");
                    //call the TakeDamage function to subtract the health of player or enemy's
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Backward hit! light 2");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
                }
                break;

            case DamageType.heavy:
                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! heavy 1");
                    //call the TakeDamage function to subtract the health of player or enemy's 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! heavy 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! heavy 2");
                    //call the TakeDamage function to subtract the health of player or enemy's 
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Backward hit! heavy 2");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
                }
                break;

            case DamageType.air:
                if (attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! air 1");
                    //call the TakeDamage function to subtract the health of player or enemy's 
                    TakeDamage(damage, attackPowerX, attackPowerY);


                }
                else if (attackerFacingRight && !enemyFacingRight)
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! air 1");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }

                else if (!attackerFacingRight && enemyFacingRight)
                {
                    //Play backward flinch animation
                    Debug.Log("Forward hit! air 2");
                    //call the TakeDamage function to subtract the health of player or enemy's 
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else if (!attackerFacingRight && !enemyFacingRight)
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
    //this function takes in 4 parameters, the damage of the attack dealt to this gameobject, the attack power forces (x & y direction) applied to this gameobject
    // and a bool that checks if the player was the one that took damage (we use this script for AI and Player)
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //need to getComponent each time enemy is attacked because we can't cache this in Start() because the enemy will be enabled/disabled constantly during runtime
        healthScript.ModifyHealth(-1f * damage);

        //Debug.Log("health = " + GetComponent<EnemyHealth>().enemyHealth);

        //change enemy's current state to the Hurt state (they can't move or flip their sprite)
        GetComponent<EnemyController>().ChangeEnemyStateToHurt();


        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }
}
