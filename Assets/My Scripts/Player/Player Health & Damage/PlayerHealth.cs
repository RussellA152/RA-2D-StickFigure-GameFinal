using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private float playerMaxHealth; //the maximum health the player can have
    private float playerCurrentHealth; //the current health of the player

    private void Start()
    {
        //set current health to max health at beginning of game
        playerMaxHealth = PlayerStats.instance.GetMaxHealth();
        playerCurrentHealth = playerMaxHealth;
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
            Debug.Log(this.gameObject.name + " has died!");
            gameObject.SetActive(false);

        }
    }

    //called whenever player's current health is to be increased or decreased
    //ex: player picks up health, increase it
    //ex: player is attacked, decrease it
    public void ModifyHealth(float amount)
    {
        //add by a negative number if we should decrease health
        playerCurrentHealth += amount;

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
}
