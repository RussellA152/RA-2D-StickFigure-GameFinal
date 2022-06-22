using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//we will use an interface for enemy attacks because not every enemy will share the same attacking behavior
public interface IAIAttacks
{
    //attack the player in some way..
    public void AttackTarget(Transform target);

    //retrieve attacking values and numbers from scriptable object
    public void InitializeAttackProperties();

    //return attackRange to EnemyController
    public float GetAttackRange();

    //Start a cooldown coroutine after enemy attacks
    IEnumerator AttackCooldown();
}
