using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private float speed;
    private bool restoreTime = false;

    // Start is called before the first frame update
    void Start()
    {
        // if the cinemachine impulses do not ignore timescale, then the hitstop will freeze the screenshakes as well (which we don't want)
        CinemachineImpulseManager.Instance.IgnoreTimeScale = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (restoreTime)
        {
            if(Time.timeScale < 1f)
            {
                Time.timeScale += Time.deltaTime * speed;
            }
            else
            {
                Time.timeScale = 1f;
                restoreTime = false;
            }
        }
    }

    public void StopTime(float changeTime, int restoreSpeed, float delay)
    {
        speed = restoreSpeed;

        if(delay > 0)
        {
            StopCoroutine(StartTimeAgain(delay));
            StartCoroutine(StartTimeAgain(delay));
        }
        else
        {
            restoreTime = true;
        }
        Time.timeScale = changeTime;
    }
    IEnumerator StartTimeAgain(float amount)
    {
        yield return new WaitForSecondsRealtime(amount);
        restoreTime = true;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // in case this gets disabled, we reset timescale back to 1
        Time.timeScale = 1f;
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
        // in case this gets destroyed, we reset timescale back to 1
        Time.timeScale = 1f;
    }
}
