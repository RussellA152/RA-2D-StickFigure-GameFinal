using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [HideInInspector]
    public float enemyHealth; //the health value of this enemy (DERIVED FROM SCRIPTABLEOBJECT)

    private void OnEnable()
    {
        //we are setting up the values for the enemy inside of OnEnable because we will use object pooling for killing enemies
        SetupEnemyFromConfiguration();

    }


    // can be virtual, but it won't be for now... (if virtual, then enemies could override this function for subtyping)
    //sets all base values equal to 
    public void SetupEnemyFromConfiguration()
    {

        //set basic values equal to the ScriptableObject's values
        enemyHealth = enemyScriptableObject.maxHealth;

    }


    private void Update()
    {

        //check if enemy was killed..
        CheckHealth();

    }


    public void CheckHealth()
    {
        //if enemy reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if (enemyHealth <= 0f)
        {
            Debug.Log(this.gameObject.name + " has died!");
            gameObject.SetActive(false);

        }
    }
}
