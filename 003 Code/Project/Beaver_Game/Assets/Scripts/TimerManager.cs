using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float timer = 60.0f * 15.0f;   // ���� Ÿ�̸�, 15��
    private TMP_Text timerText; // Ÿ�̸� �ؽ�Ʈ
    private float timeSpeed = 1.0f;    // Ÿ�̸� �ӵ�(�������� ��ſ� ���� ��ȭ)
    private bool basicTimeSpeedBool = true; // Ÿ�̸� �ӵ� ��ȭ�� ���(�������� ��ſ� ���� ��ȭ)
    private float timeSpeedRecoverTimer = 20.0f;    // ���� ��� �ð�
    private TowerInfo nowTower; // ���� ��ġ�� ����ž�� ����(��� �ð� ����)
    public GameWinManager gameWinManager;   // �ð� �� �Ǹ� ���� ����

    public PhotonView timerPhotonView;

    public float GetNowTime()   // ���� �ð� ����
    {
        return timer;
    }

    [PunRPC]
    public void SetTimeSpeedRecoverTimer(float towerTime)   // ����ž�� ���� ��� �ð� ���
    {
        timeSpeedRecoverTimer = towerTime;
    }

    [PunRPC]
    public void RadioComunicationTime(float speed, int towerViweID) // ����� ���� Ÿ�̸� ����
    {
        TowerInfo tower = PhotonView.Find(towerViweID).gameObject.GetComponent<TowerInfo>();
        timeSpeed = speed;
        if (timeSpeed != 1.0f)  // ����� �������̾��ٸ� ��� ���߱�
        {
            nowTower = tower;
            basicTimeSpeedBool = false;
            timeSpeedRecoverTimer = tower.remainComunicationTime;   // �ش� Ÿ���� ���� ��� �ð� ���� (Ÿ�̸� -> Ÿ��)
        }
        else    // ��� ���� �ƴϾ��ٸ� ��� ����
        {
            basicTimeSpeedBool = true;
            tower.remainComunicationTime = timeSpeedRecoverTimer;   // �ش� Ÿ���� ���� ��� �ð��� �� Ÿ�̸ӿ� ��� (Ÿ�� -> Ÿ�̸�)
        }
        
    }

    [PunRPC]
    public void TowerTime(float addTime)    // Ÿ�̸ӿ� ���� �ð� ��ȭ(����ž �Ǽ� �� ����, ö�� �� ȸ��)
    {
        timer -= addTime;
        timerPhotonView.RPC("ShowTimer", RpcTarget.All, timer);
        //ShowTimer(timer);
    }

    [PunRPC]
    public void ShowTimer(float timer) // Ÿ�̸� �ؽ�Ʈ�� ������
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
        timerText = this.GetComponent<TMP_Text>();
        timerPhotonView = this.GetComponent<PhotonView>();
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        timer -= timeSpeed * Time.deltaTime;    // Ÿ�̸� �ð� �帧
        //ShowTimer();    // Ÿ�̸� �ؽ�Ʈ�� �����ֱ�
        timerPhotonView.RPC("ShowTimer", RpcTarget.All, timer);

        if (timer <= 0) // �ð� �� �Ǹ� ���� ����
        {
            timer = 0.0f;
            ShowTimer(timer);
            gameWinManager.TimeCheck();
        }

        if (!basicTimeSpeedBool && nowTower.remainComunicationTime >= 0.0f) // ��� ���� ���
        {
            nowTower.gameObject.GetPhotonView().RPC("UpdateFillAmountofGauge", RpcTarget.All, timeSpeedRecoverTimer);
            //nowTower.gauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount = 1 - timeSpeedRecoverTimer / 20.0f; // ��� ������, ��ġ�� ���� �ö�, �ִ밡 1.0, �ּ� 0.0

            timeSpeedRecoverTimer -= Time.deltaTime;    // ����ž�� ���� ��� ���� �ð�

            if (timeSpeedRecoverTimer <= 0.0f)  // ��� ���̾��ٰ� �ش� Ÿ���� ��� ���� �ð��� �� �Ǹ� ��� ����
            {
                RadioComunicationTime(1.0f, nowTower.gameObject.GetPhotonView().ViewID);
            }
        }

    }
}
