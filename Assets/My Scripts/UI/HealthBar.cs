using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [Header("'True' means this UI is for the Player")]
    [SerializeField] private bool isPlayer;

    [Header("Can this slider adjust its direction?")]
    [SerializeField] private bool healthBarSliderIsStatic; // should the healthbar's slider be able to rotate (needed for enemies that have health bar in World Space)
    [Header("Only needed for Enemies")]

    [SerializeField] private EnemyMovement enemyMovementScript; // needed for enemies (if the enemy is facing a direction, then we need the health bar slider to move in the opposite direction

    [Header("UI Components Needed")]
    public Slider slider;

    public Gradient barGradient;
    public Gradient textGradient;

    public Image fill;


    [SerializeField] private TextMeshProUGUI currentHealthText;
    [SerializeField] private TextMeshProUGUI healthDividerText;
    [SerializeField] private TextMeshProUGUI maxHealthText;

    [SerializeField] private IHealth healthScript;


    private void Start()
    {
        // check if this healthbar is for the player
        // if not, then we don't need to do GameObject.Find because enemies already have a canvas attached to them (unlike the Player's)
        if (isPlayer)
            healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealth>();
        else
        {
            healthScript = GetComponentInParent<IHealth>();
        }
            
    }



    private void Update()
    {
        // if this is for an enemy and the health bar slider is allowed to rotate...
        if(enemyMovementScript != null && !healthBarSliderIsStatic)
        {
            // if enemy is facing right, then the health bar slider should have a "Left To Right" direction
            if (enemyMovementScript.GetDirection())
                slider.direction = Slider.Direction.LeftToRight;

            // otherwise, if the enemy is facing left, then the health bar slider should have a "Right To Left" direction
            else
                slider.direction = Slider.Direction.RightToLeft;
        }

        // setting the color of the health bar based on slider value (player health determines slider value)
        fill.color = barGradient.Evaluate(slider.normalizedValue);
        // setting the color of the health text based on slider value (player health determines slider value)
        //currentHealthText.color = textGradient.Evaluate(slider.normalizedValue);
        //maxHealthText.color = textGradient.Evaluate(slider.normalizedValue);
        //healthDividerText.color = textGradient.Evaluate(slider.normalizedValue);

        SetHealth(healthScript.GetHealth());
        SetMaxHealth(healthScript.GetMaxHealth());

        //SetMaxHealth(PlayerStats.instance.GetMaxHealth());
        //SetHealth(PlayerStats.instance.GetCurrentHealth());
    }

    public void SetMaxHealth(float health)
    {
        // set the slider max value equal to the entity's max potential health
        slider.maxValue = health;

        // if this health bar doesn't have text, don't try to display it
        if(maxHealthText != null)
            maxHealthText.text = health.ToString();

        
    }

    public void SetHealth(float health)
    {
        // set the slider value equal to the entity's current health
        slider.value = health;

        // if this health bar doesn't have text, don't try to display it
        if (maxHealthText != null)
            currentHealthText.text = health.ToString();


    }
}
