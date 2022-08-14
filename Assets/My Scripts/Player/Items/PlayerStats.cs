using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] private PlayerInputActions playerControls;

    private InputAction useEquipmentBinding;

    private bool canUseEquipment = true;

    [SerializeField] private List<PassiveItem> passiveItems = new List<PassiveItem>(); //the list of items
    [SerializeField] private EquipmentItem equipmentItemSlot; //the player's current equipment (can only have 1 at a time)

    [SerializeField] private float runningSpeed; //running speed of the player
    [SerializeField] private float maxHealth; //max health of the player
    [SerializeField] private float damageMultiplier; //value multiplied to all player damage (higher would increase all attack damage)
    [SerializeField] private float procChanceMultiplier;

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
            //Debug.Log("Create player stats  instance");
        }

        playerControls = new PlayerInputActions();

    }

    // Start is called before the first frame update
    void Start()
    {
        //useEquipmentBinding = playerControls.Player.UseEquipment;
    }

    private void OnEnable()
    {
        useEquipmentBinding = playerControls.Player.UseEquipment;

        useEquipmentBinding.Enable();
        useEquipmentBinding.performed += UseEquipment;
    }

    private void OnDisable()
    {
        useEquipmentBinding.Disable();

        //Debug.Log("PlayerStats is disabled!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UseEquipment(InputAction.CallbackContext context)
    {
        //if the player has an equipment item...
        if(equipmentItemSlot != null)
        {
            //if the player is allowed to use the equipment item
            //and the item has a sufficient amount of uses.. call the item's action
            if (canUseEquipment)
            {
                Debug.Log("Use equipment!");
                equipmentItemSlot.ItemAction(this.gameObject);
            }
        }
        else
        {
            Debug.Log("You do not have any equipment!");
        }

        
    }

    public void AddPassiveItemToInventory(PassiveItem item)
    {
        //adding given item to the list
        passiveItems.Add(item);

        //item.CopyScriptableObjectStats();

        //if the passive is meant to occur right away, then just call it here
        //if (item.activateOnPickUp)

        //passiveItems[passiveItems.IndexOf(item)].ItemAction(this.gameObject);
        item.ItemAction(this.gameObject);
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

    public void SetCanUseEquipment(bool boolean)
    {
        canUseEquipment = boolean;
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
