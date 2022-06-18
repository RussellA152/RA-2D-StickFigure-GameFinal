using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [HideInInspector]
    public float enemyHealth; //the health value of this enemy (DERIVED FROM SCRIPTABLEOBJECT)



    // this function is called inside of EnemyAttacks
    public void SetupEnemyHealthFromConfiguration()
    {
        //set basic values equal to the ScriptableObject's values
        enemyHealth = enemyScriptableObject.maxHealth;

    }

    private void Update()
    {

        //check if enemy was killed..
        CheckHealth(enemyHealth);

    }

    public void CheckHealth(float health)
    {
        //if enemy reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if (health <= 0f)
        {
            Debug.Log(this.gameObject.name + " has died!");
            gameObject.SetActive(false);

        }
    }
}
