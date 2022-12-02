using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;


    private void Update()
    {
        fill.color = gradient.Evaluate(slider.normalizedValue);

        SetMaxHealth(PlayerStats.instance.GetMaxHealth());
        SetHealth(PlayerStats.instance.GetCurrentHealth());
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;

        
    }

    public void SetHealth(float health)
    {
        slider.value = health;

        
    }
}
