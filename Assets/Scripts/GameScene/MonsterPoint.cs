using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPoint : MonoBehaviour
{
    //怪物有多少波
    public int maxWave;
    //每波怪物多少只
    public int monsterNumPerWave;
    //用于记录当前波的怪物还有多少只没创建
    private int nowNum;
    //怪物ID
    public List<int> monsterIDs;
    //当前波要创建的怪物的ID
    private int nowID;
    //每只怪物的创建间隔
    public float createOffsetPerMonster;
    //每波怪物的创建间隔
    public float createOffsetPerWave;
    //第一波怪物创建的间隔时间
    public float firstOffset;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("CreateWave",firstOffset);
        GameLevelMgr.Instance.AddMonsterPoint(this);
        GameLevelMgr.Instance.UpdateMaxNum(maxWave);
    }
    /// <summary>
    /// 开始创建一波的怪物
    /// </summary>
    private void CreateWave() {
        nowID = monsterIDs[Random.Range(0,monsterIDs.Count)];
        nowNum = monsterNumPerWave;
        //减少波数
        --maxWave;
        //更新UI
        GameLevelMgr.Instance.ChangeNowWaveNum(1);
        //创建怪物
        CreateMonster();
    }
    /// <summary>
    /// 创建怪物
    /// </summary>
    private void CreateMonster() {
        //创建怪物
        MonsterInfo info = GameDataMgr.Instance.monsterInfoList[nowID - 1];

        GameObject obj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        MonsterObject monsterObject = obj.AddComponent<MonsterObject>();
        monsterObject.InitInfo(info);
        GameLevelMgr.Instance.AddMonster(monsterObject);
        nowNum--;
        if(nowNum == 0) {
            //当前波怪物创建完了
            if(maxWave > 0) {
                Invoke("CreateWave",createOffsetPerWave);
            }
        }
        else {
            Invoke("CreateMonster",createOffsetPerMonster);
        }
    }
    /// <summary>
    /// 出怪点是否出怪结束
    /// </summary>
    /// <returns></returns>
    public bool CheckOver() {
        return nowNum == 0 && maxWave == 0;
    }
}
