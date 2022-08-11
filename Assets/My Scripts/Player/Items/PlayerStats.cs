using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public static PlayerStats instance;

    [SerializeField] private List<PassiveItem> passiveItems = new List<PassiveItem>(); //the list of items
    [SerializeField] private EquipmentItem equipmentItemSlot; //the player's current equipment (can only have 1 at a time)

    [SerializeField] private float runningSpeed;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageMultiplier;

    //[SerializeField] private CharacterController2D playerMovementScript;
    //[SerializeField] private PlayerHealth playerHealthScript;
    //[SerializeField] private AttackController playerAttackControllerScript;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            Debug.Log("Create player stats  instance");
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPassiveItemToInventory(PassiveItem item)
    {
        //adding given item to the list
        passiveItems.Add(item);

        //if the passive is meant to occur right away, then just call it here
        if (item.activateOnPickUp)
            item.ItemAction();
    }

    public void AddEquipmentItemToInventory(EquipmentItem item)
    {
        if(equipmentItemSlot != null)
        {
            equipmentItemSlot.SetRetrieved(false);

            Debug.Log("Drop previous equipment item");
            Debug.Log("Make other item retrievable again");
        }

        equipmentItemSlot = item;
    }
    public float GetRunningSpeed()
    {
        return runningSpeed;
    }
    public void ModifyRunningSpeed(float amountToIncreaseBy)
    {
        runningSpeed += amountToIncreaseBy;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void ModifyMaxHealth(float amountToIncreaseBy)
    {
        maxHealth += amountToIncreaseBy;
    }

    public float GetDamageMultiplier()
    {
        return damageMultiplier;
    }

    public void ModifyDamageMultiplier(float amountToIncreaseBy)
    {
        damageMultiplier += amountToIncreaseBy;
    }

    /*
    public CharacterController2D GetMovementScript()
    {
        return playerMovementScript;
    }

    public PlayerHealth GetHealthScript()
    {
        return playerHealthScript;
    }
    public AttackController GetAttackControllerScript()
    {
        return playerAttackControllerScript;
    }

    */

}
