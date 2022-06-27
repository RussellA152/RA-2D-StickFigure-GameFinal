using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Fast Enemy1", menuName = "ScriptableObject/Fast Enemy Configuration")]
public class FastEnemySO : EnemyScriptableObject
{
    public void AttackTarget(Transform target)
    {
        if (animator != null)
        {
            //if enemy is not on attack cooldown... let them perform an attack
            if (!onCooldown)
                animator.Play(attackAnimation1);
            else
                return;

            //if the attack cooldown is on going, don't call it again
            //if (!cooldownCoroutineStarted)
                //StartCoroutine(AttackCooldown());
            //else
                //return;
        }
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public IEnumerator AttackCooldown()
    {
        //Debug.Log("enemy attack cooldown started!");

        cooldownCoroutineStarted = true;
        onCooldown = true;

        yield return new WaitForSeconds(attackCooldownTimer);

        onCooldown = false;
        cooldownCoroutineStarted = false;

        //Debug.Log("enemy attack cooldown finished!");
    }
}
