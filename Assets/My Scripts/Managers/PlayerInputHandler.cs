using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovementInput playerMovement;
    private CharacterController2D characterController;


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        playerMovement = FindObjectOfType<PlayerMovementInput>();

        characterController = playerMovement.GetComponent<CharacterController2D>();

    }
    public void OnPlayerMove(CallbackContext context)
    {
        if (playerMovement != null)
        {
            playerMovement.PlayerMove(context.ReadValue<Vector2>().x);
        }
    }
    public void OnJump(CallbackContext context)
    {
        if (playerMovement != null)
        {
            playerMovement.Jump();
        }

    }

    public void OnRoll(CallbackContext context)
    {
        if (playerMovement != null)
        {
            playerMovement.Roll(context.performed);
        }

    }
    public void OnSlide(CallbackContext context)
    {
        if (playerMovement != null)
        {
            playerMovement.Slide(context);
        }
    }
}
