using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fast Boss", menuName = "ScriptableObject/Fast Boss Configuration")]
public class FastBoss : EnemyScriptableObject
{
    [Header("Fast Boss Stats")]
    public float specialAttack1Cooldown;
    public string specialAttack2Animation;
    public float specialAttack2Chance;
    public float specialAttack2Cooldown;
    
    public override void AttackTarget(Animator animator, Transform target)
    {
        //float distanceX = Mathf.Abs(animator.transform.position.x - target.transform.position.x);
        //float distanceY = Mathf.Abs(animator.transform.position.y - target.transform.position.y);

        //Debug.Log("How often am I called?");
        // if special attack chance procs, then perform a special attack
        if (Random.value <= specialAttackChance)
        {
            //Debug.Log("Enemy performed a special attack!");
            animator.Play(specialAttackAnimation);
        }
        else if (Random.value <= specialAttack2Chance)
        {
            animator.Play(specialAttack2Animation);
        }

        else
        {
            // play common attack animation
            //Debug.Log("Enemy performed a common attack!");
            animator.Play(commonAttackAnimation);
        }



    }
}
