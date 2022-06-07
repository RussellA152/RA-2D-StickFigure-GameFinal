using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{


    private DamageType damageType; //type of damage the attack does (updated from AttackAnimationBehavior)

    private float attackDamage = 0f; //damage of the attack
    private float attackingPowerX = 0f; //amount of force applied to enemy that is hit by this attack in x-direction
    private float attackingPowerY = 0f; //amount of force applied to enemy that is hit by this attack in y-direction

    private Transform targetTransform;

    void Start()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        targetTransform = collision.transform;
        //Debug.Log(targetTransform);

        //check if target is an enemy
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            DealDamage(attackDamage, attackingPowerX, attackingPowerY);
    }


    private void DealDamage(float damage, float attackPowerX,float attackPowerY)
    {
        //if we have a target, deal damage to it
        if (targetTransform != null)
        {
            //call the HitTarget function which will apply make enemy take damage and a certain amount of force (depends on the direction the attack comes from)
            targetTransform.gameObject.GetComponent<DamageHandler>().HitTarget(damageType,transform.parent.position,damage, attackPowerX, attackPowerY);
            //Debug.Log("DEALT DAMAGE!");
            //set target to null afterwards to prevent player from dealing damage to enemy without any collision
            targetTransform = null;
        }
        else
        {
            //Debug.Log("No Target.");
        }
        
            

        
    }

    //attack animations will invoke this method and set the damage collider's attack damage values to the parameters passed in
    public void UpdateAttackValues(DamageType damageTypeParameter, float damage, float attackPowerX,float attackPowerY)
    {
        damageType = damageTypeParameter;

        attackDamage = damage;

        attackingPowerX = attackPowerX;

        attackingPowerY = attackPowerY;
    }
}
