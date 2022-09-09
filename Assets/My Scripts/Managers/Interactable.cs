using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

//This class will be inherited by other scripts that deal with input interactions
//ex. player needs to press "e" to open a chest, open a door, or pick up an item
public abstract class Interactable : MonoBehaviour
{
    
    private Transform interacter; //the entity that interacted with this object (usually Player)

    //private PlayerComponents playerComponentScript;

    private InputAction playerInputButton;

    [SerializeField] private bool needsButtonPress; //does the player need to press the interaction button to interact with this?

    [HideInInspector]
    public bool inTrigger; //is the player in this interactable object's trigger collider?

    private bool canInteractWith = true; //is the player allowed to "use" this object?

    [SerializeField] private float cooldownTimer; //how long the player needs to wait until they can "use" this object again

    private bool cooldownStarted = false; //has the cooldown coroutine for this object started yet?

    //public bool needsCooldownAfterInteraction; //does this interactable object need a cooldown before being able to "use" it again?

    private void Awake()
    {
        //create a new instance of the player's "interact" binding 
        //this is so that all interactable gameobjects don't have to GetComponent the "interact" binding from the PlayerComponent script
        playerInputButton = new PlayerInputActions().Player.Interact;
    }

    protected void OnEnable()
    {
        playerInputButton.Enable();
    }

    protected void OnDisable()
    {
        playerInputButton.Disable();
    }


    public void CheckInteraction()
    {

        
        //if the interaction button isn't null
        // and if the player needs to press interaction button
        if (needsButtonPress)
        {
           
            //check if they are pressing that button
            //and if they are in trigger and can interact
            if (playerInputButton != null)
            {
                //Debug.Log("Invoking check interaction! after input button isnt null " + gameObject.name);

                if (playerInputButton.triggered && canInteractWith)
                {
                    InteractableAction();
                    //Debug.Log("Call Interactable action " + gameObject.name);
                    //Debug.Log("try to invoke interaction (yes button)");
                }
            }
            
    
        }
        //if this object doesn't need to check interaction button
        //then just check if they're in the trigger and can interact
        else if (!needsButtonPress)
        {
            if (canInteractWith)
            {
                InteractableAction();
                //Debug.Log("try to invoke interaction (no button)");
            }
                
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if the player has entered this interactable's collision trigger
        //grab a reference to the PlayerComponent script
        if (collision.gameObject.CompareTag("Player"))
        {
            //interacter is always the player (because player has "Player" tag)
            interacter = collision.transform;
            //Debug.Log("Collision detected!");


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

    //what the interactable object will do when the player presses the interaction button on it
    public abstract void InteractableAction();

    public void StartCooldown()
    {
        if (!cooldownStarted)
            StartCoroutine("InteractingCooldown");
    }

    //don't let player interact with this object until the cooldown is finished
    private IEnumerator InteractingCooldown()
    {
        //Debug.Log("Cooldown begun");
        cooldownStarted = true;

        canInteractWith = false;

        yield return new WaitForSeconds(cooldownTimer);

        canInteractWith = true;

        cooldownStarted = false;
    }

    protected Transform GetInteracter()
    {
        return interacter;
    }


}
