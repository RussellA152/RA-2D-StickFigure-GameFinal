using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSounds : MonoBehaviour
{
    public static ObjectSounds instance;

    [SerializeField] private AudioSource mainAudioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void PlaySoundEffect(AudioClip soundEffect)
    {
        mainAudioSource.PlayOneShot(soundEffect);
    }
}
