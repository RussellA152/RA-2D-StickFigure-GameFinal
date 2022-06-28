using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Fast Enemy", menuName = "ScriptableObject/Fast Enemy Configuration")]
public class FastEnemySO : EnemyScriptableObject
{
    public override void AttackTarget(Transform target)
    {
        Debug.Log("Attack in Fast Enemy!");
        Debug.Log("Coroutine started = " + attackCooldownCoroutineStarted);

        if (animator != null)
        {
            //if enemy is not on attack cooldown... let them perform an attack
            
            if (!attackOnCooldown)
                animator.Play(attackAnimation1);
            else
                return;
        }
    }

    public override IEnumerator AttackCooldown()
    {
        attackCooldownCoroutineStarted = true;
        attackOnCooldown = true;

        Debug.Log("Attack coroutine STARTED!");

        yield return new WaitForSeconds(attackCooldownTimer);

        attackOnCooldown = false;
        attackCooldownCoroutineStarted = false;

        Debug.Log("Attack coroutine ENDED!");
    }


}
