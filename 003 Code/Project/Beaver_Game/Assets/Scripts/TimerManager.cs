using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField]
    private float Timer = 60.0f * 15;
    private TMP_Text timerText;


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
        Timer -= Time.deltaTime;
        ShowTimer();

    }
}
