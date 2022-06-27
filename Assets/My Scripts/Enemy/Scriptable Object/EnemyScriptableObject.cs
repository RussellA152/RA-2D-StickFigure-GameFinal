using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]

public class EnemyScriptableObject : ScriptableObject, IAIAttacks
{
    [Header("Basic Enemy Properties")]
    public float maxHealth;
    public float walkingSpeed;
    public float followRange; //distance enemy must be to chase player

    [Header("Basic Animation Properties")]
    public Sprite sprite; //while we could just set the sprite renderer in the inspector... when the enemy dies, they will be disabled, and they should reappear on scene with the idle sprite
    //public Animator animatorController; // the animator and the controlle the enemy will use

    [Header("Attack Properties")]
    public float attackRange; //how far enemy can be to attack player
    public float attackCooldownTimer; //time between each attack

    [Header("Rigidbody Properties")]
    public float rbMass;

    [HideInInspector]
    public bool cooldownCoroutineStarted = false;
    [HideInInspector]
    public bool onCooldown = false;

    [Header("Animator Properties")]
    public Animator animator;

    [Header("Attack Animations")]
    [SerializeField] public string attackAnimation1;

    //Animations to play when enemy is hit by a light attack (depends on the direction of the light attack)
    [Header("Enemy Light Attack Hurt Animation Names")]
    [SerializeField] public string lightHurtAnimFront;
    [SerializeField] public string lightHurtAnimBehind;

    //Animations to play when enemy is hit by a heavy attack (depends on the direction of the heavy attack)
    [Header("Enemy Heavy Attack Hurt Animation Names")]
    [SerializeField] public string heavyHurtAnimFront;
    [SerializeField] public string heavyHurtAnimBehind;

    public IEnumerator AttackCooldown()
    {
        throw new System.NotImplementedException();
    }

    public void AttackTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public float GetAttackRange()
    {
        throw new System.NotImplementedException();
    }

    public void InitializeAttackProperties()
    {
        throw new System.NotImplementedException();
    }
}
