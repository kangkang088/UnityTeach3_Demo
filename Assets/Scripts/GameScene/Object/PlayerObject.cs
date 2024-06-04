using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    //玩家属性初始化
    private int atk;
    public int money;
    private float rotateSpeed = 50;

    private Animator animator;
    //开火点
    public Transform gunPoint;

    void Start()
    {
        animator = this.GetComponent<Animator>();
        //Camera.main.GetComponent<CameraMove>().SetTarget(GameObject.Find("1").transform);
    }
    /// <summary>
    /// 初始化玩家信息
    /// </summary>
    /// <param name="atk"></param>
    /// <param name="money"></param>
    public void InitPlayerInfo(int atk,int money) {
        this.atk = atk;
        this.money = money;
        UpdateMoney();
    }
    void Update()
    {
        //动作变化
        animator.SetFloat("VSpeed",Input.GetAxis("Vertical"));
        animator.SetFloat("HSpeed",Input.GetAxis("Horizontal"));
        this.transform.Rotate(Vector3.up,Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            animator.SetLayerWeight(animator.GetLayerIndex("New Layer"),1);
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift)) {
            animator.SetLayerWeight(animator.GetLayerIndex("New Layer"),0);
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            animator.SetTrigger("Roll");
        }
        if(Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Fire");
        }
    }
    //攻击动作的不同处理（碰撞检测方式）
    /// <summary>
    /// 专门用于处理刀的伤害检测
    /// </summary>
    public void KnifeEvent() {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward + this.transform.up,1,1 << LayerMask.NameToLayer("Monster"));
        GameDataMgr.Instance.PlaySound("Music/Knife");
        for(int i = 0;i < colliders.Length;i++) {
            //得到碰撞到的对象上的怪物脚本，让其受伤
            MonsterObject monsterObject = colliders[i].gameObject.GetComponent<MonsterObject>();
            if(monsterObject != null && !monsterObject.isDead) {
                monsterObject.Wound(atk);
                break;
            }
        }
    }
    /// <summary>
    /// 专门用于处理枪的伤害检测
    /// </summary>
    public void ShootEvent() {
        RaycastHit[] raycastHits = Physics.RaycastAll(new Ray(gunPoint.position,this.transform.forward),1000,1 << LayerMask.NameToLayer("Monster"));
        GameDataMgr.Instance.PlaySound("Music/Gun");
        for(int i = 0;i < raycastHits.Length;i++) {
            //得到射线检测到的对象上的怪物脚本，让其受伤
            MonsterObject monsterObject = raycastHits[i].collider.gameObject.GetComponent<MonsterObject>();
            if(monsterObject != null && !monsterObject.isDead) {
                //特效的创建
                GameObject obj = Instantiate(Resources.Load<GameObject>(GameDataMgr.Instance.nowSelRole.hitEff));
                obj.transform.position = raycastHits[i].point;
                obj.transform.rotation = Quaternion.LookRotation(raycastHits[i].normal);
                Destroy(obj,1);
                monsterObject.Wound(atk);
                break;
            }
        }
    }

    //金币更新
    public void UpdateMoney() {
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);
    }
    /// <summary>
    /// 加钱的方法
    /// </summary>
    /// <param name="money"></param>
    public void AddMoney(int money) {
        this.money += money;
        UpdateMoney();
    }
}
