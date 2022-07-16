using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] private BasicDungeon room; //the room THIS door is inside of

    [SerializeField] private AdjacentRoomCheck roomChecker; //roomCheck gameobject we will use to find the adjacent rooms

    private Transform neighboringDoor; //the nearby door THIS door will teleport the player to

    public enum DoorType
    {
        bottom,
        top,
        left,
        right
    }
    private void Update()
    {
        CheckInteraction();
    }

    //the interactable action of a door is to teleport to a neighboring door
    public override void InteractableAction()
    {

    }

    //private void TeleportPlayer()
    //{

    //}
}
