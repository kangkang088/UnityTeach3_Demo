using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObject : MonoBehaviour {
    //炮台头部，旋转指向目标
    public Transform head;
    //开火点，用于发射子弹和制造特效
    public Transform gunPoint;
    //炮台头部旋转速度
    private float roundSpeed = 20;
    //炮台关联的数据
    private TowerInfo info;
    //当前要攻击的目标
    private MonsterObject targetObj;
    private List<MonsterObject> targetObjs; 
    //用于计时，判断攻击间隔时间
    private float nowTime;
    //用于记录怪物位置
    private Vector3 monsterPos;

    void Update() {
        if(info.atkType == 1) {
            //单体攻击
            if(targetObj == null || targetObj.isDead || Vector3.Distance(this.transform.position,targetObj.transform.position) > info.atkRange) {
                //寻找目标
                targetObj = GameLevelMgr.Instance.FindMonster(this.transform.position,info.atkRange);
            }
            //没有找到可以攻击的对象，炮台不动
            if(targetObj == null)
                return;
            monsterPos = targetObj.transform.position;
            monsterPos.y = head.position.y;
            head.rotation = Quaternion.Slerp(head.rotation,Quaternion.LookRotation(monsterPos - head.position),roundSpeed * Time.deltaTime);
            if(Vector3.Angle(head.forward,monsterPos - head.position) < 5 && (Time.time - nowTime) >= info.offsetTime) {
                //让目标受伤
                targetObj.Wound(info.atk);
                GameDataMgr.Instance.PlaySound("Music/Tower");
                //创建开火特效
                GameObject obj = Instantiate(Resources.Load<GameObject>(info.eff),gunPoint.position,gunPoint.rotation);
                Destroy(obj,0.2f);
                nowTime = Time.time;
            }
        }
        else {
            //群体攻击
            targetObjs = GameLevelMgr.Instance.FindMonsters(this.transform.position,info.atkRange);
            if(targetObjs.Count > 0 && (Time.time - nowTime) >= info.offsetTime) {
                //创建开火特效
                GameObject obj = Instantiate(Resources.Load<GameObject>(info.eff),this.transform.position,this.transform.rotation);
                Destroy(obj,0.2f);
                for(int i = 0;i < targetObjs.Count;i++) {
                    targetObjs[i].Wound(info.atk);
                }
                nowTime = Time.time;
        }
        }
    }
    private void Start() {
        //InitInfo(GameDataMgr.Instance.towerInfoList[6]);
    }
    /// <summary>
    /// 初始化炮台相关数据
    /// </summary>
    /// <param name="info"></param>
    public void InitInfo(TowerInfo info) {
        this.info = info;
    }
}
