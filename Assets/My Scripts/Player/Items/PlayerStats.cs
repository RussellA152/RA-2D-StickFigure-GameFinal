using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;

    [SerializeField] private PlayerInputActions playerControls;

    //public InputAction useEquipmentBinding { public get; private set; }

    //[SerializeField] private GameObject componentHolder; //gameobject that holds all added components from item
                                                         //mainly used so that the Player gameobject is not clogged with too many components

    public bool CanUseEquipment { get; private set; }


    //[Header("New Items")]
    //[SerializeField] private List<Item> newPassiveItems = new List<Item>();


    //[SerializeField] private Item newEquipmentItemSlot; //the player's current equipment (can only have 1 at a time)
    //[SerializeField] private EquipmentItem newEquipmentItemOriginal; //the gameobject of the current equipment item (can only have 1 at a time)








    //[Header("Old Items")]
    //[SerializeField] private List<PassiveItem> passiveItems = new List<PassiveItem>(); //the list of passive items

    //[SerializeField] private EquipmentItem equipmentItemSlot; //the player's current equipment (can only have 1 at a time)
    //[SerializeField] private EquipmentItem equipmentItemOriginal; //the gameobject of the current equipment item (can only have 1 at a time)

    [Header("All Modifiable Stats")]
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

        //playerControls = new PlayerInputActions();

    }

    // Start is called before the first frame update
    void Start()
    {
        //useEquipmentBinding = playerControls.Player.UseEquipment;
    }

    private void OnEnable()
    {
        //useEquipmentBinding = playerControls.Player.UseEquipment;

        //useEquipmentBinding.Enable();
        //useEquipmentBinding.performed += UsePlayerEquipment;
    }

    private void OnDisable()
    {
        //useEquipmentBinding.Disable();

        //Debug.Log("PlayerStats is disabled!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UsePlayerEquipment(InputAction.CallbackContext context)
    {
        //if the player has an equipment item...
        //if (newEquipmentItemSlot != null)
        //{
            //if the player is allowed to use the equipment item
            //and the item has a sufficient amount of uses.. call the item's action
            //if (canUseEquipment)
            //{
                //Debug.Log("Try to use equipment!");
                //newEquipmentItemSlot.ItemAction(this.gameObject);
            //}
        //}
        //else
        //{
            //Debug.Log("You do not have any equipment!");
        //}


    }

    public void AddPassiveItem(Item passiveItem)
    {
        //adding given item to the list
        //newPassiveItems.Add(passiveItem);

        //all items will immediately activate their ability on pick up
        //stat boost passive items will immediately modify a value
        //while "proc" items will subscribe to their corresponding event system
        //passiveItem.ItemAction(this.gameObject);
    }

    public void AddEquipmentItem(Item equipmentItem)
    {

        //if equipmentItemSlot is not null when you pick an equipment item
        //then the player has to swap out their current equipment for the new equipment
        //if (newEquipmentItemSlot != null)
        //{
            //equipmentItemSlot.SetRetrieved(false);
            //newEquipmentItemSlot.itemGiver.OnDrop();

            //Debug.Log("Drop previous equipment item");
            //Debug.Log("Make other item retrievable again");
        //}

        //newEquipmentItemSlot = equipmentItem;
        //equipmentItemOriginal = equipmentItem.originalDroppedInstance;
    }





    /*
    private void UseEquipment(InputAction.CallbackContext context)
    {
        //if the player has an equipment item...
        if(equipmentItemSlot != null)
        {
            //if the player is allowed to use the equipment item
            //and the item has a sufficient amount of uses.. call the item's action
            if (canUseEquipment)
            {
                Debug.Log("Try to use equipment!");
                equipmentItemSlot.ItemAction(this.gameObject);
            }
        }
        else
        {
            Debug.Log("You do not have any equipment!");
        }

        
    }
    */
    /*
    public void AddPassiveItemToInventory(PassiveItem item)
    {
        //adding given item to the list
        passiveItems.Add(item);

        //all items will immediately activate their ability on pick up
        //stat boost passive items will immediately modify a value
        //while "proc" items will subscribe to their corresponding event system
        item.ItemAction(this.gameObject);
    }

    public void AddEquipmentItemToInventory(EquipmentItem item)
    {

        //if equipmentItemSlot is not null when you pick an equipment item
        //then the player has to swap out their current equipment for the new equipment
        if(equipmentItemSlot != null)
        {
            //equipmentItemSlot.SetRetrieved(false);
            equipmentItemSlot.EquipmentSwap();
            
            //Debug.Log("Drop previous equipment item");
            //Debug.Log("Make other item retrievable again");
        }

        equipmentItemSlot = item;
        equipmentItemOriginal = item.originalDroppedInstance;
    }

    //return true if the player has an equipment item in their slot
    //otherwise return false
    //public bool HasAnEquipmentItem()
    //{
        //if (equipmentItemSlot != null)
            //return true;
        //else
            //return false;
    //}
    */
    
    //return the component holder
    //mainly needed when PassiveItem or EquipmentItem need to add a script component
    //public GameObject GetComponentHolder()
    //{
        //return componentHolder;
    //}


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
        CanUseEquipment = boolean;
    }

    public GameObject GetPlayer()
    {
        return this.transform.gameObject;
    }

}