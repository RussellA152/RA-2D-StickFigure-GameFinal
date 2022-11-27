using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulExchange : Item
{
    // Survive a fatal attack and regain some health
    // this item only works once, and once it activates it will not revive player again
    public override void ItemAction(GameObject player)
    {
        PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath += ResurrectPlayer;

    }

    public override void InitializeValues()
    {

        itemName = myScriptableObject.itemName;

        type = myScriptableObject.itemType;

        //PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath += ItemAction;

    }

    public void ResurrectPlayer()
    {
        // Survive a fatal attack and regain some health
        if (ShouldActivate())
        {
            PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);
        }

        PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath -= ResurrectPlayer;

        //ReturnToPool();
    }


}
