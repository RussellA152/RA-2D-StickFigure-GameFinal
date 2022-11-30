using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats instance;


    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private PlayerInputActions playerControls;

    [SerializeField] private PlayerHealth playerHealthScript;
    [SerializeField] private PlayerComponents playerComponentScript;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private BoxCollider2D hurtbox;

    //[SerializeField] private PlayerHitCollider playerHitColliderScript;

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

    [SerializeField] private int money; // the amount of money the player has 

    private float defaultGravity; // the amount of gravity applied to the player from the inspector

    [Header("All Modifiable Stats")]
    [SerializeField] private bool canBeHurt = true;
    [SerializeField] private float gravityApplied; // the amount of gravity applied to the player
    [SerializeField] private float runningSpeed; // the running speed of the player
    [SerializeField] private float maxHealth; // the max health of the player
    [SerializeField] private float damageMultiplier; // a value multiplied to all player damage (higher would increase all attack damage)
    [SerializeField] private float attackPowerMultiplier; // a value multiplied to all player attack power damage (higher would increase amount of force applied to enemies)
    [SerializeField] private float damageAbsorptionMultiplier; // a percentage that will be removed from each damage that the player takes (ex. a value of 0.1 would be 10% less damage from all sources)
    [SerializeField] private float procChanceMultiplier; // a value multiplied to all proc chances of Items 
    [SerializeField] private float playerGravity; // the amount of gravity applied to the player
   

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


        // sets "defaultGravity" to whatever was set in the inspector
        defaultGravity = rb.gravityScale;
        playerGravity = defaultGravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        //useEquipmentBinding = playerControls.Player.UseEquipment;
        spriteRenderer.color = new Color(255, 255, 255, 121);
        
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
    //public void TestScriptableObject(AttackValues so)
    //{
    //    playerHitColliderScript.SetAttackValues(so);
    //}

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

    // set the player's gravity scale back to whatever they had in Awake()
    public void ResetGravity()
    {
        rb.gravityScale = playerGravity;
    }

    // set the player's gravity scale back to "amount"
    public void SetPlayerGravity(float amount)
    {
        rb.gravityScale = amount;
    }

    // set the player's gravity scale back to "amount"
    public void ModifyPlayerGravity(float amountToIncreaseBy)
    {
        playerGravity += amountToIncreaseBy;
    }

    public int GetPlayerMoney()
    {
        return money;
    }

    public void ModifyPlayerMoney(int amount)
    {
        Debug.Log("MONEY MODIFIED!");
        money += amount;
    }


    public float GetRunningSpeed()
    {
        return runningSpeed;
    }
    public void ModifyRunningSpeed(float amountToIncreaseBy)
    {
        // increase running by a percentage
        runningSpeed = runningSpeed + (runningSpeed * amountToIncreaseBy);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public bool IsHealthFull()
    {
        return playerHealthScript.IsCurrentHealthFull();
    }

    public void ModifyPlayerCurrentHealth(float amountToIncreaseBy)
    {
        playerHealthScript.ModifyHealth(amountToIncreaseBy);
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

    public float GetAttackPowerMultiplier()
    {
        return attackPowerMultiplier;
    }
    public void ModifyAttackPowerMultiplier(float amountToIncreaseBy)
    {
        attackPowerMultiplier += amountToIncreaseBy;
    }
    public float GetDamageAbsorptionMultiplier()
    {
        return damageAbsorptionMultiplier;
    }
    public void ModifyDamageAbsorptionMultiplier(float amountToIncreaseBy)
    {
        damageAbsorptionMultiplier += amountToIncreaseBy;
    }

    public float GetProcChanceMultiplier()
    {
        return procChanceMultiplier;
    }
    public void ModifyProcChanceMultiplier(float amountToIncreaseBy)
    {
        procChanceMultiplier += amountToIncreaseBy;      
    }

    public void SetCanUseEquipment(bool boolean)
    {
        CanUseEquipment = boolean;
    }
    public bool ReturnCanBeHurt()
    {
        return canBeHurt;
    }
    public void TurnOffHurt(float timer)
    {
        StartCoroutine(TurnOffHurtTemporarily(timer));
    }
    IEnumerator TurnOffHurtTemporarily(float timer)
    {

        canBeHurt = false;
        yield return new WaitForSeconds(timer);

        canBeHurt = true;
    }

    //public void SetPlayerTransparency(Color32 colorToSet)
    //{
    //    spriteRenderer.material.color = colorToSet;
    //}

    // true if facing right
    // false if facing left
    public bool ReturnPlayerDirection()
    {
        return playerComponentScript.GetPlayerDirection();
    }

    public GameObject GetPlayer()
    {
        return this.transform.gameObject;
    }

}
