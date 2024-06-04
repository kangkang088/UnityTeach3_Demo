using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePanel : BasePanel {
    public Button btnQuit;
    public Image imgHP;
    public Text textHP;
    public Text textNumber;
    public Text textMoney;
    //hp的初始宽度
    public float hpW = 800;
    //下方塔组的父对象，主要用于控制显隐
    public Transform buttonsTrans;
    public List<TowerButton> towerButtons = new List<TowerButton>();
    //当前进入的造塔点
    private TowerPoint nowSelPoint;
    //用来标识是否检测造塔的输入
    private bool CheckInput;
    public override void Init() {
        btnQuit.onClick.AddListener(() => {
            UIManager.Instance.HidePanel<GamePanel>();
            SceneManager.LoadScene("BeginScene");
        });
        buttonsTrans.gameObject.SetActive(false);
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Confined;
    }
    /// <summary>
    /// 更新安全区血量
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="maxHP"></param>
    public void UpdateTowerHP(int hp,int maxHP) {
        textHP.text = hp + "/" + maxHP;
        (imgHP.transform as RectTransform).sizeDelta = new Vector2((float)hp / maxHP * hpW,38);
    }
    /// <summary>
    /// 更新剩余波数
    /// </summary>
    /// <param name="nowNumber"></param>
    /// <param name="maxNumber"></param>
    public void UpdateNumber(int nowNumber,int maxNumber) {
        textNumber.text = nowNumber + "/" + maxNumber;
    }
    /// <summary>
    /// 更新金币数量
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money) {
        textMoney.text = money.ToString();
    }
    /// <summary>
    /// 更新当前选中的造塔点界面的一些变化
    /// </summary>
    public void UpdateSelTower(TowerPoint point) {

        //根据造塔点的信息，更新界面显示的内容
        nowSelPoint = point;
        if(nowSelPoint == null) {
            CheckInput = false;
            buttonsTrans.gameObject.SetActive(false);
        }
        else {
            CheckInput = true;
            buttonsTrans.gameObject.SetActive(true);
            if(nowSelPoint.info == null) {
                for(int i = 0;i < towerButtons.Count;i++) {
                    towerButtons[i].gameObject.SetActive(true);
                    towerButtons[i].InitInfo(nowSelPoint.chooseIDs[i],"数字键" + (i + 1));
                }
            }
            else {
                for(int i = 0;i < towerButtons.Count;i++) {
                    towerButtons[i].gameObject.SetActive(false);
                }
                towerButtons[1].gameObject.SetActive(true);
                towerButtons[1].InitInfo(nowSelPoint.info.nextLevel,"空格键");
            }
        }
    }
    protected override void Update() {
        base.Update();
        if(!CheckInput)
            return;
        //造塔点键盘输入造塔
        //如果没有造过塔，显示123去建造
        if(nowSelPoint.info == null) {
            if(Input.GetKeyDown(KeyCode.Alpha1)) {
                nowSelPoint.CreateTower(nowSelPoint.chooseIDs[0]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2)) {
                nowSelPoint.CreateTower(nowSelPoint.chooseIDs[1]);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3)) {
                nowSelPoint.CreateTower(nowSelPoint.chooseIDs[2]);
            }
        }
        //造过塔，检测空格键去升级
        else {
            if(Input.GetKeyDown(KeyCode.Space)) {
                nowSelPoint.CreateTower(nowSelPoint.info.nextLevel);
            }
        }
    }
}
