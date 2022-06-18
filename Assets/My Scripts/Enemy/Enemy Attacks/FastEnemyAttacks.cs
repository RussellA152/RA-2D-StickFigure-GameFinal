using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyAttacks : MonoBehaviour, IEnemyAttacks
{

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    private float attackRange; //range that enemy can attack player (derived from EnemyScriptableObject)


    public void AttackTarget(Transform target)
    {
        float chanceToAttack = Random.Range(-10, 10);

        //Debug.Log(chanceToAttack);

        //if(chanceToAttack % 5 == 0)
            //Debug.Log("Enemy attack target!");
    }

    //this function is called inside of EnemyController
    public void SetUpEnemyAttackConfiguration()
    {
        attackRange = enemyScriptableObject.attackRange;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }
}
