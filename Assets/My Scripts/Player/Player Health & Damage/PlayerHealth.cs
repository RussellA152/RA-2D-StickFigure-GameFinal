using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private Animator animator;

    private float playerMaxHealth; //the maximum health the player can have
    private float playerCurrentHealth; //the current health of the player

    private bool isAlive; //is the player alive?

    private int isAliveHash; // the "isAlive" parameter from the player's animator controller

    public Action onPlayerDeath;


    private void Start()
    {
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

        //if player reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if (health <= 0f)
        {
            //Debug.Log(this.gameObject.name + " has died!");

            OnPlayerDeath();
            //isAlive = false;

            //animator.SetBool(isAliveHash, isAlive);


        }
        else
        {
            isAlive = true;

            animator.SetBool(isAliveHash, isAlive);
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

    public bool CheckIfAlive()
    {
        return isAlive;
    }

    private void OnPlayerDeath()
    {
        isAlive = false;

        animator.SetBool(isAliveHash, isAlive);

        // invoke onPlayerDeath eventSystem
        if (onPlayerDeath != null)
        {
            onPlayerDeath();
        }
    }
}
