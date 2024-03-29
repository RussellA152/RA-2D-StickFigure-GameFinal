using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomstick : Item
{
    [SerializeField] private ItemHitCollider itemHitColliderScript;
    //[SerializeField] private SpriteRenderer
    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private Animator animator;
    
    [SerializeField] private string animationName;

    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction

    [SerializeField] private Vector3 offsetX; // offset applied to weapon

    private void Awake()
    {
        base.Awake();
        animator.enabled = false;
    }

    private void OnEnable()
    {
        base.OnEnable();
        animator.enabled = true;
    }

    private void OnDisable()
    {
        base.OnDisable();
        animator.enabled = false;
    }

    public override void ItemAction(GameObject player)
    {
        
        // Turn off player's hitbox for a few seconds
        if (ShouldActivate())
        {
            //animator.enabled = true;

            itemHitColliderScript.ShakeScreen(myScriptableObject.screenShakePower, myScriptableObject.screenShakeDuration);

            if (PlayerStats.instance.ReturnPlayerDirection())
            {
                transform.position = PlayerStats.instance.GetPlayer().transform.position + offsetX;
                transform.localScale = facingRightVector;
            }
                
            else
            {
                transform.position = PlayerStats.instance.GetPlayer().transform.position - offsetX;
                transform.localScale = facingLeftVector;
            }
                

            //PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
            animator.Play(animationName);
            PlayItemSound(itemActionSound);
            //animator.enabled = false;



        }

    }

    public override void InitializeValues()
    {
        
        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;
        maxAmountOfCharge = myScriptableObject.maxAmountOfCharge;
        amountOfCharge = myScriptableObject.maxAmountOfCharge;

        usageCooldown = myScriptableObject.usageCooldown;
        itemHitColliderScript.damageType = myScriptableObject.damageType;
        itemHitColliderScript.attackDamage = myScriptableObject.damageOfItem;
        itemHitColliderScript.attackPowerX = myScriptableObject.attackPowerX;
        itemHitColliderScript.attackPowerY = myScriptableObject.attackPowerY;

        itemActionSound = myScriptableObject.itemActionSound;
    }

    //public void DealDamage(Transform attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY)
    //{
    //    PlayerStats.instance.GetPlayer();
    //}

    //public void PlayParticleEffect(float duration, Vector2 positionOfParticle)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public BoxCollider2D GetHitBox()
    //{
    //    return hitbox;
    //}
}
