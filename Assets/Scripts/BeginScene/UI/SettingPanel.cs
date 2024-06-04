using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnClose;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        #region 初始化，根据本地数据
        MusicData data = GameDataMgr.Instance.musicData;
        toggleMusic.isOn = data.isOpenMusic;
        toggleSound.isOn = data.isOpenSound;
        sliderMusic.value = data.musicIntensity;
        sliderSound.value = data.soundIntensity;
        #endregion

        #region 数据修改与存储
        btnClose.onClick.AddListener(() => 
        {
            GameDataMgr.Instance.SaveMusicData();
            UIManager.Instance.HidePanel<SettingPanel>();
        });
        toggleMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.SetIsOpen(v);
            GameDataMgr.Instance.musicData.isOpenMusic = v;
        });
        toggleSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.isOpenSound = v;
        });
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.SetVolume(v);
            GameDataMgr.Instance.musicData.musicIntensity = v;
        });
        sliderSound.onValueChanged.AddListener((v) =>
        {
            GameDataMgr.Instance.musicData.soundIntensity = v;
        });
        #endregion

    }
}
