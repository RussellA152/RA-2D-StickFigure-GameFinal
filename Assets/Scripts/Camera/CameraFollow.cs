using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float cameraOffsetX;
    [SerializeField] private float cameraOffsetY;

    [SerializeField] private float offsetSmoothing;

    private Vector3 temp;


    private Vector3 velocity = Vector3.zero;

    private void Update()
    {
        //Application.targetFrameRate = 60;
    }
    private void LateUpdate()
    {

        //temp is the position of camera
        temp = transform.position;

        //camera's x & y position equals player's x & y position
        temp.x = playerTransform.position.x;
        temp.y = playerTransform.position.y;

        temp.x += cameraOffsetX;
        temp.y += cameraOffsetY;

        //transform.position = temp;
        //SMOOTH BUT BLURRY
        transform.position = Vector3.SmoothDamp(transform.position, temp, ref velocity, 0.3f);
    }

    private void FixedUpdate()
    {
        //NOT SMOOTH BUT NOT BLURRY
        //we update position inside of fixedUpdate because the player sprite becomes blurry when inside of lateUpdate
       // transform.position = Vector3.SmoothDamp(transform.position, temp, ref velocity, 0.3f);
    }


}
