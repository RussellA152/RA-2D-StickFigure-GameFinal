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
    public float followRangeX; //x distance enemy must be to chase player
    public float followRangeY; //y distance enemy must be to chase player

    //[Header("Basic Animation Properties")]
    //public Sprite sprite; //while we could just set the sprite renderer in the inspector... when the enemy dies, they will be disabled, and they should reappear on scene with the idle sprite
    //public Animator animatorController; // the animator and the controlle the enemy will use

    [Header("Attack Properties")]
    public float attackRangeX; //how far enemy can be to attack player in x-direction
    public float attackRangeY; //how far enemy can be to attack player in y-direction
    public float attackCooldownTimer; //time between each attack

    [Header("Rigidbody Properties")]
    public float rbMass;

    [Header("Animator Controller")]
    public RuntimeAnimatorController enemyAnimatorController; // the animator controller that this enemy will use (determines animation clips that enemy will play)

    [Header("Attack Animations")]
    [SerializeField] public string lightAttackAnimation;

    //Animations to play when enemy is hit by a light attack (depends on the direction of the light attack)
    [Header("Enemy Light Attack Hurt Animation Names")]
    [SerializeField] public string lightHurtAnimFront;
    [SerializeField] public string lightHurtAnimBehind;

    //Animations to play when enemy is hit by a heavy attack (depends on the direction of the heavy attack)
    [Header("Enemy Heavy Attack Hurt Animation Names")]
    [SerializeField] public string heavyHurtAnimFront;
    [SerializeField] public string heavyHurtAnimBehind;

    public virtual IEnumerator AttackCooldown()
    {
        throw new System.NotImplementedException();
    }

    public virtual void AttackTarget(Animator animator, Transform target)
    {
        throw new System.NotImplementedException();
    }

    public float GetAttackRangeX()
    {
        return attackRangeX;
    }

    public float GetAttackRangeY()
    {
        return attackRangeY;
    }

}
