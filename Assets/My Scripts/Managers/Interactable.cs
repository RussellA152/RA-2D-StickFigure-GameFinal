using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//This class will be inherited by other scripts that deal with input interactions
//ex. player needs to press "e" to open a chest, open a door, or pick up an item
public class Interactable : MonoBehaviour
{
    private PlayerComponents playerComponentScript;

    private InputAction playerInputButton;

    private bool inTrigger; //is the player in this interactable object's trigger collider?

    public void CheckInteraction()
    {
        //when player is pressing interaction button...
        if(playerInputButton != null)
        {
            if (playerInputButton.ReadValue<float>() > 0 && inTrigger)
                InteractableAction();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player has entered this interactable's collision trigger
        //grab a reference to the PlayerComponent script
        if (collision.gameObject.CompareTag("Player"))
        {
            playerComponentScript = collision.gameObject.GetComponent<PlayerComponents>();

            playerInputButton = playerComponentScript.GetInteractionButton();

            inTrigger = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    public virtual void InteractableAction()
    {

    }
}
