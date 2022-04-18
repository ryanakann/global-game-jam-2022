using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{

    public AudioMixer mixer;

    public void SetLevel(float sliderValue)
    {
        if (sliderValue <= 0)
            sliderValue = -40;
        else
            sliderValue = Mathf.Log10(sliderValue) * 20;
        mixer.SetFloat("MasterVolume", sliderValue);
    }

}
