using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//we will use an interface for enemy attacks because not every enemy will share the same attacking behavior
public interface IEnemyAttacks
{

    public void AttackTarget(Transform target);


    public void SetUpEnemyAttackConfiguration();

    public float GetAttackRange();
}
