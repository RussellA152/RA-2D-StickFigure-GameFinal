using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script should be placed on the hit box of the player
// when the player performs an attack animation, the damage values of this script will be updated
// and when the enemy is inside the box collider's trigger, call the deal damage function that will apply the damage and force on the enemy
public class PlayerHitCollider : MonoBehaviour, IDamageDealingCharacter
{
    [SerializeField] private BoxCollider2D hitbox;

    private Transform targetTransform;// the transform of who enters our hitbox collider

    private bool enemyInsideTrigger; //is the enemy inside of our hitbox collider?

    //temporary damage values updated by the attack animation
    private IDamageAttributes.DamageType tempDamageType;
    private float tempAttackDamage;
    private float tempAttackPowerX;
    private float tempAttackPowerY;

    private void Start()
    {
        //hitbox = GetComponent<BoxCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //update our target
        targetTransform = collision.transform;

        //checking if trigger collided with EnemyHurtBox tag (located only on the hurtbox child gameobject on each enemy)
        if (targetTransform.CompareTag("EnemyHurtBox"))
        {
            //the hurtbox is a child of the enemy, so set the target equal to the parent
            targetTransform = targetTransform.parent;

            //Debug.Log("Enemy entered player's hitbox!");

            enemyInsideTrigger = true;

            //now that enemy is inside the trigger, call the deal damage function
            DealDamage(transform.parent, tempDamageType, tempAttackDamage, tempAttackPowerX, tempAttackPowerY);
        }
        else
        {
            enemyInsideTrigger = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("my target is " + targetTransform);

        //if there is no target to be found... or the hit collider leaves the enemy's hurt box...
        // then set enemyInsideTrigger to false
        if (targetTransform == null || targetTransform.CompareTag("EnemyHurtBox") == true)
        {
            enemyInsideTrigger = false;

            ResetAttackValues();
        }
    }

    public void DealDamage(Transform attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            if (enemyInsideTrigger)
            {

                //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the player's hit collider)
                targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(attacker.position, damageType, damage, attackPowerX, attackPowerY);

                //Debug.Log("Player hit enemy!");

                //set target to null afterwards to prevent player from dealing damage to enemy without any collision
                targetTransform = null;
            }

        }
    }

    //this function is updated by the player's attack animations
    public void UpdateAttackValues(IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        tempDamageType = damageType;
        tempAttackDamage = damage;
        tempAttackPowerX = attackPowerX;
        tempAttackPowerY = attackPowerY;
    }

    private void ResetAttackValues()
    {
        tempDamageType = IDamageAttributes.DamageType.none;
        tempAttackDamage = 0f;
        tempAttackPowerX = 0f;
        tempAttackPowerY = 0f;

    }

    public BoxCollider2D GetHitBox()
    {
        throw new System.NotImplementedException();
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
