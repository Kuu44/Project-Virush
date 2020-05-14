using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource[] sounds;
    public GameObject muteButton;
    public GameObject N_muteButton;
    public GameObject MmuteButton;
    public GameObject MN_muteButton;
    private void Awake()
    { 

        if (PlayerPrefs.GetInt("SoundON", 1) == 1)
        {
            muteButton.SetActive(true);
            N_muteButton.SetActive(false);
            Game.toggles.muted = false;
            for (int i = 0; i < sounds.Length; i++)
            {
                if (i != 3)
                    sounds[i].mute = false;
            }
            /*sounds[0].mute = false;
            sounds[3].mute = false;*/
        }
        else
        {
            muteButton.SetActive(false);
            N_muteButton.SetActive(true);
            Game.toggles.muted = true;
            for (int i = 0; i < sounds.Length; i++)
            {
                if(i!=3)
                sounds[i].mute = true;
            }
            /*sounds[0].mute = false;
            sounds[3].mute = false;*/
        }

        if (PlayerPrefs.GetInt("MusicON", 1) == 1)
        {
            MmuteButton.SetActive(true);
            MN_muteButton.SetActive(false);
            Game.toggles.mutedM = false;

                sounds[3].mute = false;
            
        }
        else
        {
            MmuteButton.SetActive(false);
            MN_muteButton.SetActive(true);
            Game.toggles.mutedM = true;

                sounds[3].mute = true;

        }

        Game.controllers.soundController = this;
    }

    public void mute()
    {
        PlayerPrefs.SetInt("SoundON", 0);
        Game.toggles.muted = true;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (i != 3)
            {
                sounds[i].mute = true;
                sounds[i].Stop();
            }
        }
    }

    public void muteM()
    {
        PlayerPrefs.SetInt("MusicON", 0);
        Game.toggles.mutedM = true;

        {

            sounds[3].mute = true;
            sounds[3].Stop();
        }
    }

    public void unmute()
    {
        PlayerPrefs.SetInt("SoundON", 1);
        Game.toggles.muted = false;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (i != 3)
            {
                sounds[i].mute = false;
            }
        }

    }

    public void unmuteM()
    {
        PlayerPrefs.SetInt("MusicON", 1);
        Game.toggles.mutedM = false;

        {
            sounds[3].mute = false;
        }
        sounds[3].Play();
    }


    public void splatSound()
    {
        if (!Game.toggles.muted)
        {
            sounds[0].pitch = Random.Range(0.75f,1.25f);
            sounds[0].Play();            
        }
    }
}
