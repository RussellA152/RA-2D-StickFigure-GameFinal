using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitCollider : MonoBehaviour, IDamageDealing
{
    private BoxCollider2D hitbox;

    private Transform targetTransform;// the transform of who enters our hitbox collider

    private bool enemyInsideTrigger; //is the enemy inside of our hitbox collider?

    private void Start()
    {
        hitbox = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetTransform = collision.transform;

        //checking if trigger collided with EnemyHurtBox tag (located only on the hurtbox child gameobject on each enemy)
        if (targetTransform.CompareTag("EnemyHurtBox"))
        {
            //the hurtbox is a child of the enemy, so set the target equal to the parent
            targetTransform = targetTransform.parent;

            enemyInsideTrigger = true;
            //DealDamage(attackDamage, attackingPowerX, attackingPowerY);
        }
        else
        {
            enemyInsideTrigger = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if we hit the enemy's hurt box
        if (targetTransform.CompareTag("EnemyHurtBox"))
        {
            enemyInsideTrigger = false;
        }
    }

    public void DealDamage(Transform attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            if (enemyInsideTrigger)
            {

                //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the player's hit collider)
                targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(attacker.position, damageType, damage, attackPowerX, attackPowerY);

                //set target to null afterwards to prevent player from dealing damage to enemy without any collision
                targetTransform = null;
            }

        }
    }


    /*
    private Transform targetTransform;

    private DamageType damageType; //type of damage the attack does (updated from AttackAnimationBehavior)

    private float attackDamage; //damage of the attack

    private float attackingPowerX; //amount of force applied to enemy that is hit by this attack in x-direction
    private float attackingPowerY; //amount of force applied to enemy that is hit by this attack in y-direction

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        targetTransform = collision.transform;

        //checking if trigger collided with EnemyHurtBox tag (located only on the hurtbox child gameobject on each enemy)
        if (targetTransform.CompareTag("EnemyHurtBox"))
        {
            //the hurtbox is a child of the enemy, so set the target equal to the parent
            targetTransform = targetTransform.parent;
            DealDamage(attackDamage, attackingPowerX, attackingPowerY);
        }
            
    }

    protected override void DealDamage(float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the player's hit collider)
            targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(damageType, transform.parent.position, damage, attackPowerX, attackPowerY);

            //set target to null afterwards to prevent player from dealing damage to enemy without any collision
            targetTransform = null;
        }
    }

    public override void UpdateAttackValues(DamageType damageTypeParameter, float damage, float attackPowerX, float attackPowerY)
    {
        damageType = damageTypeParameter;

        attackDamage = damage;

        attackingPowerX = attackPowerX;

        attackingPowerY = attackPowerY;

        //throw new System.NotImplementedException();
    }

    */
}
