using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSound : MonoBehaviour
{
    AudioSource waveAudio;
    AudioSource mainSound;

    // Start is called before the first frame update
    void Start()
    {
        waveAudio = gameObject.AddComponent<AudioSource>();
        mainSound = gameObject.AddComponent<AudioSource>();
        WavePlay("StartWaves");
        MainPlay("LevelStart");
    }

    public void MainPlay(string bgmName)
    {
        AudioClip Sound = (AudioClip)Resources.Load("Sound/" + bgmName);
        //사운드가 없다면 리턴
        if (Sound == null) return;

        //메인오디오의 클립에 새로운 오디오클을 연결한다.
        mainSound.clip = Sound;

        //메인오디오 플레이 하기
        mainSound.Play();
    }

    public void WavePlay(string bgmName)
    {
        AudioClip Sound = (AudioClip)Resources.Load("Sound/" + bgmName);
        //사운드가 없다면 리턴
        if (Sound == null) return;

        //메인오디오의 클립에 새로운 오디오클을 연결한다.
        waveAudio.clip = Sound;

        //메인오디오 플레이 하기
        waveAudio.Play();
    }
}
