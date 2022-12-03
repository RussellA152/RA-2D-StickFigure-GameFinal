using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    //function that checks if the entity is dead
    void CheckHealth(float health);

    //function that will change the entity's health by a certain amount (could decrease it, or increase it)
    void ModifyHealth(float amount);

    //function that will change the entity's max health cap by a certain amount (could decrease it, or increase it)
    //void ModifyMaxHealth(float amount);

    //return the health value of this entity
    float GetHealth();

    // return the maximum amount of health this entity can have
    float GetMaxHealth();

    //return a bool representing whether the player is alive or not 
    bool CheckIfAlive();

}
