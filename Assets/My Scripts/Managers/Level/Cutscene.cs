using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Animator cameraAnimator;

    [Header("Intro Cutscene")]

    [SerializeField] private CinemachineVirtualCamera introCamera;
    [SerializeField] private float introCutsceneDuration;
    [SerializeField] private float introCameraOffsetX;

    // Start is called before the first frame update
    void Start()
    {
        // wait for all rooms to spawn first

        LevelManager.instance.onAllRoomsSpawned += IntroCutscene;
    }

    private void IntroCutscene()
    {
        BossRoom bossRoom = LevelManager.instance.GetBossRoom();

        Transform centerOfRoom = bossRoom.GetCenterOfRoom();

        // set the introCamera's position to the boss room's position
        introCamera.transform.position = new Vector3(centerOfRoom.position.x, centerOfRoom.position.y, introCamera.transform.position.z);

        // introCamera should look at boss room
        introCamera.LookAt = bossRoom.GetCenterOfRoom();

        cameraAnimator.SetBool("introCutscene", true);

        // stop cutscene after 5 seconds
        Invoke(nameof(StopIntroCutscene), introCutsceneDuration);

    }

    private void StopIntroCutscene()
    {
        cameraAnimator.SetBool("introCutscene", false);

        LevelManager.instance.onAllRoomsSpawned -= IntroCutscene;
    }
}
