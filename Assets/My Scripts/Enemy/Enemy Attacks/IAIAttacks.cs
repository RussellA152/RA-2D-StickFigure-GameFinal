using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//we will use an interface for enemy attacks because not every enemy will share the same attacking behavior
public interface IAIAttacks
{
    //attack the player in some way..
    public void AttackTarget(Animator animator, Transform target);

    //return attackRange to EnemyController
    public float GetAttackRange();

    //Start a cooldown coroutine after enemy attacks
    IEnumerator AttackCooldown();
}
