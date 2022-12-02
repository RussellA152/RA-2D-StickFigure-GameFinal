using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient barGradient;
    public Gradient textGradient;

    public Image fill;


    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI healthDividerText;
    [SerializeField] private TextMeshProUGUI maxHealthText;
    


    private void Update()
    {
        // setting the color of the health bar based on slider value (player health determines slider value)
        fill.color = barGradient.Evaluate(slider.normalizedValue);
        // setting the color of the health text based on slider value (player health determines slider value)
        //currentHealthText.color = textGradient.Evaluate(slider.normalizedValue);
        //maxHealthText.color = textGradient.Evaluate(slider.normalizedValue);
        //healthDividerText.color = textGradient.Evaluate(slider.normalizedValue);

        SetMaxHealth(PlayerStats.instance.GetMaxHealth());
        SetHealth(PlayerStats.instance.GetCurrentHealth());
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;

        maxHealthText.text = health.ToString();

        
    }

    public void SetHealth(float health)
    {
        slider.value = health;

        currentHealthText.text = health.ToString();


    }
}
