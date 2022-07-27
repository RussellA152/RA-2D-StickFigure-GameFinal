using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float playerMaxHealth; //the maximum health the player can have
    private float playerCurrentHealth; //the current health of the player

    private void Start()
    {
        //set current health to max health at beginning of game
        playerCurrentHealth = playerMaxHealth;
    }

    private void Update()
    {

        //check if enemy was killed..
        CheckHealth(playerCurrentHealth);

    }

    public void CheckHealth(float health)
    {
        //if player reaches 0 health, disable their game object (FOR NOW, WE WILL USE OBJECT POOLING LATER)
        if (health <= 0f)
        {
            Debug.Log(this.gameObject.name + " has died!");
            gameObject.SetActive(false);

        }
    }

    public void ModifyHealth(float amount)
    {
        //add by a negative number if we should decrease health
        playerCurrentHealth += amount;

    }

    public void ModifyMaxHealth(float amount)
    {
        //add by a negative number if we should decrease health
        playerMaxHealth += amount;

    }


    public float GetHealth()
    {
        return playerCurrentHealth;
    }
}
