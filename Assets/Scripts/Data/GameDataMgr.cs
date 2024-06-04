using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 专门的数据管理类
/// </summary>
public class GameDataMgr
{
    private static GameDataMgr instance = new GameDataMgr();
    public static GameDataMgr Instance => instance;
    public MusicData musicData;
    //所有的角色数据
    public List<RoleInfo> roleInfoList;
    //玩家相关数据
    public PlayerData playerData;
    //记录选择的角色数据，用于在之后的游戏场景中创建
    public RoleInfo nowSelRole;
    //所有的场景数据
    public List<SceneInfo> sceneInfoList;
    //所有怪物的数据
    public List<MonsterInfo> monsterInfoList;
    //所有塔的数据
    public List<TowerInfo> towerInfoList;
    private GameDataMgr()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        roleInfoList = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        sceneInfoList = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        monsterInfoList = JsonMgr.Instance.LoadData<List<MonsterInfo>>("MonsterInfo");
        towerInfoList = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }
    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData,"MusicData");
    }
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData, "PlayerData");
    }
    /// <summary>
    /// 播放音效的方法
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySound(string soundName) {
        GameObject musicObj = new GameObject();
        AudioSource a = musicObj.AddComponent<AudioSource>();
        a.clip = Resources.Load<AudioClip>(soundName);
        a.volume = musicData.soundIntensity;
        a.mute = !musicData.isOpenSound;
        a.Play();
        GameObject.Destroy(musicObj,1);
    }
}
