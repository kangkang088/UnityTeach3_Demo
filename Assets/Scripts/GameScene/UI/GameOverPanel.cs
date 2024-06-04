using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel {
    public Button btnSure;
    public Text textWin;
    public Text textInfo;
    public Text textMoney;
    public override void Init() {
        btnSure.onClick.AddListener(() => 
        {
            //隐藏面板,切换场景
            UIManager.Instance.HidePanel<GameOverPanel>();
            UIManager.Instance.HidePanel<GamePanel>();
            GameLevelMgr.Instance.ClearInfo();
            SceneManager.LoadScene("BeginScene");            
        });
    }
    public void InitInfo(int money,bool isWin) {
        textWin.text = isWin ? "通关" : "失败";
        textInfo.text = isWin ? "获得通关奖励" : "获得失败奖励";
        textMoney.text = "$" + money;
        //根据奖励，改变玩家数据
        GameDataMgr.Instance.playerData.haveMoney += money;
        GameDataMgr.Instance.SavePlayerData();
    }
    public override void ShowMe() {
        base.ShowMe();
        Cursor.lockState = CursorLockMode.None;
    }
}
