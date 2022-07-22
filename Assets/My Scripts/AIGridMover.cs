using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class AIGridMover : MonoBehaviour
{
    [SerializeField] private AstarPath aStarGrid;

    [SerializeField] private Vector2 offset;

    private void Start()
    {
         
        LevelManager.instance.onPlayerEnterNewArea += MoveGrid;
    }

    //sets the center component of the aStar grid equal to a new Vector2
    //the Vector2 should represent the position of the current room the player is inside of
    private void MoveGrid()
    {
        //get the current room the player is inside of
        BasicDungeon currentRoom = LevelManager.instance.GetCurrentRoom();

        //get the coordinates of the current room
        Vector2 currentRoomCoordinate = currentRoom.localRoomCoordinate;

        //the new position the graph will move towards
        //the x & y values are multiply by 150 and 110 because that would return the actual transform position values
        Vector2 coordinateToMoveGraphToward = new Vector2(currentRoomCoordinate.x * 150 + offset.x, currentRoomCoordinate.y * 110 + offset.y);


        //get the graph of the scene
        var graph = AstarPath.active.data.gridGraph;

        //set the graph's center equal to the coordinate we generated
        graph.center = coordinateToMoveGraphToward;

        //need to scan otherwise ai wouldn't actually know that the graph moved
        graph.Scan();

    }
}
