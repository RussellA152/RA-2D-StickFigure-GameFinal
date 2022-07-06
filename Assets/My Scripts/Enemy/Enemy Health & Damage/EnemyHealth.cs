using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [Header("Required Components")]
    [SerializeField] private EnemyController enemyControllerScript;

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [HideInInspector]
    public float enemyHealth; //the health value of this enemy (DERIVED FROM SCRIPTABLEOBJECT)
    private bool hasDied; //has the enemy died?



    // this function is called inside of EnemyAttacks
    public void InitializeHealthProperties(EnemyScriptableObject scriptableObject)
    {
        enemyScriptableObject = scriptableObject;

        hasDied = false;

        //set basic values equal to the ScriptableObject's values
        if(enemyScriptableObject != null)
            enemyHealth = enemyScriptableObject.maxHealth;
        else
            Debug.Log("This enemy doesn't have a scriptable object! Inside Health Script*");
        

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
            hasDied = true;
            Debug.Log(this.gameObject.name + " has died!");

        }
    }

    public void ModifyHealth(float amount)
    {
        //add by a negative number if we should decrease health
        enemyHealth += amount;

    }

    public float GetHealth()
    {
        return enemyHealth;
    }

    public bool CheckIfDead()
    {
        return hasDied;
    }
}
