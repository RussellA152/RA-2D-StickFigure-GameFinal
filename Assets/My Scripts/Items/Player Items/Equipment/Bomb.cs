using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{

    [SerializeField] private ItemHitCollider itemHitColliderScript;
    [SerializeField] private Animator animator;

    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private BoxCollider2D hitbox;
    [SerializeField] private string animationName;

    [SerializeField] private Vector3 facingRightVector; //vector3 representing the enemy facing the right direction
    [SerializeField] private Vector3 facingLeftVector; //vector3 representing the enemy facing the left direction

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
            transform.position = PlayerStats.instance.GetPlayer().transform.position;


            rb.isKinematic = false;
            //PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
            animator.Play(animationName);
            //itemHitColliderScript.ShakeScreen();

            if (PlayerStats.instance.ReturnPlayerDirection())
            {
                //rb.isKinematic = false;
                rb.AddForce(new Vector2(myScriptableObject.throwForceX, myScriptableObject.throwForceY));
                transform.localScale = facingRightVector;
            }

            else
            {
                //rb.isKinematic = false;
                rb.AddForce(new Vector2(-myScriptableObject.throwForceX, myScriptableObject.throwForceY));
                transform.localScale = facingLeftVector;
            }



        }

    }

    public override void InitializeValues()
    {
        //animator.enabled = false;
        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        usageCooldown = myScriptableObject.usageCooldown;

        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;
        maxAmountOfCharge = myScriptableObject.maxAmountOfCharge;
        amountOfCharge = myScriptableObject.maxAmountOfCharge;

        itemHitColliderScript.damageType = myScriptableObject.damageType;
        itemHitColliderScript.attackDamage = myScriptableObject.damageOfItem;
        itemHitColliderScript.attackPowerX = myScriptableObject.attackPowerX;
        itemHitColliderScript.attackPowerY = myScriptableObject.attackPowerY;

        itemHitColliderScript.screenShakeDuration = myScriptableObject.screenShakePower;
        itemHitColliderScript.screenShakeDuration = myScriptableObject.screenShakeDuration;
    }

}
