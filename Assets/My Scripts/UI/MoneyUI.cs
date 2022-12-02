using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    // Update is called once per frame
    void Update()
    {
        SetMoney(PlayerStats.instance.GetPlayerMoney());
    }

    private void SetMoney(float money)
    {
        moneyText.text = money.ToString();
    }
}
