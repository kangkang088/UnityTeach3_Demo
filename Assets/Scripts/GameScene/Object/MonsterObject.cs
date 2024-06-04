using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterObject : MonoBehaviour {
    //动画相关
    private Animator animator;
    //寻路组件
    private NavMeshAgent agent;
    //不变的基础数据
    private MonsterInfo monsterInfo;
    //变化的数据：血量
    private int hp;
    public bool isDead = false;
    //上一次攻击的事件
    private float frontTime;
    void Awake() {
        animator = this.GetComponent<Animator>();
        agent = this.GetComponent<NavMeshAgent>();
    }
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="monsterInfo"></param>
    public void InitInfo(MonsterInfo monsterInfo) {
        this.monsterInfo = monsterInfo;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(monsterInfo.animator);
        hp = monsterInfo.hp;
        //加速度
        agent.acceleration = monsterInfo.moveSpeed;
        //速度
        agent.speed = monsterInfo.moveSpeed;
        //旋转
        agent.angularSpeed = monsterInfo.roundSpeed;
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void Wound(int damage) {
        if(isDead)
            return;
        hp -= damage;
        animator.SetTrigger("Wound");
        if(hp <= 0) {
            //死亡
            Dead();
        }
        else {
            //播放受伤音效
            GameDataMgr.Instance.PlaySound("Music/Wound");
        }
    }
    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead() {
        isDead = true;
        agent.isStopped = true;
        agent.enabled = false;
        animator.SetBool("Death",true);
        //播放音效
        GameDataMgr.Instance.PlaySound("Music/Dead");
        //加钱
        GameLevelMgr.Instance.player.AddMoney(50);
    }
    /// <summary>
    /// 死亡动画播放完毕后的事件
    /// </summary>
    public void DeadEvent() {
        //移除对象
        GameLevelMgr.Instance.RemoveMonster(this);
        Destroy(this.gameObject);
        //怪物死亡时检测游戏是否胜利
        if(GameLevelMgr.Instance.CheckOver()) {
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo(GameLevelMgr.Instance.player.money,true);
        }
    }
    /// <summary>
    /// 出生结束后的事件
    /// </summary>
    public void BornOver() {
        agent.SetDestination(MainTower.Instance.transform.position);
        //播放移动动画
        animator.SetBool("Run",true);
    }
    private void Update() {
        //伤害检测。达到一定距离时开始检测
        if(isDead)
            return;
        animator.SetBool("Run",agent.velocity != Vector3.zero);
        if(Vector3.Distance(this.transform.position,MainTower.Instance.transform.position) < 5 && (Time.time - frontTime) > monsterInfo.atkOffset) {
            animator.SetTrigger("Attack");
            frontTime = Time.time;
        }
    }
    /// <summary>
    /// 范围检测
    /// </summary>
    public void AtkEvent() {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,1,1 << LayerMask.NameToLayer("MainTower"));
        GameDataMgr.Instance.PlaySound("Music/Eat");
        for(int i = 0;i < colliders.Length;i++) {
            if(MainTower.Instance.gameObject == colliders[i].gameObject) {
                //受伤害
                MainTower.Instance.Wound(monsterInfo.atk);
            }
        }
    }
}
