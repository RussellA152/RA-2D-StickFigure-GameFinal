using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageDealing
{
    public void DealDamage(Transform attacker, DamageType damageType, float damage, float attackPowerX, float attackPowerY);


}
