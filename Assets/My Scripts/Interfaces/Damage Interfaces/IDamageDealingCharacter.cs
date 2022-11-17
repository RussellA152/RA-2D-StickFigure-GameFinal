using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Player and Enemy hit scripts will implement this interface
//unlike IDamageDealingItem, this interface contains UpdateAttackValues,
//which allows Hit collider scripts to fetch differing damage and attack force values
//based on a specific attack animation (Items won't have attack animations so they don't need this function)
public interface IDamageDealingCharacter : IDamageDealing
{
    // attack animations will update the damage values that the hit collider will apply to targets
    public void UpdateAttackValues(IDamageAttributes.DamageType damageType, float damage, float attackPowerX, float attackPowerY, float screenShakePower, float screenShakeDuration);
}
