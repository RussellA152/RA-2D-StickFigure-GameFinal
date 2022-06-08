using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    /*
    public static ComponentManager instance { get; private set; }

    private GameObject player;
    private GameObject playerSprite;
    private GameObject mainCamera;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera");
        playerSprite = GameObject.Find("Player Sprite Renderer");

        SetUpPlayer();
        SetUpCamera();

    }

    public void SetUpPlayer()
    {
        var animator = playerSprite.gameObject.GetComponent<Animator>();

        player.GetComponent<PlayerMove>().controller = player.GetComponent<CharacterController2D>();

        player.GetComponent<CharacterController2D>().animator = animator;
        player.GetComponent<AttackController>().animator = animator;
    }

    public void SetUpCamera()
    {
        mainCamera.GetComponent<CameraFollow>().playerTransform = player.transform;
    }

    */


}
