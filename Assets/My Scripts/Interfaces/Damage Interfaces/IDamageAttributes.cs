using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageAttributes
{
    //will move the gameobject using force by powerX and powerY
    public void JoltThisObject(bool directionIsRight, float powerX, float powerY);

    // set the entity's gravity scale to the given argument
    // air attacks will generally have lower gravity counts
    public void SetEntityGravity(float amountOfGravity);

    //determines the knock back effect applied on the enemy
    public enum DamageType
    {
        none, light, heavy, air
    }

}
