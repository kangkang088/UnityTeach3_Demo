using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;
    private AudioSource bkSource;
    private void Awake()
    {
        instance = this;
        bkSource = this.GetComponent<AudioSource>();

        MusicData data = GameDataMgr.Instance.musicData;
        SetIsOpen(data.isOpenMusic);
        SetVolume(data.musicIntensity);
    }
    //开关背景音乐的方法
    public void SetIsOpen(bool isOpen)
    {
        bkSource.mute = !isOpen;
    }
    //调整音量的方法
    public void SetVolume(float volume)
    {
        bkSource.volume = volume;
    }
}
