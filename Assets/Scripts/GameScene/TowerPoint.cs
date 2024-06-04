using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPoint : MonoBehaviour
{
    //造塔点关联的塔的对象
    private GameObject towerObj = null;
    //造塔点关联的塔的数据
    public TowerInfo info = null;
    //可以建造的三个塔的id
    public List<int> chooseIDs = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 造塔
    /// </summary>
    /// <param name="id"></param>
    public void CreateTower(int id) {
        TowerInfo info = GameDataMgr.Instance.towerInfoList[id - 1];
        //钱够不够
        if(info.money > GameLevelMgr.Instance.player.money)
            return;
        //扣钱
        GameLevelMgr.Instance.player.AddMoney(-info.money);
        //造塔
        //先判断之前有没有塔
        if(towerObj != null) {
            Destroy(towerObj);
            towerObj = null;
        }
        //实例化塔对象
        towerObj = Instantiate(Resources.Load<GameObject>(info.res),this.transform.position,Quaternion.identity);
        //初始化
        towerObj.GetComponent<TowerObject>().InitInfo(info);
        this.info = info;
        //塔建造完毕，更新UI
        if(this.info.nextLevel != 0) {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
        }
        else {
            UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
        }
    }
    private void OnTriggerEnter(Collider other) {
        if(info != null && info.nextLevel == 0)
            return; 
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(this);
    }
    private void OnTriggerExit(Collider other) {
        UIManager.Instance.GetPanel<GamePanel>().UpdateSelTower(null);
    }
}
