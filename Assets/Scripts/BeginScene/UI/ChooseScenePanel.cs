using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChooseScenePanel : BasePanel
{
    public Button btnStart;
    public Button btnBack;
    public Button btnLeft;
    public Button btnRight;
    public Image imgScene;
    public Text textInfo;

    //当前的场景索引
    private int nowIndex;
    //当前的选择场景
    private SceneInfo nowSceneInfo;
    public override void Init()
    {
        btnLeft.onClick.AddListener(() => 
        {
            nowIndex--;
            if (nowIndex < 0)
                nowIndex = GameDataMgr.Instance.sceneInfoList.Count - 1;
            ChangeScene();
        });
        btnRight.onClick.AddListener(() => 
        {
            nowIndex++;
            if (nowIndex >= GameDataMgr.Instance.sceneInfoList.Count)
                nowIndex = 0;
            ChangeScene();
        });
        btnStart.onClick.AddListener(() => 
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            //切换场景
            AsyncOperation ao = SceneManager.LoadSceneAsync(nowSceneInfo.sceneName);
            //关卡初始化
            ao.completed += (obj) => {
                GameLevelMgr.Instance.InitInfo(nowSceneInfo);
            };
        });
        btnBack.onClick.AddListener(() => 
        {
            UIManager.Instance.HidePanel<ChooseScenePanel>();
            UIManager.Instance.ShowPanel<ChooseHeroPanel>();
        });
        ChangeScene();
    }
    /// <summary>
    /// 切换场景选择界面显示的场景信息
    /// </summary>
    public void ChangeScene()
    {
        nowSceneInfo = GameDataMgr.Instance.sceneInfoList[nowIndex];
        //更新图片和显示的文字信息
        imgScene.sprite = Resources.Load<Sprite>(nowSceneInfo.imgRes);
        textInfo.text = "名称:" + nowSceneInfo.name + "\n" + "描述:" + nowSceneInfo.tips;
    }
}
