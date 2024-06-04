using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 组合控件，主要用于控制造塔相关ui的设置
/// </summary>
public class TowerButton : MonoBehaviour
{
    public Image imgTower;
    public Text textTitle;
    public Text textMoney;
    public void InitInfo(int id,string inputStr) {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        imgTower.sprite = Resources.Load<Sprite>(info.imgRes);
        textMoney.text = "$" + info.money;
        textTitle.text = inputStr;
        if(info.money > GameLevelMgr.Instance.player.money) {
            textMoney.text = "金钱不足";
        }
    }
}
