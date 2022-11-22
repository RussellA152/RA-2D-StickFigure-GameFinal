using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackValues : ScriptableObject
{
    [Header("Damage & Force")]
    [SerializeField] private float attackDamage; // damage of the attack
    [SerializeField] private float attackingPowerX; // amount of force applied to enemy that is hit by this attack in x-direction
    [SerializeField] private float attackingPowerY; // amount of force applied to enemy that is hit by this attack in y-direction

    [Header("Particle Effect Values")]
    [SerializeField] private float particleEffectDuration; // duration that the attack particle effect will play for

    [Header("Screenshake Values")]
    [SerializeField] private float screenShakePower; // amount of screenshake to apply
    [SerializeField] private float screenShakeDuration; // duration of screenshake

    [SerializeField] private float hitStopDelay; // how long will hitstop delay last?

    [Header("Jolt Force Applied To Entity")]
    [SerializeField] private ForceMode2D forceMode; // the force mode applied to the jolt force
    [SerializeField] private float joltForceX; //determines how far the player will 'jolt' forward in the x-direction when attacking (Should be a high value)
    [SerializeField] private float joltForceY; //determines how far the player will 'jolt' forward in the y-direction when attacking (Should be a high value)

    [Header("Gravity During Animation")]
    [SerializeField] private float gravityDuringAttack; // how much gravity is applied to player during this attack?  

    //private int isGroundedHash; //hash value for animator's isGrounded parameter (for performance)

    [SerializeField] private bool turnOffEnemyCollision; // should this attack tell player's colliders to ignore enemy collision?

    [SerializeField] private bool resetYVelocityOnAttack; // will the player's rigidbody Y velocity reset when they attack?
}
