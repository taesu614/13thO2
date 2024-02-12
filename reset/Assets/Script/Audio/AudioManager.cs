using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;

    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int sfxChannels;
    AudioSource[] sfxPlayers;
    int sfxChannelIndex;

    public enum BGM { main, battle, riddle};
    public enum SFX { draw, roulette, shield, attack, openClick, closeClick, success};
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
        Init();
        bgmPlayer.Play();
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;

        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.loop = true;
        bgmPlayer.clip = bgmClips[0];   // 일단 메인 브금으로 초기화

        //bgmplayer = bgmobject.addcomponent<audiosource>();
        //bgmplayer.playonawake = false;
        //bgmplayer.loop = true;
        //bgmplayer.volume = bgmvolume;
        //bgmplayer.clip = bgmclip;

        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[sfxChannels];

        for(int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
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
            case (int)BGM.battle:
                bgmPlayer.clip = bgmClips[1];
                break;
            case (int)BGM.riddle:
                bgmPlayer.clip = bgmClips[2];
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
}
