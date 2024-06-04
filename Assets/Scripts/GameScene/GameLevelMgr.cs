using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelMgr {
    private static GameLevelMgr instance = new GameLevelMgr();
    public static GameLevelMgr Instance => instance;
    public PlayerObject player;
    //所有的出怪点
    private List<MonsterPoint> points = new List<MonsterPoint>();
    //记录当前还剩余的怪物波数
    private int nowWaveNum = 0;
    //记录一共有多少波
    private int maxWaveNum = 0;
    //当前场景上的怪物
    //private int nowMonsterNum = 0;
    //记录当前场景上的怪物的列表
    private List<MonsterObject> monsterObjectList = new List<MonsterObject>();
    private GameLevelMgr() {

    }
    //切换场景时，动态创建玩家
    public void InitInfo(SceneInfo sceneInfo) {
        UIManager.Instance.ShowPanel<GamePanel>();
        RoleInfo roleInfo = GameDataMgr.Instance.nowSelRole;
        //获取玩家出生点
        Transform heroPos = GameObject.Find("HeroBornPos").transform;
        GameObject heroObj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.res),heroPos.position,heroPos.rotation);
        player = heroObj.GetComponent<PlayerObject>();
        player.InitPlayerInfo(roleInfo.atk,sceneInfo.money);
        Camera.main.GetComponent<CameraMove>().SetTarget(heroObj.transform);
        MainTower.Instance.UpdateHP(sceneInfo.towerHP,sceneInfo.towerHP);
    }
    //判断游戏是否胜利
    /// <summary>
    /// 用于记录出怪点的方法
    /// </summary>
    /// <param name="point"></param>
    public void AddMonsterPoint(MonsterPoint point) {
        points.Add(point);
    }
    /// <summary>
    /// 更新一共有多少波怪
    /// </summary>
    /// <param name="num"></param>
    public void UpdateMaxNum(int num) {
        maxWaveNum += num;
        nowWaveNum = maxWaveNum;
        UIManager.Instance.GetPanel<GamePanel>().UpdateNumber(nowWaveNum,maxWaveNum);
    }
    /// <summary>
    /// 更新当前波怪
    /// </summary>
    /// <param name="num"></param>
    public void ChangeNowWaveNum(int num) {
        nowWaveNum -= num;
        UIManager.Instance.GetPanel<GamePanel>().UpdateNumber(nowWaveNum,maxWaveNum);
    }
    /// <summary>
    /// 检测是否出怪完成
    /// </summary>
    /// <returns></returns>
    public bool CheckOver() {
        for(int i = 0;i < points.Count;i++) {
            if(!points[i].CheckOver()) {
                return false;
            }
        }
        if(monsterObjectList.Count > 0)
            return false;

        Debug.Log("游戏胜利");
        return true;
    }
    /// <summary>
    /// 改变当前场景上怪物的数量
    /// </summary>
    /// <param name="num"></param>
    //public void ChangeMonsterNum(int num) {
    //    monsterObjectList.Count += num;
    //}
    
    //添加怪物到列表中
    public void AddMonster(MonsterObject monster) {
        monsterObjectList.Add(monster);
    }
    //死亡时列表中移除怪物
    public void RemoveMonster(MonsterObject monster) {
        monsterObjectList.Remove(monster);
    }
    //怪物列表中找到满足距离条件的单个怪物并返回，用于塔的攻击
    public MonsterObject FindMonster(Vector3 pos,int range) {
        for(int i = 0;i < monsterObjectList.Count;i++) {
            if(!monsterObjectList[i].isDead && Vector3.Distance(pos,monsterObjectList[i].transform.position) <= range) {
                return monsterObjectList[i];
            }
        }
        return null;
    }
    //怪物列表中找到满足距离条件的所有怪物并返回，用于塔的攻击
    public List<MonsterObject> FindMonsters(Vector3 pos,int range) {
        List<MonsterObject> monsterObjects = new List<MonsterObject>();
        for(int i = 0;i < monsterObjectList.Count;i++) {
            if(!monsterObjectList[i].isDead && Vector3.Distance(pos,monsterObjectList[i].transform.position) <= range) {
                monsterObjects.Add(monsterObjectList[i]);
            }
        }
        return monsterObjects;
    }
    /// <summary>
    /// 游戏结束，清空数据
    /// </summary>
    public void ClearInfo() {
        points.Clear();
        monsterObjectList.Clear();
        nowWaveNum = 0;
        maxWaveNum = 0;
        player = null;
    }
}
