using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cameraOffsetX;
    [SerializeField] private float cameraOffsetY;

    [SerializeField] private float offsetSmoothing;

    private Vector3 cameraFollowPosition;

    private float followSpeed = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        //cameraFollowPosition is the position of camera
        cameraFollowPosition = transform.position;

        //camera's x & y position equals player's x & y position
        cameraFollowPosition.x = playerTransform.position.x;
        cameraFollowPosition.y = playerTransform.position.y;

        //add cameraFollowPosition by an offset
        cameraFollowPosition.x += cameraOffsetX;
        cameraFollowPosition.y += cameraOffsetY;

        //set actual Main Camera's position using SmoothDamp so the camera can fall behind player 
        transform.position = Vector3.SmoothDamp(transform.position, cameraFollowPosition, ref velocity, followSpeed);
    }



}
