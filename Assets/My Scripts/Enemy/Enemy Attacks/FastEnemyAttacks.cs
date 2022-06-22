using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyAttacks : MonoBehaviour, IAIAttacks
{

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [Header("Attack Animations")]
    [SerializeField] private string attackAnimation1;

    private float attackRange; //range that enemy can attack player (derived from EnemyScriptableObject)
    private float attackCooldownTimer; //how often the enemy can perform an attack (cooldown speed)

    private bool onCooldown = false; //is the enemy on cooldown to attack?
    private bool cooldownCoroutineStarted = false; //is the Attack Cooldown coroutine on going?

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void AttackTarget(Transform target)
    {
        //if enemy is not on attack cooldown... let them perform an attack
        if (!onCooldown)
            animator.Play(attackAnimation1);
        else
            return;

        //if the attack cooldown is on going, don't call it again
        if(!cooldownCoroutineStarted)
            StartCoroutine(AttackCooldown());
        else
            return;

    }

    //this function is called inside of EnemyController
    public void InitializeAttackProperties()
    {
        attackRange = enemyScriptableObject.attackRange;
        attackCooldownTimer = enemyScriptableObject.attackCooldownTimer;
    }

    public float GetAttackRange()
    {
        return attackRange;
    }

    public IEnumerator AttackCooldown()
    {
        Debug.Log("enemy attack cooldown started!");

        cooldownCoroutineStarted = true;
        onCooldown = true;

        yield return new WaitForSeconds(attackCooldownTimer);

        onCooldown = false;
        cooldownCoroutineStarted = false;

        Debug.Log("enemy attack cooldown finished!");
    }
}
