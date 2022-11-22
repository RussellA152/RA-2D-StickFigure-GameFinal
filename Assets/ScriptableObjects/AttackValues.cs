using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack Values Configuration", menuName = "ScriptableObject/Attack Values Configuration")]

public class AttackValues : ScriptableObject
{
    [Header("Damage Type")]
    public IDamageAttributes.DamageType damageType;

    [Header("Damage & Force")]
    public float attackDamage; // damage of the attack
    public float attackingPowerX; // amount of force applied to enemy that is hit by this attack in x-direction
    public float attackingPowerY; // amount of force applied to enemy that is hit by this attack in y-direction

    [Header("Particle Effect Values")]
    public float particleEffectDuration; // duration that the attack particle effect will play for

    [Header("Screenshake Values")]
    public float screenShakePower; // amount of screenshake to apply
    public float screenShakeDuration; // duration of screenshake

    [Header("Hitstop Values")]
    public float hitStopDelay; // how long will hitstop delay last?

    [Header("Jolt Force Applied To Entity")]
    public ForceMode2D forceMode; // the force mode applied to the jolt force
    public float joltForceX; //determines how far the player will 'jolt' forward in the x-direction when attacking (Should be a high value)
    public float joltForceY; //determines how far the player will 'jolt' forward in the y-direction when attacking (Should be a high value)

    [Header("Gravity During Attack")]
    public float gravityDuringAttack; // how much gravity is applied to player during this attack?  

    //private int isGroundedHash; //hash value for animator's isGrounded parameter (for performance)

    public bool turnOffEnemyCollision; // should this attack tell player's colliders to ignore enemy collision? (for enemies, they would ignore player's collision)

    public bool resetYVelocityOnAttack; // will the player's rigidbody Y velocity reset when they attack?
}
