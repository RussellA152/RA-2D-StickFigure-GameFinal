using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//we will use an interface for enemy attacks because not every enemy will share the same attacking behavior
public interface IAIAttacks
{
    //attack the player in some way..
    public void AttackTarget(Animator animator, Transform target);

    //return attackRange in x direction to EnemyController
    public float GetAttackRangeX();

    //return attackRange in y direction to EnemyController
    public float GetAttackRangeY();

    //Start a cooldown coroutine after enemy attacks
    IEnumerator AttackCooldown();
}
