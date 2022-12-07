using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    [SerializeField] private int creditsSceneInt;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        instance = this;

        //if (instance != null && instance != this)
        //{
        //    Destroy(this);
        //}
        //else
        //{
        //    instance = this;
        //}
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoadSubscribe;
    }

    public void LoadScene(int sceneIndex)
    {
        
        SceneManager.LoadScene(sceneIndex);



    }

    public void OnSceneLoadSubscribe(Scene scene, LoadSceneMode mode)
    {
        if (EnemyManager.enemyManagerInstance != null)
            EnemyManager.enemyManagerInstance.onBossKill += LoadCredits;

        if (PlayerStats.instance != null)
        {
            PlayerStats.instance.AccessPlayerHealth().playerIsCompletelyDead += LoadCredits;
        }
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene(creditsSceneInt);
    }

    private void OnDisable()
    {
        if (EnemyManager.enemyManagerInstance != null)
            EnemyManager.enemyManagerInstance.onBossKill -= LoadCredits;
    }
    private void OnDestroy()
    {
        if (EnemyManager.enemyManagerInstance != null)
            EnemyManager.enemyManagerInstance.onBossKill -= LoadCredits;
    }

}
