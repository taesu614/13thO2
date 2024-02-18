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
    }

    public void SetBGMVolume()
    {
        float sound = BGMSlider.value;

        if (sound <= -5f)
            audiomixer.SetFloat("BGM", -80f);
        else
            audiomixer.SetFloat("BGM", sound);

        Debug.Log("BGM " + (sound <= -20f));
        SaveData.instance.bgmVolumeSet = sound;
    }


    public void SetSFXVolume()
    {
        float sound = SFXSlider.value;

        if (sound <= -5f)
            audiomixer.SetFloat("SFX", -80f);
        else
            audiomixer.SetFloat("SFX", sound);

        Debug.Log("SFX " + (sound <= -20f));
        SaveData.instance.sfxVolumeSet = sound;
    }
}
