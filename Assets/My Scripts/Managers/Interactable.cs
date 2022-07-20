using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//This class will be inherited by other scripts that deal with input interactions
//ex. player needs to press "e" to open a chest, open a door, or pick up an item
public class Interactable : MonoBehaviour
{
    [HideInInspector]
    public Transform interacter; //the entity that interacted with this object (usually Player)

    private PlayerComponents playerComponentScript;

    private InputAction playerInputButton;

    private bool inTrigger; //is the player in this interactable object's trigger collider?

    private bool canInteractWith = true; //is the player allowed to "use" this object?

    public float cooldownTimer; //how long the player needs to wait until they can "use" this object again

    private bool cooldownStarted = false; //has the cooldown coroutine for this object started yet?

    //public bool needsCooldown; //does this interactable object need a cooldown before being able to "use" it again?

    public void CheckInteraction()
    {
        //when player is pressing interaction button...
        if(playerInputButton != null)
        {
            if (playerInputButton.ReadValue<float>() > 0 && inTrigger && canInteractWith)
                InteractableAction();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player has entered this interactable's collision trigger
        //grab a reference to the PlayerComponent script
        if (collision.gameObject.CompareTag("Player"))
        {
            interacter = collision.transform;

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

    public void StartCooldown()
    {
        if (!cooldownStarted)
            StartCoroutine("InteractingCooldown");
    }

    //don't let player interact with this object until the cooldown is finished
    private IEnumerator InteractingCooldown()
    {
        cooldownStarted = true;

        canInteractWith = false;

        yield return new WaitForSeconds(cooldownTimer);

        canInteractWith = true;

        cooldownStarted = false;
    }


}
