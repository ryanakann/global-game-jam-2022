using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider slider;

    public bool music;

    public void SetVolume()
    {
        if (music)
        {
            mixer.SetFloat("MusicVolume", slider.value);
        }
        else
        {
            mixer.SetFloat("SFXVolume", slider.value);
        }
    }
}
