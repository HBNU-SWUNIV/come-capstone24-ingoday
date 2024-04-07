using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyBeaverAction : MonoBehaviour
{
    public InventorySlotGroup inventorySlotGroup;
    public TowerInfo towerInfo;
    public TimerManager timerManager;
    private TowerInfo nowTower = null;

    [SerializeField]
    private float decreaseTime = 30.0f;

    private bool onTower = false;

    // 나중에 타워 철거 관련 코드에 있는 트리거와 이 트리거를 한 곳에 합쳐서 타워 관리하기
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            timerManager.SetTimeSpeedRecoverTimer(nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tower")
        {
            onTower = false;
            timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickRadioComunicationButton()
    {
        if (onTower && nowTower.remainComunicationTime > 0.0f)
        {
            timerManager.RadioComunicationTime(2.0f, nowTower);
        }
    }

    public void BuildTower()
    {
        
        if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))
        {
            /*
            for (int i = 0; i < 4; i++)
            {
                inventorySlotGroup.UseItem(i, towerInfo.requiredResourceOfTowers[i]);
            }
            */
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers);
            inventorySlotGroup.NowResourceCount();

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject);
            newTower.transform.position = this.transform.position;
            timerManager.TowerTime(decreaseTime);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
