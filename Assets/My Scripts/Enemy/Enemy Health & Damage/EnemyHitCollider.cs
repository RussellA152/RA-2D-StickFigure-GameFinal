using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitCollider : MonoBehaviour, IDamageDealing
{

    private Transform targetTransform; //the gameobject inside of the enemy's hit collider
    private bool playerInsideTrigger; // is the player inside of enemy's hit collider?


    //temporary damage values updated by the attack animation
    private DamageType tempDamageType;
    private float tempAttackDamage;
    private float tempAttackPowerX;
    private float tempAttackPowerY;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //update our target
        targetTransform = collision.transform;

        //checking if trigger collided with PlayerHurtBox tag (located only on the hurtbox child gameobject on the Player)
        if (targetTransform.CompareTag("PlayerHurtBox"))
        {
            //the hurtbox is a child of the player, so set the target equal to the parent
            targetTransform = targetTransform.parent;

            playerInsideTrigger = true;

            //now that player is inside the trigger, call the deal damage function
            DealDamage(transform.parent, tempDamageType, tempAttackDamage, tempAttackPowerX, tempAttackPowerY);
        }
        else
        {
            playerInsideTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //if there is no target to be found... or the hit collider leaves the player's hurt box...
        // then set playerInsideTrigger to false
        if (targetTransform == null || targetTransform.CompareTag("PlayerHurtBox") == true)
        {
            playerInsideTrigger = false;
        }


    }

    public void DealDamage(Transform attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            if (playerInsideTrigger)
            {
                //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the enemy's hit collider)
                targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(attacker.position, damageType, damage, attackPowerX, attackPowerY);

                //set target to null afterwards to prevent enemy from dealing damage to player without any collision
                targetTransform = null;
            }
            
        }
    }

    //this function is updated by the enemy's attack animations
    public void UpdateAttackValues(DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        tempDamageType = damageType;
        tempAttackDamage = damage;
        tempAttackPowerX = attackPowerX;
        tempAttackPowerY = attackPowerY;
    }
    private void ResetAttackValues()
    {
        tempDamageType = DamageType.none;
        tempAttackDamage = 0f;
        tempAttackPowerX = 0f;
        tempAttackPowerY = 0f;

    }
}
