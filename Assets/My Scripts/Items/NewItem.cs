using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class NewItem : MonoBehaviour
{
    public string itemName;

    public ItemType type;

    public float procChance;

    public int amountOfCharge; //how many times can this item be used?
    public int chargeConsumedPerUse; //how much charge will this item consume on each use?

    [SerializeField] private TextAsset textAssetData;

    [HideInInspector]
    public bool hasSufficientCharge = true;

    private void Start()
    {
        OnItemPickup();
    }


    public enum ItemType
    {
        passive, //items that automatically activate and affect the player without thought or effort

        equipment, //items that require the player to press a button to activate power

        instant //items that immediately affect the player (does not go to any inventory or stay on player)
    }

    //what the item does for the player
    //passes in player to access components if needed..
    public abstract void ItemAction(GameObject player);

    //take values from persistant data source containing stats for each item
    public abstract void InitializeValues();


    public bool ShouldActivate()
    {
        switch (type)
        {
            //not needed for passive items that do not have to proc during gameplay (probably won't even invoke this function)
            case ItemType.passive:
                //check if the item's proc chance was successful
                //if not, return and do not allow ability to activate
                if (Random.value < procChance)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case ItemType.equipment:
                //if this item has sufficient charge, take some away
                //otherwise, return false and don't allow item to activate its use
                if (amountOfCharge < chargeConsumedPerUse)
                {
                    //when player's equipment item is out of charges we can probably play some sound that
                    //indicates the item can't be used
                    Debug.Log("Insufficient Charge");

                    hasSufficientCharge = false;
                    return false;
                }

                else
                {
                    amountOfCharge -= chargeConsumedPerUse;
                    hasSufficientCharge = true;

                    return true;
                }

            //this function is not needed for instant items since they will always activate on pickup (probably won't even invoke this function)
            case ItemType.instant:
                //instant items should always activate
                return true;
        }

        return true;

        
    }

    public void SwapEquipment()
    {

    }

    public void OnItemPickup()
    {
        //copy values from persistant data source when picked up
        InitializeValues();

        switch (type)
        {
            default:

            case ItemType.passive:
                //adds the item of type "Passive" to the player's passive item inventory
                PlayerStats.instance.AddPassiveItem(this);
                break;
            case ItemType.equipment:
                //adds the item of type "Equipment" to the player's equipment item inventory
                PlayerStats.instance.AddEquipmentItem(this);
                break;
            case ItemType.instant:
                //instant items do not get added to any inventory
                break;
        }

    }
}
