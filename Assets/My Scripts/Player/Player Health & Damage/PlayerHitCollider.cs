using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


// This script should be placed on the hit box of the player
// when the player performs an attack animation, the damage values of this script will be updated
// and when the enemy is inside the box collider's trigger, call the deal damage function that will apply the damage and force on the enemy
public class PlayerHitCollider : MonoBehaviour, IDamageDealingCharacter
{
    //public AttackValues statsScriptableObject;

    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] private HitStop hitStopScript;

    [SerializeField] private BoxCollider2D hitbox;

    
    [SerializeField] private ParticleSystem particleSys; // particle system used for playing particle effects when damaging something
    [SerializeField] private ParticleSystemRenderer particleRenderer; // the renderer of the hit effect particle system
    private ParticleSystem.MainModule psMain;

    private Transform targetTransform;// the transform of who enters our hitbox collider

    private bool enemyInsideTrigger; //is the enemy inside of our hitbox collider?


    //temporary damage values updated by the attack animation
    private IDamageAttributes.DamageType tempDamageType;
    private float tempAttackDamage;

    private float tempAttackPowerX;
    private float tempAttackPowerY;

    private float particleEffectDuration;
    private Material particleEffectMaterial;

    private float tempScreenShakePower;
    private float tempScreenShakeDuration;

    //private int tempHitStopRestoreTimer;
    private float tempHitStopDelay;

    private int ignorePlayerLayer;

    private void Start()
    {
        ignorePlayerLayer = LayerMask.NameToLayer("IgnorePlayer");

        psMain = particleSys.main;

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Detected something!");
        //update our target
        targetTransform = collision.transform;

        //checking if trigger collided with EnemyHurtBox tag (located only on the hurtbox child gameobject on each enemy)
        if (targetTransform.CompareTag("EnemyHurtBox") && targetTransform.transform.parent.gameObject.layer != ignorePlayerLayer)
        {
            //Debug.Log("Detected enemy collision!");

            //the hurtbox is a child of the enemy, so set the target equal to the parent
            targetTransform = targetTransform.parent;

            Debug.Log("Hit an enemy!  " + targetTransform.gameObject.name);

            enemyInsideTrigger = true;

            //DealDamage(transform.parent, statsScriptableObject.damageType, statsScriptableObject.attackDamage, statsScriptableObject.attackingPowerX, statsScriptableObject.attackingPowerY);

            //now that enemy is inside the trigger, call the deal damage function
            DealDamage(transform.parent, tempDamageType, tempAttackDamage, tempAttackPowerX, tempAttackPowerY);

            //particleSys.transform.position = collision.gameObject.GetComponent<BoxCollider2D>().ClosestPoint(transform.position);
            //ResetAttackValues();
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

            //ResetAttackValues();
        }
    }

    public void DealDamage(Transform attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    {
        if (targetTransform != null)
        {
            if (enemyInsideTrigger)
            {
                Debug.Log("Deal damage to " + targetTransform.gameObject.name);
                //calls the receiver's OnHurt function which will apply the damage and force of this attack (receiverWasPlayer is false because this is the player's hit collider)
                targetTransform.gameObject.GetComponent<IDamageable>().OnHurt(attacker.position, damageType, damage, attackPowerX, attackPowerY);

                // formula for making attacks with high attack power have longer hitstops
                //int hitStopRestoreTime = (int) Mathf.Abs((50 / attackPowerY) * 10000);
                //Debug.Log("Restore Time: " +  hitStopRestoreTime);

                // clear any screenshakes that occured previously (this is so screenshakes don't stack)
                // only for player's attacks
                CinemachineImpulseManager.Instance.Clear();

                // the closest point from this hitbox to the enemy's collider
                Vector2 particlePosition = targetTransform.gameObject.GetComponent<Collider2D>().ClosestPoint(transform.position);

                //Debug.Log("Player hit enemy!");

                PlayParticleEffect(particleEffectDuration, particlePosition);

                // do a hitstop when landing an attack
                hitStopScript.Stop(tempHitStopDelay);

                // hitStopScript.StopTime(0.05f, tempHitStopRestoreTimer, tempHitStopDelay);

                // change duration of the screenshake 
                impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = tempScreenShakeDuration;
                //impulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = statsScriptableObject.screenShakeDuration;

                // generate an impulse to shake the screen
                impulseSource.GenerateImpulse(tempScreenShakePower);
                //impulseSource.GenerateImpulse(statsScriptableObject.screenShakePower);

                //set target to null afterwards to prevent player from dealing damage to enemy without any collision
                targetTransform = null;
            }

        }
    }

    public void PlayParticleEffect(float duration, Vector2 positionOfParticle)
    {
        particleRenderer.material = particleEffectMaterial;

        particleSys.transform.position = positionOfParticle;

        particleSys.Stop();

        psMain.duration = duration;

        particleSys.Play();
    }

    ////will move the gameobject using force by powerX and powerY
    //public void JoltThisObject(bool directionIsRight, float powerX, float powerY)
    //{

    //}

    //// set the entity's gravity scale to the given argument
    //// air attacks will generally have lower gravity counts
    //public void SetEntityGravity(float amountOfGravity) {

    //}

    public void SetParticleEffectDetails(Material material, float duration)
    {
        particleEffectMaterial = material;
        particleEffectDuration = duration;
    }

    //this function is updated by the player's attack animations
    public void UpdateAttackValues(IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY, float screenShakePower, float screenShakeDuration)
    {
        tempDamageType = damageType;
        tempAttackDamage = damage;
        tempAttackPowerX = attackPowerX;
        tempAttackPowerY = attackPowerY;
        tempScreenShakePower = screenShakePower;
        tempScreenShakeDuration = screenShakeDuration;
    }

    public void SetHitStopValues(float delay)
    {
        //tempHitStopRestoreTimer = restoreTime;
        tempHitStopDelay = delay;
    }

    private void ResetAttackValues()
    {
        tempDamageType = IDamageAttributes.DamageType.none;
        tempAttackDamage = 0f;
        tempAttackPowerX = 0f;
        tempAttackPowerY = 0f;
        tempScreenShakePower = 0f;
       // tempHitStopRestoreTimer = 0;

    }

    //public void JoltThisObject(bool directionIsRight, float powerX, float powerY)
    //{
    //    // if this attack is supposed to reset y velocity on attack
    //    if (resetYVelocityOnAttack)
    //        playerComponentScript.GetRB().velocity = Vector2.zero;

    //    if (directionIsRight)
    //        rb.AddForce(new Vector2(powerX, powerY), forceMode);
    //    //if player is facing left, then multiply force by negative 1 to prevent player from jolting backwards
    //    else
    //        rb.AddForce(new Vector2(-powerX, powerY), forceMode);
    //}

    public BoxCollider2D GetHitBox()
    {
        return hitbox;
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
