using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTower : MonoBehaviour
{
    private int hp;
    private int maxHP;
    private bool isDead = false;
    private static MainTower instance;
    public static MainTower Instance => instance;
    private MainTower() {
    }
    private void Awake() {
        instance = this;    
    }
    /// <summary>
    /// 更新血量
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxHP"></param>
    public void UpdateHP(int hp,int maxHP) {
        this.hp = hp;
        this.maxHP = maxHP;
        UIManager.Instance.GetPanel<GamePanel>().UpdateTowerHP(hp,maxHP);
    }
    /// <summary>
    /// 受伤
    /// </summary>
    /// <param name="damage"></param>
    public void Wound(int damage) {
        if(isDead)
            return;
        hp -= damage;
        if(hp <= 0) {
            hp = 0;
            isDead = true;
            //游戏结束
            GameOverPanel panel = UIManager.Instance.ShowPanel<GameOverPanel>();
            panel.InitInfo((int)(GameLevelMgr.Instance.player.money * 0.5f),false);
        }
        UpdateHP(hp,maxHP);
    }
    private void OnDestroy() {
        instance = null;
    }
}
