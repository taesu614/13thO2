using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioMixer audioMixer;
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

        Init();
        bgmPlayer.Play();
        instance = this;
    }

    void Init()
    {
        if (audioMixer != null)
        {
            AudioMixerGroup[] masterGroups = audioMixer.FindMatchingGroups("Master");
            GameObject bgmObject = GameObject.Find("BGMPlayer");
            GameObject sfxObject = GameObject.Find("SFXPlayer");
            sfxPlayers = new AudioSource[sfxChannels];

            if (masterGroups != null && masterGroups.Length > 0)
            {
                AudioMixerGroup bgmGroup = masterGroups[0].audioMixer.FindMatchingGroups("BGM")[0];

                bgmPlayer = bgmObject.AddComponent<AudioSource>();
                bgmPlayer.playOnAwake = false;
                bgmPlayer.volume = bgmVolume;
                bgmPlayer.loop = true;
                bgmPlayer.clip = bgmClips[0];   // 일단 메인 브금으로 초기화
                bgmPlayer.outputAudioMixerGroup = bgmGroup;

                AudioMixerGroup sfxGroup = masterGroups[0].audioMixer.FindMatchingGroups("SFX")[0];

                for (int index = 0; index < sfxPlayers.Length; index++)
                {
                    sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
                    sfxPlayers[index].playOnAwake = false;
                    sfxPlayers[index].volume = sfxVolume;
                    sfxPlayers[index].outputAudioMixerGroup = sfxGroup;
                }
            }
            else
            {
                Debug.LogError("Master group not found.");
            }
        }
        else
        {
            Debug.LogError("AudioMixer is null.");
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
        GameObject bgmObject = GameObject.Find("BGMPlayer");
        bgmPlayer = bgmObject.GetComponent<AudioSource>();
        bgmPlayer.volume = volume;
    }

    public void ChangeSFXVolume(int volume)
    {
        GameObject sfxObject = GameObject.Find("SFXPlayer");
        for(int i = 0; i < sfxPlayers.Length; i++)
        {
            sfxPlayers[i] = sfxObject.GetComponent<AudioSource>();
            sfxPlayers[i].volume = volume;
        }
    }

    public void DestroyAudioManager()
    {
        Destroy(gameObject);
    }
}
