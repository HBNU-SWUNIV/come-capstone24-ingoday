using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //public AudioSource bgmSource;
    //public AudioSource sfxSource;
    public SoundEffectManager soundEffectManager;
    private static AudioManager instance;

    // ���� ������ �����ϴ� ����
    private float bgmVolume = 1.0f;
    private float sfxVolume = 1.0f;

    


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBGMVolume(float volume)  // �������
    {
        bgmVolume = volume;

        if (soundEffectManager.bgmAudio != null)
            soundEffectManager.bgmAudio.volume = bgmVolume;

    }

    public float GetBGMVolume()
    {
        return bgmVolume;
    }

    public void SetSFXVolume(float volume)  // ȿ����
    {
        sfxVolume = volume;

        if (soundEffectManager.buttonClickAudio != null)
            soundEffectManager.buttonClickAudio.volume = sfxVolume;
        if (soundEffectManager.getResourceAudio != null)
            soundEffectManager.getResourceAudio.volume = sfxVolume;
        if (soundEffectManager.playerAudioSource != null)
            soundEffectManager.playerAudioSource.volume = sfxVolume;

    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }

    void Start()
    {
        soundEffectManager = GameObject.Find("SoundEffectManager").GetComponent<SoundEffectManager>();

        SetBGMVolume(bgmVolume);
        SetSFXVolume(sfxVolume);
    }

    void Update()
    {
        
    }
}
