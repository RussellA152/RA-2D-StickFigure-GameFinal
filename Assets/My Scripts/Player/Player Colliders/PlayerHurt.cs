using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurt : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnHurt(Vector3 attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
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

    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //this GameObjects's health is subtracted by damage dealt
        Debug.Log("PLAYER WAS HIT!");

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }
}
