using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixer audiomixer;

    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int sfxChannelIndex;

    public enum BGM { main, map ,battle, riddle};
    public enum SFX { draw, roulette, shield, attack, openClick, closeClick, success};
    void Awake()
    {
        var objs = GameObject.FindGameObjectsWithTag("AudioManager");
        if(objs.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        instance = this;
        Init();
        bgmPlayer.Play();
    }

    void Init()
    {
        GameObject bgmObject = GameObject.Find("BGMPlayer");
        bgmPlayer = bgmObject.GetComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClips[0];   // 일단 메인 브금으로 초기화


        GameObject sfxObject = GameObject.Find("SFXPlayer");

        sfxPlayers = new AudioSource[sfxChannels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.GetComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }

    }

    public void PlayBGM(BGM bgm)
    {
        bgmPlayer.Stop();
        switch ((int)bgm)
        {
            case (int)BGM.main:
                bgmPlayer.clip = bgmClips[0];
                break;
            case (int)BGM.map:
                bgmPlayer.clip = bgmClips[1];
                break;
            case (int)BGM.battle:
                bgmPlayer.clip = bgmClips[2];
                break;
            case (int)BGM.riddle:
                bgmPlayer.clip = bgmClips[3];
                break;
        }
        bgmPlayer.Play();

    }

    public void PlaySFX(SFX sfx)
    {
        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + sfxChannelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            sfxChannelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }
    }

    public bool CheckBGM(string playing)
    {
        if (bgmPlayer.clip.name == playing)
            return true;
        else
            return false;

    }

    public void ChangeBGMVolume(int volume)
    {
        bgmVolume = volume;
    }
}
