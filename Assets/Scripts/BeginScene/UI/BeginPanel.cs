using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSettings;
    public Button btnAbouting;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() => 
        {
            Camera.main.GetComponent<CameraAnimator>().TurnLeft(() => 
            {
                UIManager.Instance.ShowPanel<ChooseHeroPanel>();
            });
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSettings.onClick.AddListener(() => { UIManager.Instance.ShowPanel<SettingPanel>(); });
        btnAbouting.onClick.AddListener(() => { });
        btnQuit.onClick.AddListener(() => { Application.Quit(); });
    }
}
