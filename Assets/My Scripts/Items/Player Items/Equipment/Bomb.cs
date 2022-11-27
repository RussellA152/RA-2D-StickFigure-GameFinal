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

    public override void ItemAction(GameObject player)
    {
        // Turn off player's hitbox for a few seconds
        if (ShouldActivate())
        {
            transform.position = PlayerStats.instance.GetPlayer().transform.position;
            

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
                

            //PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
            animator.Play(animationName);



        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        usageCooldown = myScriptableObject.usageCooldown;
        itemHitColliderScript.damageType = myScriptableObject.damageType;
        itemHitColliderScript.attackDamage = myScriptableObject.damageOfItem;
        itemHitColliderScript.attackPowerX = myScriptableObject.attackPowerX;
        itemHitColliderScript.attackPowerY = myScriptableObject.attackPowerY;
    }

}
