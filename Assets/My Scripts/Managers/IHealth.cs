using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    //function that checks if the entity is dead
    void CheckHealth(float health);

    //function that will change the entity's health by a certain amount (could decrease it, or increase it)
    void ModifyHealth(float amount);
}
