using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControlUI : MonoBehaviour
{

    private PlayerInput playerInput;

    [SerializeField] private Image jumpPrompt;
    [SerializeField] private Image lightAttackPrompt;
    [SerializeField] private Image heavyAttackPrompt;
    [SerializeField] private Image slidePrompt;
    [SerializeField] private Image rollPrompt;
    [SerializeField] private Image interactPrompt;
    [SerializeField] private Image useEquipmentPrompt;

    //[SerializeField] private Image extraSlidePrompt; // the "T" in "SHIFT" is cut off, so we need an extra image for it (if gamepad is detected, disable it)
    [SerializeField] private Image extraRollPrompt; // the "L" in "CTRL" is cut off, so we need an extra image for it (if gamepad is detected, disable it)

    [Header("Keyboard Prompt Sprites")]
    [SerializeField] private Sprite jumpKBPrompt;
    [SerializeField] private Sprite lightAttKBPrompt;
    [SerializeField] private Sprite heavyAttKBPrompt;
    [SerializeField] private Sprite slideKBPrompt;
    [SerializeField] private Sprite rollKBPrompt;
    [SerializeField] private Sprite interactKBPrompt;
    [SerializeField] private Sprite useEquipmentKBPrompt;


    [Header("Xbox Prompt Sprites")]
    [SerializeField] private Sprite jumpXBPrompt;
    [SerializeField] private Sprite lightAttXBPrompt;
    [SerializeField] private Sprite heavyAttXBPrompt;
    [SerializeField] private Sprite slideXBPrompt;
    [SerializeField] private Sprite rollXBPrompt;
    [SerializeField] private Sprite interactXBPrompt;
    [SerializeField] private Sprite useEquipmentXBPrompt;

    [Header("Playstation Prompt Sprites")]
    [SerializeField] private Sprite jumpPSPrompt;
    [SerializeField] private Sprite lightAttPSPrompt;
    [SerializeField] private Sprite heavyAttPSPrompt;
    [SerializeField] private Sprite slidePSPrompt;
    [SerializeField] private Sprite rollPSPrompt;
    [SerializeField] private Sprite interactPSPrompt;
    [SerializeField] private Sprite useEquipmentPSPrompt;

    private void OnEnable()
    {
        // fetch player input component from the Player Gameobject
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        playerInput.onControlsChanged += PlayerControlsChanged;
    }

    private void OnDisable()
    {
        playerInput.onControlsChanged -= PlayerControlsChanged;
    }

    public void PlayerControlsChanged(PlayerInput obj)
    {
        Debug.Log("Change controls!");

        // Check the type of controller the user is using
        switch (obj.currentControlScheme)
        {
            // if player is using a keyboard and mouse
            case "Keyboard&Mouse":

                // these prompts are needed for keyboard use because SHIFT and CTRL prompts are cut off without these being enabled
                extraRollPrompt.enabled = true;
                //extraSlidePrompt.enabled = true;

                UpdateControlPrompts(jumpKBPrompt, lightAttKBPrompt, heavyAttKBPrompt, slideKBPrompt, rollKBPrompt, interactKBPrompt,useEquipmentKBPrompt);
                //Debug.Log("KEYBOARD USED!");
                break;

            // if player is using a gamepad, check which type of controller
            // Bug: If both a playstation and xbox controller are plugged in, the playstation controller will take priority (might be Unity related)
            case "Gamepad":

                // don't need these prompts if gamepad is in use because this is for keyboard
                extraRollPrompt.enabled = false;
                //extraSlidePrompt.enabled = false;

                // if player is using an xbox controller
                if (Gamepad.current.device.displayName == "Xbox Controller")
                {
                    UpdateControlPrompts(jumpXBPrompt, lightAttXBPrompt, heavyAttXBPrompt, slideXBPrompt, rollXBPrompt, interactXBPrompt,useEquipmentXBPrompt);

                }
                // if player is using a dualshock controller 
                else if(Gamepad.current.device.displayName == "Wireless Controller")
                {
                    UpdateControlPrompts(jumpPSPrompt, lightAttPSPrompt, heavyAttPSPrompt, slidePSPrompt, rollPSPrompt, interactPSPrompt,useEquipmentPSPrompt);
                }
                break;

            default:
                break;
        }
    }

    // update the UI's button prompts based on the current controller the User is using
    private void UpdateControlPrompts(Sprite jumpSprite, Sprite lightAttackSprite, Sprite heavyAttackSprite, Sprite slideSprite, Sprite rollSprite, Sprite interactSprite, Sprite useEquipmentSprite)
    {
        jumpPrompt.sprite = jumpSprite;
        lightAttackPrompt.sprite = lightAttackSprite;
        heavyAttackPrompt.sprite = heavyAttackSprite;
        slidePrompt.sprite = slideSprite;
        rollPrompt.sprite = rollSprite;
        interactPrompt.sprite = interactSprite;
        useEquipmentPrompt.sprite = useEquipmentSprite;
    }

}
