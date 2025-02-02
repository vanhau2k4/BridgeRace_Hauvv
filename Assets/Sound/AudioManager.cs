using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadAudioSettings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("PlayGame");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not Found");
        }
        else
        {
            musicSource.clip = s.sound;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            sfxSource.PlayOneShot(s.sound);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
        PlayerPrefs.SetInt("MusicMute", musicSource.mute ? 1 : 0); // Lưu trạng thái mute
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        PlayerPrefs.SetInt("SFXMute", sfxSource.mute ? 1 : 0); // Lưu trạng thái mute
    }

    public void MusicVolume(float _volume)
    {
        musicSource.volume = _volume;
        PlayerPrefs.SetFloat("MusicVolume", _volume); // Lưu âm lượng
    }

    public void SFXVolume(float _volume)
    {
        sfxSource.volume = _volume;
        PlayerPrefs.SetFloat("SFXVolume", _volume); // Lưu âm lượng
    }

    private void LoadAudioSettings()
    {
        musicSource.mute = PlayerPrefs.GetInt("MusicMute", 0) == 1;
        sfxSource.mute = PlayerPrefs.GetInt("SFXMute", 0) == 1;
    }
}
