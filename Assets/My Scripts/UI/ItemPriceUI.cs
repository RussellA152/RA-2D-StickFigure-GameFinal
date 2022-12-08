using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPriceUI : MonoBehaviour
{
    [SerializeField] private PaywallUnlocker paywallDisplay;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Update()
    {
        priceText.text = paywallDisplay.GetPriceAsString();
    }
}
