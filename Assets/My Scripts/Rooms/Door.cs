using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private BaseRoom room; //the room THIS door is inside of 

    [SerializeField] private Door neighboringDoor; //the nearby door THIS door will teleport the player to

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 teleportPositionOffset = new Vector3(0f,2.5f); //an offset added to the position the player will be teleported to when using a door

    private void Update()
    {
        //if player is in door's trigger, and the room that this door belongs to is cleared...
        //check for input from the player
        if (inTrigger)
        {
            if (room.roomEnemyCountState == BaseRoom.RoomEnemyCount.cleared)
            {
                CheckInteraction();
                //Debug.Log("Check interaction! door");

            }
        }
        
        
    }


    //the interactable action of a door is to teleport to a neighboring door
    public override void InteractableAction()
    {
        //Fetch the interacter of this door from base class
        Transform interacter = base.GetInteracter();

        //set the interacter's position to this door's neighboring door's position
        interacter.position = new Vector3(neighboringDoor.transform.position.x, neighboringDoor.transform.position.y, neighboringDoor.transform.position.z) + teleportPositionOffset;

        //don't allow player to use door again for a bit
        base.StartCooldown();

        //also need to set the neighboring door on cooldown so that the player doesn't immediately teleport back to the original door 
        neighboringDoor.StartCooldown();

        //when player teleports to a new room, the current room of the LevelManager is updated
        //this should be called before invoking the "EnteringNewAreaEvent" so that the event system can safely have an updated CurrentRoom
        LevelManager.instance.UpdateCurrentRoom(neighboringDoor.GetDoorRoom());

        //when player uses the door, we will invoke the "EnteringNewAreaEvent" event system
        LevelManager.instance.EnteringNewAreaEvent();
    }

    // call this function when you want to change the appearance of the door 
    // ex. a door leading to a boss room will have a red boss door
    public void ChangeDoorSprite(Sprite doorSprite)
    {
        spriteRenderer.sprite = doorSprite;
    }

    public void SetNeighboringDoor(Door door)
    {
        neighboringDoor = door;

    }

    public BaseRoom GetDoorRoom()
    {
        return room;
    }

}
