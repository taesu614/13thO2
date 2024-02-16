using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audiomixer;
    public Slider BGMSlider;
    public Slider SFXSlider;

    void Start()
    {
        audiomixer.SetFloat("BGM", 0);
        audiomixer.SetFloat("SFX", 0);
    }

    public void SetBGMVolume()
    {
        float sound = BGMSlider.value;

        if (sound == -20f)
            audiomixer.SetFloat("BGM", -80);
        else
            audiomixer.SetFloat("BGM", sound);
    }

    public void SetSFXVolume()
    {
        float sound = SFXSlider.value;

        if (sound == -20f)
            audiomixer.SetFloat("SFX", -80);
        else
            audiomixer.SetFloat("SFX", sound);
    }
}
