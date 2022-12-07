using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private float timeUntilFadeInStarts;
    [SerializeField] private float timeUntilFadeOutStarts;

    [SerializeField] private string fadeInAnimation;
    [SerializeField] private string fadeOutAnimation;

    private bool fadeInBool = false;
    private bool fadeOutBool = false;

    private void Start()
    {
        if(LevelManager.instance != null)
        {
            LevelManager.instance.onAllRoomsSpawned += FadeInWithTime;
        }
        if(EnemyManager.enemyManagerInstance != null)
        {
            EnemyManager.enemyManagerInstance.onBossKill += FadeOutWithTime;
        }

        if(PlayerStats.instance != null)
        {
            PlayerStats.instance.AccessPlayerHealth().playerIsCompletelyDead += FadeOutWithTime;
        }
            
    }

    public void FadeOut()
    {
        animator.Play(fadeOutAnimation);
    }

    // fade in begins after a few seconds
    public void FadeInWithTime()
    {
        if (!fadeInBool)
            StartCoroutine(FadeInCoroutine(timeUntilFadeInStarts));
    }

    // fade out begins after a few seconds
    public void FadeOutWithTime()
    {
        if (!fadeInBool)
            StartCoroutine(FadeInCoroutine(timeUntilFadeInStarts));
    }

    public void FadeIn()
    {
        animator.Play(fadeInAnimation);
    }

    IEnumerator FadeInCoroutine(float timer)
    {
        fadeInBool = true;
        yield return new WaitForSeconds(timer);
        animator.Play(fadeInAnimation);
        fadeInBool = false;
    }

    IEnumerator FadeOutCoroutine(float timer)
    {
        fadeOutBool = true;
        yield return new WaitForSeconds(timer);
        animator.Play(fadeOutAnimation);
        fadeOutBool = false;
    }
}
