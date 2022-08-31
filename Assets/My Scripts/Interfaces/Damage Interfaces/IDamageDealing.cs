using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Any class that outputs damage to an object, Player, or Enemy's health will implement this or a sub version
//of this interface
public interface IDamageDealing
{
    //function that is invoked by the hit collider's OnTriggerEnter function
    //only called when the target is inside of the hit collider's trigger
    public void DealDamage(Transform attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY);

    //return the hitbox (box collider 2D, which is typically needed by attack animations)
    public BoxCollider2D GetHitBox();

}