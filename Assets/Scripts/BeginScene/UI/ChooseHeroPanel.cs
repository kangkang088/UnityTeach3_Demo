using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChooseHeroPanel : BasePanel
{
    public Button btnLeft;
    public Button btnRight;
    public Button btnUnLock;
    public Text textUnLock;
    public Button btnStart;
    public Button btnBack;
    public Text textMoney;
    public Text textName;

    //英雄创建位置
    private Transform heroPos;
    //当前场景中的对象
    private GameObject heroObj;
    //当前使用的数据
    private RoleInfo nowRoleData;
    //当前使用数据的索引
    private int nowIndex;
    public override void Init()
    {
        heroPos = GameObject.Find("HeroPos").transform;
        //更新玩家拥有的钱
        textMoney.text = GameDataMgr.Instance.playerData.haveMoney.ToString();
        btnLeft.onClick.AddListener(() =>
        {
            nowIndex--;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.roleInfoList.Count - 1;
            //模型的更新
            ChangeHero();
        });
        btnRight.onClick.AddListener(() =>
        {
            nowIndex++;
            if (nowIndex > GameDataMgr.Instance.roleInfoList.Count - 1)
                nowIndex = 0;
            //模型的更新
            ChangeHero();
        });
        btnStart.onClick.AddListener(() =>
        {
            //记录当前选择的角色(因为切换场景会删除)
            GameDataMgr.Instance.nowSelRole = nowRoleData;
            //隐藏自己，显示场景选择界面
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            UIManager.Instance.ShowPanel<ChooseScenePanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ChooseHeroPanel>();
            Camera.main.GetComponent<CameraAnimator>().TurnRight(() =>
            {
                UIManager.Instance.ShowPanel<BeginPanel>();
            });
        });
        btnUnLock.onClick.AddListener(() =>
        {
            PlayerData playerData = GameDataMgr.Instance.playerData;
            if (playerData.haveMoney >= nowRoleData.lockMoney)
            {
                //购买逻辑
                playerData.haveMoney -= nowRoleData.lockMoney;
                textMoney.text = playerData.haveMoney.ToString();
                //记录购买的ID
                playerData.boughtHeros.Add(nowRoleData.id);
                GameDataMgr.Instance.SavePlayerData();
                //更新解锁按钮
                UpdateLockButton();

                //提示面板显示购买成功
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买成功!");
            }
            else
            {
                //提示面板显示金钱不足
                UIManager.Instance.ShowPanel<TipPanel>().ChangeInfo("购买失败!");
            }
        });
        //初始化显示模型
        ChangeHero();
    }
    /// <summary>
    /// 更新场景上显示的模型
    /// </summary>
    private void ChangeHero()
    {
        if (heroObj != null)
        {
            Destroy(heroObj);
            heroObj = null;
        }
        nowRoleData = GameDataMgr.Instance.roleInfoList[nowIndex];
        //实例化并记录下来，用于被切换时被删除
        heroObj = Instantiate(Resources.Load<GameObject>(nowRoleData.res), heroPos.position, heroPos.rotation);
        Destroy(heroObj.GetComponent<PlayerObject>());
        //更新上方显示的描述信息
        textName.text = nowRoleData.tips;
        //根据解锁相关数据决定是否显示解锁按钮
        UpdateLockButton();
    }
    /// <summary>
    /// 更新解锁按钮显示情况
    /// </summary>
    private void UpdateLockButton()
    {
        //如果该角色需要解锁，并且没有解锁，就要显示解锁按钮并隐藏开始按钮
        if (nowRoleData.lockMoney > 0 && !GameDataMgr.Instance.playerData.boughtHeros.Contains(nowRoleData.id))
        {
            btnUnLock.gameObject.SetActive(true);
            textUnLock.text = "￥:" + nowRoleData.lockMoney;
            btnStart.gameObject.SetActive(false);
        }
        else
        {
            btnUnLock.gameObject.SetActive(false);
            btnStart.gameObject.SetActive(true);
        }
    }
    public override void HideMe(UnityAction callback)
    {
        base.HideMe(callback);
        if (heroObj != null)
        {
            DestroyImmediate(heroObj);
            heroObj = null;
        }
    }
}
