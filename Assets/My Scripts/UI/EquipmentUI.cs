using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Image equipmentItemImage;
    [SerializeField] private TextMeshProUGUI currentChargeText; // text display of the active equipment item's charge left
    [SerializeField] private TextMeshProUGUI maxChargeText; // text display of the active equipment item's max amount of charge

    private Item equipmentItem;

    private void Start()
    {

        // if there was an equipment item already at the start of the game, update the UI's image
        if (ItemManager.instance.activeEquipmentSlot != null)
            UpdateEquipmentImage();
        else
            equipmentItemImage.enabled = false;

        // when the player picks up a new item , update the UI's image (equipment item only)
        ItemManager.instance.itemPickupEvent += UpdateEquipmentImage;
    }

    // Update is called once per frame
    void Update()
    {
        if(ItemManager.instance.GetActiveEquipmentItem() != null)
            UpdateEquipmentChargeText(ItemManager.instance.GetActiveEquipmentItem().amountOfCharge, ItemManager.instance.GetActiveEquipmentItem().maxAmountOfCharge);
        else
        {
            equipmentItemImage.enabled = false;
            UpdateEquipmentChargeText(0, 0);
        }
            
    }

    public void UpdateEquipmentImage()
    {
        // only update the equipment item image if we are actually getting a new equipment item
        // ex. if we picked up an item, and we still have Bomb, then don't update
        // but if we had a Bomb, and then picked up a Health Potion, then update the image
        if(ItemManager.instance.GetActiveEquipmentItem() != this.equipmentItem)
        {
            equipmentItemImage.enabled = true;

            equipmentItem = ItemManager.instance.GetActiveEquipmentItem();

            // set the UI image to the equipment item's sprite
            equipmentItemImage.sprite = ItemManager.instance.GetActiveEquipmentItem().GetComponent<ItemGiver>().spriteRenderer.sprite;
            Debug.Log("UPDATE IMAGE!");
        }


        
    }

    private void UpdateEquipmentChargeText(int currentAmount, int maxAmount)
    {
        currentChargeText.text = currentAmount.ToString();
        maxChargeText.text = maxAmount.ToString();
    }
}
