using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitColliderBase : MonoBehaviour
{
    //private Transform targetTransform;

    //private DamageType damageType; //type of damage the attack does (updated from AttackAnimationBehavior)

    //private float attackDamage; //damage of the attack

    //private float attackingPowerX; //amount of force applied to enemy that is hit by this attack in x-direction
    //private float attackingPowerY; //amount of force applied to enemy that is hit by this attack in y-direction


    void Start()
    {

    }

    protected abstract void OnTriggerEnter2D(Collider2D collision);

    protected abstract void DealDamage(float damage, float attackPowerX, float attackPowerY);

    public abstract void UpdateAttackValues(DamageType damageTypeParameter, float damage, float attackPowerX, float attackPowerY);

    /*
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        targetTransform = collision.transform;

        DealDamage(attackDamage, attackingPowerX, attackingPowerY);
    }


    protected virtual void DealDamage(float damage, float attackPowerX,float attackPowerY)
    {
        //if we have a target, deal damage to it
        if (targetTransform != null)
        {
            //call the HitTarget function which will apply make enemy take damage and a certain amount of force (depends on the direction the attack comes from)
            //targetTransform.gameObject.GetComponent<DamageHandler>().OnHurt(damageType,transform.parent.position,damage, attackPowerX, attackPowerY);
            Debug.Log("DEALT DAMAGE!");

            //set target to null afterwards to prevent player from dealing damage to enemy without any collision
            targetTransform = null;
        }
        
    }

    //attack animations will invoke this method and set the damage collider's attack damage values to the parameters passed in
    protected virtual void UpdateAttackValues(DamageType damageTypeParameter, float damage, float attackPowerX,float attackPowerY)
    {
        damageType = damageTypeParameter;

        attackDamage = damage;

        attackingPowerX = attackPowerX;

        attackingPowerY = attackPowerY;
    }

    */
}
