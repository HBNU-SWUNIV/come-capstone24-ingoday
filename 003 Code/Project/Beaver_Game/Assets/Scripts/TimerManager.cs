using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private float Timer = 60.0f * 15;
    private TMP_Text timerText;
    private float timeSpeed = 1;
    private bool basicTimeSpeedBool = true;
    private float timeSpeedRecoverTimer = 20.0f;
    private TowerInfo nowTower;


    public void SetTimeSpeedRecoverTimer(float towerTime)
    {
        timeSpeedRecoverTimer = towerTime;
    }

    public void RadioComunicationTime(float speed, TowerInfo tower)
    {
        timeSpeed = speed;
        if (timeSpeed != 1.0f)
        {
            nowTower = tower;
            basicTimeSpeedBool = false;
            timeSpeedRecoverTimer = tower.remainComunicationTime;
        }
        else
        {
            basicTimeSpeedBool = true;
            tower.remainComunicationTime = timeSpeedRecoverTimer;
        }
        
    }

    public void TowerTime(float addTime)
    {
        Timer -= addTime;
        ShowTimer();
    }

    public void ShowTimer()
    {
        timerText.text = Mathf.FloorToInt(Timer / 60.0f).ToString() + " : ";
        if (Timer % 60.0f < 10)
            timerText.text += "0";
        timerText.text += Mathf.FloorToInt(Timer % 60.0f).ToString();
    }

    void Start()
    {
        timerText = this.GetComponent<TMP_Text>();
    }

    void Update()
    {
        Timer -= timeSpeed * Time.deltaTime;
        ShowTimer();
        if (!basicTimeSpeedBool && nowTower.remainComunicationTime >= 0.0f)
        {
            Debug.Log(timeSpeedRecoverTimer);
            timeSpeedRecoverTimer -= Time.deltaTime;

            if (timeSpeedRecoverTimer <= 0.0f)
            {
                RadioComunicationTime(1.0f, nowTower);
            }
        }
    }
}
