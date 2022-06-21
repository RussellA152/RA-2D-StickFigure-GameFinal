using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackAnimation
{
    //will move the gameobject using force by powerX and powerY
    public void JoltThisObject(bool directionIsRight, float powerX, float powerY);

}

//determines the knock back effect applied on the enemy
public enum DamageType
{
    none, light, heavy, air
}
