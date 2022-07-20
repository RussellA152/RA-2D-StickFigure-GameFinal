using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private BasicDungeon room; //the room THIS door is inside of 

    private Door neighboringDoor; //the nearby door THIS door will teleport the player to

    private Vector3 teleportPositionOffset = new Vector3(0f,2.5f); //an offset added to the position the player will be teleported to when using a door

    private float lerpTimer = 0;
    private float timeDivider = 5f;


    private void Update()
    {
        lerpTimer += Time.deltaTime;

        //only let the player leave the room if they have cleared the enemies inside*
        if (room.roomEnemyCountState == BasicDungeon.RoomEnemyCount.cleared)
            CheckInteraction();
    }

    //the interactable action of a door is to teleport to a neighboring door
    public override void InteractableAction()
    {
        //lerp the player's position to the neighboring door (teleport them)
        interacter.position = Vector2.Lerp(interacter.position, neighboringDoor.transform.position + teleportPositionOffset, lerpTimer / timeDivider);

        //don't allow player to use door again for a bit
        StartCooldown();

        //also need to set the neighboring door on cooldown so that the player doesn't immediately teleport back to the original door 
        neighboringDoor.StartCooldown();

        //when player teleports to a new room, the current room of the LevelManager is updated
        LevelManager.instance.UpdateCurrentRoom(neighboringDoor.GetDoorRoom());
    }

    public void SetNeighboringDoor(Door door)
    {
        neighboringDoor = door;

    }

    public BasicDungeon GetDoorRoom()
    {
        return room;
    }
}
