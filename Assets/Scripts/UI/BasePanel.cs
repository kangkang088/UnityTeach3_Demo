using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    //专门控制面板透明度
    private CanvasGroup canvasGroup;
    private float fadeSpeed = 10.0f;
    //标识当前是否显示自己
    public bool isShow = false;
    //隐藏自己后所需要调用的委托
    private UnityAction hideCallback = null;
    protected virtual void Awake()
    {
        canvasGroup = this.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
    }
    protected virtual void Start()
    {
        Init();
    }
    /// <summary>
    /// 用于注册控件事件的方法，所有子面板都需要去注册一些控件事件
    /// </summary>
    public abstract void Init();
    /// <summary>
    /// 显示自己时要做的事情
    /// </summary>
    public virtual void ShowMe()
    {
        isShow = true;
        canvasGroup.alpha = 0;
    }
    /// <summary>
    /// 隐藏自己时的要做的事情
    /// </summary>
    public virtual void HideMe(UnityAction callback)
    {
        isShow = false;
        canvasGroup.alpha = 1;
        hideCallback = callback;
    }
    protected virtual void Update()
    {
        #region 淡入淡出
        if (isShow && canvasGroup.alpha != 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha >= 1)
                canvasGroup.alpha = 1;
        }
        else if (!isShow && canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            if (canvasGroup.alpha <= 0)
            {
                canvasGroup.alpha = 0;
                hideCallback?.Invoke();
            }
        }
        #endregion
    }
}
