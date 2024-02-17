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
        init();
    }

    public void init()
    {
        BGMSlider.value = SaveData.instance.bgmVolumeSet;
        SFXSlider.value = SaveData.instance.sfxVolumeSet;

        BGMSlider.onValueChanged.AddListener((val) => {
            audiomixer.SetFloat("BGM", SaveData.instance.bgmVolumeSet);
        });
        SFXSlider.onValueChanged.AddListener((val) => {
            audiomixer.SetFloat("SFX", SaveData.instance.sfxVolumeSet);
        });
    }

    public void SetBGMVolume()
    {
        float sound = BGMSlider.value;

        if (sound <= -20f)
            audiomixer.SetFloat("BGM", -80);
        else
            audiomixer.SetFloat("BGM", sound);

        SaveData.instance.bgmVolumeSet = sound;
    }


    public void SetSFXVolume()
    {
        float sound = SFXSlider.value;

        if (sound <= -20f)
            audiomixer.SetFloat("SFX", -80);
        else
            audiomixer.SetFloat("SFX", sound);

        SaveData.instance.sfxVolumeSet = sound;
    }
}
