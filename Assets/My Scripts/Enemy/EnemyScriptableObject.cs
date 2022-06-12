using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ScriptableObject that holds the base stats for an enemy. These can then be modified at object creation time to buff up enemies or something
// and to reset their stats if they died or modified during runtime

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Basic Enemy Properties")]
    public float maxHealth;

    public float walkingSpeed;

    public Sprite sprite; //while we could just set the sprite renderer in the inspector... when the enemy dies, they will be disabled, and they should reappear on scene with the idle sprite 

    [Header("Rigidbody Properties")]
    public float rbMass;

}
