using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCollider : HitColliderBase
{
    private Transform targetTransform;

    private DamageType damageType; //type of damage the attack does (updated from AttackAnimationBehavior)

    private float attackDamage; //damage of the attack

    private float attackingPowerX; //amount of force applied to enemy that is hit by this attack in x-direction
    private float attackingPowerY; //amount of force applied to enemy that is hit by this attack in y-direction


    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        targetTransform = collision.transform;

        //checking if trigger collided with PlayerHurtBox tag (located only on the hurtbox child gameobject on the Player)
        if (targetTransform.CompareTag("PlayerHurtBox"))
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
            //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the enemy's hit collider)
            targetTransform.gameObject.GetComponent<DamageHandler>().OnHurt(damageType, transform.parent.position, damage, attackPowerX, attackPowerY, true);

            //set target to null afterwards to prevent enemy from dealing damage to player without any collision
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

}
