using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Button btnSure;
    public Text textInfo;
    public override void Init()
    {
        btnSure.onClick.AddListener(() => { UIManager.Instance.HidePanel<TipPanel>(); });
    }
    /// <summary>
    /// 改变提示内容的方法
    /// </summary>
    /// <param name="info"></param>
    public void ChangeInfo(string info)
    {
        textInfo.text = info;
    }
}
