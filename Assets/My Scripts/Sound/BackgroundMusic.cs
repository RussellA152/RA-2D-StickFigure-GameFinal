using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    //[SerializeField] private AudioSource audioSource;

    [Header("Music for each room type")]
    //[SerializeField] private AudioSource startingRoomMusic;
    [SerializeField] private AudioSource normalCombatMusic;
    [SerializeField] private AudioSource bossCombatMusic;
    [SerializeField] private AudioSource treasureRoomMusic;
    [SerializeField] private AudioSource shopRoomMusic;

    private AudioSource previousAudioSource;


    private void Start()
    {
        treasureRoomMusic.Play();
        previousAudioSource = treasureRoomMusic;

        LevelManager.instance.onPlayerEnterNewArea += PlayBackgroundMusic;
    }

    public void PlayBackgroundMusic()
    {
        //Debug.Log("Play new background music!");

        switch (LevelManager.instance.GetCurrentRoom().roomType)
        {
            case BaseRoom.RoomType.normal:
                // starting room doesn't have combat music because there are no enemies
                if (!LevelManager.instance.GetCurrentRoom().isStartingRoom)
                {
                    if (!normalCombatMusic.isPlaying)
                    {
                        previousAudioSource.Pause();
                        previousAudioSource = normalCombatMusic;
                        normalCombatMusic.Play();
                    }
                    

                    //if(audioSource.clip != normalCombatMusic)
                    //{
                    //    audioSource.clip = normalCombatMusic;
                    //    audioSource.Play();
                    //}
                    
                }

                else
                {
                    if (!treasureRoomMusic.isPlaying)
                    {
                        previousAudioSource.Pause();
                        previousAudioSource = treasureRoomMusic;
                        treasureRoomMusic.Play();
                    }
                        
                    //// treasure room and starting room have the same music, so don't replay them
                    //if(audioSource.clip != startingRoomMusic || audioSource.clip != treasureRoomMusic)
                    //{
                    //    audioSource.clip = startingRoomMusic;
                    //    audioSource.Play();
                    //}

                }
                    
                break;

            case BaseRoom.RoomType.treasure:
                if (!treasureRoomMusic.isPlaying)
                {
                    previousAudioSource.Pause();
                    previousAudioSource = treasureRoomMusic;
                    treasureRoomMusic.Play();
                }
                
                //if(audioSource.clip != treasureRoomMusic)
                //{
                //    audioSource.clip = treasureRoomMusic;
                //    audioSource.Play();
                //}

                break;

            case BaseRoom.RoomType.shop:
                if (!shopRoomMusic.isPlaying)
                {
                    previousAudioSource.Pause();
                    previousAudioSource = shopRoomMusic;
                    shopRoomMusic.Play();
                }
                
                //if(audioSource.clip != shopRoomMusic)
                //{
                //    audioSource.clip = shopRoomMusic;
                //    audioSource.Play();
                //}

                break;

            case BaseRoom.RoomType.boss:
                if (!bossCombatMusic.isPlaying)
                {
                    previousAudioSource.Pause();
                    previousAudioSource = bossCombatMusic;
                    bossCombatMusic.Play();
                }
                
                //if(audioSource.clip != bossCombatMusic)
                //{
                //    audioSource.clip = bossCombatMusic;
                //    audioSource.Play();
                //}

                break;
        }
    }
}
