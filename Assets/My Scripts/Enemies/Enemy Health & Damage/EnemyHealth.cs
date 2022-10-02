using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    [Header("Required Components")]
    [SerializeField] private EnemyController enemyControllerScript;

    [Header("Enemy Configuration Scriptable Object")]
    public EnemyScriptableObject enemyScriptableObject;

    [SerializeField] private Animator animator;

    [HideInInspector]
    public float enemyMaxHealth; //the max health value of this enemy(DERIVED FROM SCRIPTABLEOBJECT)
    private float enemyHealth; //the current health value of this enemy 
    private bool isAlive; //is the enemy alive?

    [SerializeField] private string deathAnimationStartName; // the name of the death animation start

    private bool playedDeathAnimation = false;

    [SerializeField] private BoxCollider2D collider;



    // this function is called inside of EnemyAttacks
    public void InitializeHealthProperties(EnemyScriptableObject scriptableObject)
    {
        enemyScriptableObject = scriptableObject;

        isAlive = true;

        //set basic values equal to the ScriptableObject's values
        if(enemyScriptableObject != null)
        {
            enemyMaxHealth = enemyScriptableObject.maxHealth;
            enemyHealth = enemyMaxHealth;
        }
            
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
            isAlive = false;
            Debug.Log(this.gameObject.name + " has died!");

            // if this enemy hasn't played their death animation, play it when they die (only once)
            if (!playedDeathAnimation)
            {
                // play death animation
                animator.Play(deathAnimationStartName);
                playedDeathAnimation = true;
            }
                
        }
    }

    public void ModifyHealth(float amount)
    {
        //add by a negative number if we should decrease health
        enemyHealth += amount;

    }

    //public void ModifyMaxHealth(float amount)
    //{
        //add by a negative number if we should decrease health
        //enemyHealth += amount;

    //}

    public float GetHealth()
    {
        return enemyHealth;
    }

    public bool CheckIfAlive()
    {
        return isAlive;
    }
}
