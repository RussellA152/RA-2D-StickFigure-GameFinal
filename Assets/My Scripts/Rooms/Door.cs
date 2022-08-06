using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private BasicRoom room; //the room THIS door is inside of 

    private Door neighboringDoor; //the nearby door THIS door will teleport the player to

    private Vector3 teleportPositionOffset = new Vector3(0f,2.5f); //an offset added to the position the player will be teleported to when using a door


    private void Update()
    {
        //only let the player leave the room if they have cleared the enemies inside*
        if (room.roomEnemyCountState == BasicRoom.RoomEnemyCount.cleared)
            CheckInteraction();
    }

    //the interactable action of a door is to teleport to a neighboring door
    public override void InteractableAction()
    {
        interacter.position = new Vector3(neighboringDoor.transform.position.x, neighboringDoor.transform.position.y, neighboringDoor.transform.position.z) + teleportPositionOffset;
        //Debug.Log("Teleporting to " + neighboringDoor.transform.position.x);

        //don't allow player to use door again for a bit
        StartCooldown();

        //also need to set the neighboring door on cooldown so that the player doesn't immediately teleport back to the original door 
        neighboringDoor.StartCooldown();

        //when player teleports to a new room, the current room of the LevelManager is updated
        LevelManager.instance.UpdateCurrentRoom(neighboringDoor.GetDoorRoom());

        //when player uses the door, we will invoke the "EnteringNewAreaEvent" event system
        LevelManager.instance.EnteringNewAreaEvent();
    }

    public void SetNeighboringDoor(Door door)
    {
        neighboringDoor = door;

    }

    public BasicRoom GetDoorRoom()
    {
        return room;
    }

}
