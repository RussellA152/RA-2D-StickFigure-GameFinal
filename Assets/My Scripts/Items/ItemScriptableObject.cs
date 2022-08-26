using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Configuration", menuName = "ScriptableObject/Item Configuration")]
public class ItemScriptableObject : ScriptableObject
{
    public string itemName;

    public ItemType itemType;

    [Header("Charge Properties")]
    public int amountOfCharge; //how many charges does this item have

    public int chargesConsumedPerUse; //how much charge does this item use, on each activation

    public int chargeRefilledPerKill; //how much charge is refilled for each enemy kill


    [Header("Proc Chance Properties")]
    public float procChance; //what is the chance of this item activating automatically

    public float procChanceModifier; //an increase or decrease to all passive item's proc chance

    [Header("Health Properties")]
    public float maxHealthModifier; //an increase or decrease to the player's max health

    public float currentHealthModifier; //an increase or decrease to the player's current health

    [Header("Money Properties")]
    public float amountOfMoneyGiven; //how much money does this item give to the player

    public float moneyMultiplierModifier; //an increase or decrease to the amount of money the player picks up

    [Header("Other Properties")]
    public float damageOfItem; //the damage value of this item

    public float rangeOfItem; //how far this item will go from the player

    public float damageMultiplierModifier; //an increase or decrease to all of the player's attacks

    public float damageAbsorptionMultiplierModifier; //an increase or decrease to the player's damage absorption

    public float movementSpeedMultiplierModifier; //an increase or decrease to the player's movement speed

    public enum ItemType
    {
        passiveBuff, //items that automatically give the player some stat boost without thought or effort (doesn't need proc chance *)

        passiveProc, //items that automatically activate from contextual events like OnHit or OnJump (needs proc chance *)

        equipment, //items that require the player to press a button to activate power (needs charge to activate *)

        instant //items that immediately affect the player (does not go to any inventory or stay on player *)
    }


}
