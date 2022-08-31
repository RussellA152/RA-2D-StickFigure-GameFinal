using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//any gameobject that is able to be damaged: player, enemy, chest
// should have their hurt colliders implement this script
public interface IDamageable
{
    // when the gameobject is attacked... make them take damage and apply force to them in a certain direction
    public void OnHurt(Vector3 attacker, IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY);

    // when the gameobject is attacked... make them take damage that will be subtracted to their health (this function is typically called by OnHurt())
    public void TakeDamage(float damage, float attackPowerX, float attackPowerY);

    //plays the hurt animation depending on the damage type and direction of the attack
    //takes in an animationHash value using StringToHash
    public void PlayHurtAnimation(int animationHash);
}
