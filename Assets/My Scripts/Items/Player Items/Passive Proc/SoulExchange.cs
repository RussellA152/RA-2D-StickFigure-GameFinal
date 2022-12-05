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

        procChance = myScriptableObject.procChance;

        itemActionSound = myScriptableObject.itemActionSound;

        //PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath += ItemAction;

    }

    public void ResurrectPlayer()
    {
        // Survive a fatal attack and regain some health
        if (ShouldActivate())
        {
            Debug.Log("SOUL EXCHANGE!");
            //PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath -= ResurrectPlayer;
            PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath -= ResurrectPlayer;

            Invoke(nameof(RefillHealth), myScriptableObject.itemDuration);
            
            //PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);

        }

        //PlayerStats.instance.gameObject.GetComponent<PlayerHealth>().onPlayerDeath -= ResurrectPlayer;

        //ReturnToPool();
    }

    private void RefillHealth()
    {
        PlayerStats.instance.ModifyPlayerCurrentHealth(myScriptableObject.currentHealthModifier);

        PlayItemSound(itemActionSound);
        PlayerStats.instance.GetPlayer().GetComponent<Animator>().Play("Resurrect");
        
    }


}
