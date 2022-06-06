using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    private Rigidbody2D rb;

    private bool directionFacingRight;

    private Transform target;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        //FLIPS ENEMY (using for test purposes right now)
        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY)
    {
        //enemy's health is subtract by damage dealt
        //health -= damage;

        //apply attackingPowerX & Y force to enemy based on the direction they are facing
        rb.AddForce(new Vector2(attackPowerX, attackPowerY));
    }
}
