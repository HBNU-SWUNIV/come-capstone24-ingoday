using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class TimerManager : MonoBehaviourPunCallbacks
{
    public float timeSetting;
    private float timer = 60.0f * 1.0f + 5.0f;   // 게임 타이머, 15분
    private TMP_Text timerText; // 타이머 텍스트
    private float timeSpeed = 1.0f;    // 타이머 속도(스파이의 통신에 의해 변화)
    private bool basicTimeSpeedBool = true; // 타이머 속도 변화에 사용(스파이의 통신에 의해 변화)
    private float timeSpeedRecoverTimer = 20.0f;    // 남은 통신 시간
    private TowerInfo nowTower; // 현재 위치한 전파탑의 정보(통신 시간 측정)
    public GameWinManager gameWinManager;   // 시간 다 되면 게임 종료

    public PhotonView timerPhotonView;

    public GameObject nightLight;
    public Light2D globalLight;
    private Color nightColor;
    bool isNight = false;
    public GetResourceScrollbar resourceScrollbar;

    private bool timerOn = false;

    public AudioClip nightBGM;
    public AudioSource cameraAudio;

    public void SetTimerOn()
    {
        timerOn = true;
    }

    [PunRPC]
    public void SetNight()
    {
        globalLight.color = nightColor;
        nightLight.SetActive(true);
        nightLight.GetComponent<Light2D>().intensity = 1.5f;
        resourceScrollbar.scrollSpeed = 1.0f;
        cameraAudio.clip = nightBGM;
        cameraAudio.Play();
    }

    public float GetNowTime()   // 현재 시간 정보
    {
        return timer;
    }

    [PunRPC]
    public void SetTimeSpeedRecoverTimer(float towerTime)   // 전파탑의 남은 통신 시간 등록
    {
        timeSpeedRecoverTimer = towerTime;
    }

    [PunRPC]
    public void RadioComunicationTime(float speed, int towerViweID) // 통신을 통해 타이머 가속
    {
        TowerInfo tower = PhotonView.Find(towerViweID).gameObject.GetComponent<TowerInfo>();
        timeSpeed = speed;
        if (timeSpeed != 1.0f)  // 통신이 진행중이었다면 통신 멈추기
        {
            nowTower = tower;
            basicTimeSpeedBool = false;
            timeSpeedRecoverTimer = tower.remainComunicationTime;   // 해당 타워에 남은 통신 시간 전송 (타이머 -> 타워)
        }
        else    // 통신 중이 아니었다면 통신 진행
        {
            basicTimeSpeedBool = true;
            tower.remainComunicationTime = timeSpeedRecoverTimer;   // 해당 타워의 남은 통신 시간을 이 타이머에 등록 (타워 -> 타이머)
        }
        
    }

    [PunRPC]
    public void TowerTime(float addTime)    // 타이머에 남은 시간 변화(전파탑 건설 시 감소, 철거 시 회복)
    {
        timer -= addTime;
        timerPhotonView.RPC("ShowTimer", RpcTarget.All, timer);
        //ShowTimer(timer);
    }

    [PunRPC]
    public void ShowTimer(float timer) // 타이머 텍스트로 보여줌
    {
        if (!PhotonNetwork.IsMasterClient)
            this.timer = timer;
        timerText.text = Mathf.FloorToInt(timer / 60.0f).ToString() + " : ";
        if (timer % 60.0f < 10)
            timerText.text += "0";
        timerText.text += Mathf.FloorToInt(timer % 60.0f).ToString();
    }

    void Start()
    {
        timer = timeSetting;
        timerText = this.GetComponent<TMP_Text>();
        timerPhotonView = this.GetComponent<PhotonView>();
        nightColor = new Color(0.03125f, 0.03125f, 0.03125f);
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient || !timerOn)
            return;

        timer -= timeSpeed * Time.deltaTime;    // 타이머 시간 흐름
        //ShowTimer();    // 타이머 텍스트로 보여주기
        timerPhotonView.RPC("ShowTimer", RpcTarget.All, timer);

        if (timer <= 0) // 시간 다 되면 게임 종료
        {
            timerOn = false;
            timer = 0.0f;
            ShowTimer(timer);

            //gameWinManager.TimeCheck();
            gameWinManager.gameObject.GetPhotonView().RPC("TimeCheck", RpcTarget.All);
        }
        else if (timer <= timeSetting / 3.0f && !isNight)
        {
            isNight = true;
            timerPhotonView.RPC("SetNight", RpcTarget.All);
            //SetNight();
        }

        if (!basicTimeSpeedBool && nowTower.remainComunicationTime >= 0.0f) // 통신 중일 경우
        {
            nowTower.gameObject.GetPhotonView().RPC("UpdateFillAmountofGauge", RpcTarget.All, timeSpeedRecoverTimer);
            //nowTower.gauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = 1 - timeSpeedRecoverTimer / 20.0f; // 통신 게이지, 수치가 점차 올라감, 최대가 1.0, 최소 0.0

            timeSpeedRecoverTimer -= Time.deltaTime;    // 전파탑의 남은 통신 제한 시감

            if (timeSpeedRecoverTimer <= 0.0f)  // 통신 중이었다가 해당 타워의 통신 제한 시간이 다 되면 통신 종료
            {
                RadioComunicationTime(1.0f, nowTower.gameObject.GetPhotonView().ViewID);
            }
        }

    }
}
