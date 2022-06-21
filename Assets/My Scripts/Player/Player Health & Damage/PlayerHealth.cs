using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHealth
{
    [SerializeField] private float playerHealth; 

    private void Start()
    {
        
    }

    private void Update()
    {

        //check if enemy was killed..
        CheckHealth(playerHealth);

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
        playerHealth += amount;

    }
}
