using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] ScreenFade fader;
    [SerializeField] private int firstSceneToLoad;
    [SerializeField] private int creditsScene;
    //private PlayerInputActions playerInputActions;
    //[SerializeField] private InputAction menu;

    // Start is called before the first frame update
    //private void Awake()
    //{
    //playerInputActions = new PlayerInputActions();
    //}

    // Update is called once per frame
    //void Update()
    //{

    //}
    private void Start()
    {
        fader.FadeIn();
    }
    public void PlayLevel1()
    {
        SceneLoader.instance.LoadScene(firstSceneToLoad);
    }
    public void LoadCredits()
    {
        SceneLoader.instance.LoadScene(creditsScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
