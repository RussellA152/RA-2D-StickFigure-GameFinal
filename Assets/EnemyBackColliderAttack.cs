using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBackColliderAttack : MonoBehaviour, IDamageDealing
{
    [SerializeField] private EnemyHurt enemyHurtScript;

    

    //[SerializeField] private BoxCollider2D backCollider; // the back collider of this enemy
    [SerializeField] private BoxCollider2D myHurtCollider; // the hurt collider of this enemy

    [SerializeField] private Rigidbody2D rb; // rigidbody is needed to check velocity

    private Transform targetTransform; //the gameobject inside of the enemy's hit collider
    private bool enemyInsideTrigger; // is an enemy inside of enemy's hit collider?

    [Header("Damage Type")]
    public IDamageAttributes.DamageType damageType;

    [Header("Damage & Force")]
    [SerializeField] private float damageOnCollision; // how much damage does the other enemy take when they collide with this knocked down enemy?
    [SerializeField] private float forceOnCollisionX; // how much X force is applied to the other enemy take when they collide with this knocked down enemy?
    [SerializeField] private float forceOnCollisionY; // how much Y force is applied to the other enemy take when they collide with this knocked down enemy?

    [Header("Velocity Needed For Collision")]
    [SerializeField] private float velocityRequirementX; // when enemy is knocked down, their X velocity must be high enough to knock down other enemies behind them
    [SerializeField] private float velocityRequirementY; // when enemy is knocked down, their Y velocity must be high enough to knock down other enemies behind them

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //update our target
        targetTransform = collision.transform;

        // did the enemy's back collider collide with a DIFFERENT enemy?
        // is the enemy NOT colliding with themself?
        // is enemy knocked down?
        // and is the velocity of enemy high enough to hurt another enemy?
        if (targetTransform.CompareTag("EnemyHurtBox") && targetTransform.gameObject != this.myHurtCollider && enemyHurtScript.GetIsKnockedDown() && (Mathf.Abs(rb.velocity.x) >= velocityRequirementX || Mathf.Abs(rb.velocity.y) >= velocityRequirementY))
        {
            //the hurtbox is a child of the player, so set the target equal to the parent
            targetTransform = targetTransform.parent;


            enemyInsideTrigger = true;

            // if the enemy that was collided with was not in a hurt state (apply a force on them)
            if(targetTransform.GetComponent<EnemyController>().GetEnemyState() != EnemyController.EnemyState.Hurt)
            {
                
                DealDamage(this.transform, damageType, damageOnCollision, forceOnCollisionX, forceOnCollisionY);
            }
            // if the enemy that was collided with was in a hurt state (do not apply any force on them)
            else
            {
                DealDamage(this.transform, damageType, damageOnCollision, 0f, 0f);
            }
                
        }
        else
        {
            enemyInsideTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        //if there is no target to be found... or the hit collider leaves the player's hurt box...
        // then set playerInsideTrigger to false
        if (targetTransform == null || targetTransform.CompareTag("EnemyHurtBox") == true)
        {
            enemyInsideTrigger = false;

        }


    }


    public void DealDamage(Transform attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            if (enemyInsideTrigger)
            {
                //Debug.Log("I am " + this.transform.parent.gameObject.name + " Hurt COLLIDER Deal damage to " + targetTransform.gameObject.name);

                // if this enemy is falling down, then apply a negative Y force to the other enemy
                if (rb.velocity.y <= -1 * velocityRequirementY)
                    attackPowerY = -1 * attackPowerY;

                //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the enemy's hit collider)
                targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(attacker.position, damageType, damage, attackPowerX, attackPowerY);

                //set target to null afterwards to prevent enemy from dealing damage to player without any collision
                targetTransform = null;
            }

        }
    }

    public BoxCollider2D GetHitBox()
    {
        return myHurtCollider;
    }
}
