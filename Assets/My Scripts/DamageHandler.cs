using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    private Rigidbody2D rb;

    //private bool directionFacingRight;

    private Transform target;

    //private EnemyController enemyController;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //enemyController = GetComponent<EnemyController>();

        //FLIPS ENEMY (using for test purposes right now)
        // Multiply the player's x local scale by -1.
        //Vector3 theScale = transform.localScale;
       /// theScale.x *= -1;
        //transform.localScale = theScale;
    }

    
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //enemy's health is subtract by damage dealt
        //health -= damage;

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }

    public void OnHurt(DamageType damageType, Vector3 attacker, float damage, float attackPowerX, float attackPowerY)
    {

        //need to check direction of attack as well

        Vector3 direction = attacker - transform.position;
        bool isFromBehind = direction.x > 0;

        //do something depending on what attack was performed on this enemy and what direction the attack came from
        switch (damageType)
        {
            case DamageType.light:
                
                if (isFromBehind)
                {
                    //Play backward flinch animation
                    Debug.Log("Backward hit! light");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);


                }
                else
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! light");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }
                break;

            case DamageType.heavy:
                if (isFromBehind)
                {
                    //Play backward knock back animation
                    Debug.Log("Backward hit! heavy");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);

                }
                else
                {
                    //Play forward knock back animation
                    Debug.Log("Forward hit! heavy");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }
                break;
            case DamageType.air:
                if (isFromBehind)
                {
                    //Play backward knock back animation
                    Debug.Log("Backward hit! air");
                    TakeDamage(damage, -attackPowerX, -attackPowerY);
                }
                else
                {
                    //Play forward knock back animation
                    Debug.Log("Forward hit! air");
                    TakeDamage(damage, attackPowerX, attackPowerY);
                }
                break;

            default:
                break;
        }
    }
}
