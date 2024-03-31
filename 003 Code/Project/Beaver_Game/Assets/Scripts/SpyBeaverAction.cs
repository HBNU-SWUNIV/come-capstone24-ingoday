using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyBeaverAction : MonoBehaviour
{
    public InvnetorySlotGroup invnetorySlotGroup;
    public TowerInfo towerInfo;
    public TimerManager timerManager;

    [SerializeField]
    private float decreaseTime = 30.0f;

    public void BuildTower()
    {
        invnetorySlotGroup.PlayerResourceCount();
        if (invnetorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))
        {
            for (int i = 0; i < 4; i++)
            {
                invnetorySlotGroup.UseItem(i, towerInfo.requiredResourceOfTowers[i]);
            }

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject);
            newTower.transform.position = this.transform.position;
            timerManager.TowerTime(decreaseTime);
        }
        /*
        if (playerResourceManager.playerResourceCountInts[0] >= towerInfo.requiredResourceOfTowers[0] && playerResourceManager.playerResourceCountInts[1] >= towerInfo.requiredResourceOfTowers[1] && 
            playerResourceManager.playerResourceCountInts[2] >= towerInfo.requiredResourceOfTowers[2])
        {
            for (int i = 0; i < 3; i++)
            {
                playerResourceManager.PlayerResourceCountChange(i, -towerInfo.requiredResourceOfTowers[i]);
            }

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject);
            newTower.transform.position = this.transform.position;

            timerManager.TowerTime(reduceTime);
        }
        */
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
