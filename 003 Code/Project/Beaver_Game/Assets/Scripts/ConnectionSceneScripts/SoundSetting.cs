using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;
    public AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindObjectOfType<AudioManager>();


        // �ʱ� ������ �����ϰų� ����� ������ �ҷ���
        bgmSlider.value = audioManager.GetBGMVolume();
        sfxSlider.value = audioManager.GetSFXVolume();

        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetBGMVolume(float volume)
    {
        audioManager.SetBGMVolume(volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioManager.SetSFXVolume(volume);
    }
}
