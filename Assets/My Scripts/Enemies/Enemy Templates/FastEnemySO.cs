using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Fast Enemy", menuName = "ScriptableObject/Fast Enemy Configuration")]
public class FastEnemySO : EnemyScriptableObject
{

    //this function will probably do more later, but for now, it just plays one animation
    public override void AttackTarget(Animator animator, Transform target)
    {
        //Debug.Log("How often am I called?");
        // if special attack chance procs, then perform a special attack
        if (Random.value <= specialAttackChance)
        {
            Debug.Log("Enemy performed a special attack!");
            animator.Play(specialAttackAnimation);
        }
        else
        {
            // play common attack animation
            Debug.Log("Enemy performed a common attack!");
            animator.Play(commonAttackAnimation);
        }
        
        
        
    }

}
