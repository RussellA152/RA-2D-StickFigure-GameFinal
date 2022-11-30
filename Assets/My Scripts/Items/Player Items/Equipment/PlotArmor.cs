using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotArmor : Item
{
    //[Header("Color During Plot Armor Usage")]
    //[SerializeField] private Color32 transparentColor;

    //[Header("Color After Plot Armor Ends")]
    //[SerializeField] private Color32 resetColor;
    public override void ItemAction(GameObject player)
    {
        // Turn off player's ability to be hurt for a few seconds
        if (ShouldActivate())
        {
            // Don't let player get hurt for a few seconds
            PlayerStats.instance.TurnOffHurt(myScriptableObject.itemDuration);

            //Change the player's transparency when they use this item
            //StartCoroutine(ChangeColor(myScriptableObject.itemDuration));

            // don't let player swap equipment for the duration of the plot armor effect and a little extra time just in case
            StartCoroutine(CanSwapTimer(myScriptableObject.itemDuration + 0.5f));
            
            //Debug.Log("Hello? Plot armor?");
        }

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        usageCooldown = myScriptableObject.usageCooldown;

        chargeConsumedPerUse = myScriptableObject.chargesConsumedPerUse;

        maxAmountOfCharge = myScriptableObject.maxAmountOfCharge;
        amountOfCharge = myScriptableObject.maxAmountOfCharge;

    }

    //IEnumerator ChangeColor(float timer)
    //{
    //    // set player to a new color
    //    PlayerStats.instance.SetPlayerTransparency(transparentColor);
    //    yield return new WaitForSeconds(timer);
    //    // reset player color 
    //    PlayerStats.instance.SetPlayerTransparency(resetColor);
    //}

    IEnumerator CanSwapTimer(float timer)
    {
        ItemSwapper.instance.SetCanSwapEquipment(false);
        yield return new WaitForSeconds(timer);
        ItemSwapper.instance.SetCanSwapEquipment(true);
    }
}
