using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// EnemyDamageHandler requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody2D))]
public class DamageHandler : MonoBehaviour
{
    private Rigidbody2D rb;

    //private bool directionFacingRight;

    private Transform target;

    //private EnemyController enemyController;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnHurt(DamageType damageType, Vector3 attacker, float damage, float attackPowerX, float attackPowerY, bool receiverWasPlayer)
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
                    //call the TakeDamage function to subtract the health of player or enemy's (depending on value of receiverWasPlayer) as well as add force to their rigidbody
                    TakeDamage(damage, -attackPowerX, -attackPowerY, receiverWasPlayer);


                }
                else
                {
                    //Play forward flinch animation
                    Debug.Log("Forward hit! light");
                    TakeDamage(damage, attackPowerX, attackPowerY, receiverWasPlayer);
                }
                break;

            case DamageType.heavy:
                if (isFromBehind)
                {
                    //Play backward knock back animation
                    Debug.Log("Backward hit! heavy");
                    TakeDamage(damage, -attackPowerX, -attackPowerY, receiverWasPlayer);

                }
                else
                {
                    //Play forward knock back animation
                    Debug.Log("Forward hit! heavy");
                    TakeDamage(damage, attackPowerX, attackPowerY, receiverWasPlayer);
                }
                break;
            case DamageType.air:
                if (isFromBehind)
                {
                    //Play backward knock back animation
                    Debug.Log("Backward hit! air");
                    TakeDamage(damage, -attackPowerX, -attackPowerY, receiverWasPlayer);
                }
                else
                {
                    //Play forward knock back animation
                    Debug.Log("Forward hit! air");
                    TakeDamage(damage, attackPowerX, attackPowerY, receiverWasPlayer);
                }
                break;

            default:
                break;
        }
    }
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY, bool receiverWasPlayer)
    {

        //this GameObjects's health is subtracted by damage dealt
        //if (receiverWasPlayer == true)
        //playerHealth -= damage
        //else
        //enemyHealth -= damage

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }
}