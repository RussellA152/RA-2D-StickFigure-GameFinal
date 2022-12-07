using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private Animator animator;

    private float playerMaxHealth; //the maximum health the player can have
    [SerializeField] private float playerCurrentHealth; //the current health of the player

    private bool isAlive; //is the player alive?

    private int isAliveHash; // the "isAlive" parameter from the player's animator controller

    public Action onPlayerDeath; // player is dead, but can be saved

    public Action playerIsCompletelyDead; // player is officially dead

    private bool checkIfDeadCompletely = false;


    private void Start()
    {
        onPlayerDeath += CheckIfFullyDead;


        //set current health to max health at beginning of game
        playerMaxHealth = PlayerStats.instance.GetMaxHealth();
        playerCurrentHealth = playerMaxHealth;

        isAlive = true;

        isAliveHash = Animator.StringToHash("isAlive");

        animator.SetBool(isAliveHash, isAlive);


    }

    private void Update()
    {

        //check if enemy was killed..
        CheckHealth(playerCurrentHealth);

    }

    public void CheckHealth(float health)
    {
        playerMaxHealth = PlayerStats.instance.GetMaxHealth();
        animator.SetBool(isAliveHash, isAlive);

        //if player reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if (health <= 0f)
        {
            //Debug.Log(this.gameObject.name + " has died!");

            
            isAlive = false;

            //animator.SetBool(isAliveHash, isAlive);
            OnPlayerDeath();


        }
        else
        {
            isAlive = true;

            
        }
    }

    //called whenever player's current health is to be increased or decreased
    //ex: player picks up health, increase it
    //ex: player is attacked, decrease it
    public void ModifyHealth(float amount)
    {
        if(playerCurrentHealth + amount >= playerMaxHealth)
        {
            playerCurrentHealth = playerMaxHealth;
        }
        else
        {
            //add by a negative number if we should decrease health
            playerCurrentHealth += amount;
        }
            

        

    }

    // return true or false if player's current health is full
    public bool IsCurrentHealthFull()
    {
        return playerCurrentHealth == playerMaxHealth;
    }

    //Obsolete
    //public void ModifyMaxHealth(float amount)
    //{
        //add by a negative number if we should decrease health
        //playerMaxHealth += amount;

    //}


    public float GetHealth()
    {
        return playerCurrentHealth;
    }

    public float GetMaxHealth()
    {
        return playerMaxHealth;
    }

    public bool CheckIfAlive()
    {
        return isAlive;
    }

    private void OnPlayerDeath()
    {
        //isAlive = false;

        //animator.SetBool(isAliveHash, isAlive);

        // invoke onPlayerDeath eventSystem
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }

    public void CheckIfFullyDead()
    {
        if(!checkIfDeadCompletely)
            StartCoroutine(CheckIfFullyDeadAfterTime());
    }

    // if the player is dead for more than 5 seconds, then they are officially dead
    IEnumerator CheckIfFullyDeadAfterTime()
    {
        checkIfDeadCompletely = true;
        yield return new WaitForSeconds(5f);

        if (playerCurrentHealth <= 0)
        {
            playerIsCompletelyDead();
            Debug.Log("PLAYER IS COMPLETLY DEAD!");
        }
            

        checkIfDeadCompletely = false;

    }

    private void OnDisable()
    {
        onPlayerDeath -= CheckIfFullyDead;
    }
}
