using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Fast Enemy", menuName = "ScriptableObject/Fast Enemy Configuration")]
public class FastEnemySO : EnemyScriptableObject
{
    [Header("Fast Enemy Special Attack Animation Name")]
    [SerializeField] public string fastEnemySpecialAttack; //a special attack that the fast enemy could do (A barage of punches)

    //this function will probably do more later, but for now, it just plays one animation
    public override void AttackTarget(Animator animator, Transform target)
    {
        // play the attack animation
        animator.Play(lightAttackAnimation);
        
        
    }

}
