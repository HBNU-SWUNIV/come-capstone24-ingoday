using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviourPunCallbacks
{
    public InventorySlotGroup inventorySlotGroup;   // �κ��丮
    public TowerInfo towerInfo;     // Ÿ�� ����(�Ǽ��Ҷ� ���)
    public TimerManager timerManager;   // Ÿ�̸�(Ÿ�� �Ǽ� ��)
    private TowerInfo nowTower = null;  // ���� ��ġ�� Ÿ��(����� ����)

    //public Button buildComunicationButton;  // ž �Ǽ� <-> ��� ��ư

    public ButtonIconManager btnManager;
    public GameObject towerGaugePrefab;     // Ÿ�� ��� ������
    public Transform cnavasGaugesTransform; // ��� �������� �θ� ��ġ

    public GameWinManager gameWinManager;   // �¸�(Ÿ�� ���� �� �̻� �ʵ忡 ���ÿ� ������ ���)
    public Transform towerParentTransfotm;  // Ÿ���� �θ�

    public bool spyBeaverEscape = false;   // ������ ��� ��� Ż�� ���� ����(Ư�� �ð��� �Ǿ�����)
    public bool useEmergencyEscape = false; // ������ ��� ��� Ż�� ��� ����(�̹� �� �� ����ߴ���)
    //public GameObject escapePrisonButton;   // ���� Ż�� ��ư


    [SerializeField]
    private float decreaseTime = 30.0f; // ����ž �Ǽ� �� ��� �پ��� �ð�
    private bool onTower = false;   // Ÿ�� ���� �ִ��� ����(Ÿ�� �Ǽ��� ����� ��Ȳ�� �����ϱ� ����)



    private void OnTriggerEnter2D(Collider2D collision) // Ÿ�� ���� �ִ��� Ȯ��, ���� �ִٸ� Ÿ�� ���� ��������
    {
        if (!this.GetComponent<PhotonView>().IsMine || !this.enabled)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (collision.gameObject.tag == "Tower")    // Ÿ�� ���� ������
        {
            if (onTower)    // �ϳ��� Ÿ������ ������ ����� ���� �ٸ� Ÿ���� ����� ��� �� ���� ����ִ� Ÿ���� ��� ����
            {
                nowTower.gauge.SetActive(false);

                timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 1.0f, nowTower.gameObject.GetPhotonView().ViewID);
                //timerManager.RadioComunicationTime(1.0f, nowTower);
            }

            // Ÿ�� ���� ������ �Ǽ� ��ư�� ��� ��ư���� �ٲ�, ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�
            //btnManager.buildTowerComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Radio\nComuni\ncation";
            btnManager.ChangeBuildTowerComunicationButton(true); 

            // ���� ���� Ÿ���� Ÿ�� ���� ����
            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            nowTower.gauge.SetActive(true);

            timerManager.gameObject.GetPhotonView().RPC("SetTimeSpeedRecoverTimer", RpcTarget.MasterClient, nowTower.remainComunicationTime);
            //timerManager.SetTimeSpeedRecoverTimer(nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // Ÿ������ ����� ����ϴ��� �ڵ����� ����
    {
        if (!this.GetComponent<PhotonView>().IsMine || !this.enabled)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (collision.gameObject.tag == "Tower")
        {
            //btnManager.buildTowerComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Build\nTower";    // ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�
            btnManager.ChangeBuildTowerComunicationButton(false);

            // ��� �ִ� Ÿ�� ���� �����
            onTower = false;
            nowTower.gauge.SetActive(false);

            timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 1.0f, nowTower.gameObject.GetPhotonView().ViewID);
            //timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // Ÿ�� �Ǽ� �Ǵ� ��� ��ư
    {
        if (!this.GetComponent<PhotonView>().IsMine)    // �ڽ��� ĳ���͸� �����̵���
        {
            return;
        }

        if (onTower)    // Ÿ�� ���� ������ ���
        {
            if (nowTower.remainComunicationTime > 0.0f) // Ÿ������ ���
            {
                timerManager.gameObject.GetPhotonView().RPC("RadioComunicationTime", RpcTarget.MasterClient, 2.0f, nowTower.gameObject.GetPhotonView().ViewID);
                //timerManager.RadioComunicationTime(2.0f, nowTower); // �ð� �پ��� �ӵ� ������
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // Ÿ�� ���� ��ᰡ ����ϸ� Ÿ�� �Ǽ�
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers); // ��� ���
            inventorySlotGroup.NowResourceCount();  // �κ��丮 ���� ����

            GameObject newTower = PhotonNetwork.Instantiate("TowerPrefab", this.gameObject.transform.position, Quaternion.identity);
            //GameObject newTower = GameObject.Instantiate(towerInfo.gameObject, towerParentTransfotm);   // Ÿ�� ����

            newTower.transform.position = this.transform.position;  // Ÿ�� ��ġ ����


            if (PhotonNetwork.IsMasterClient)
            {
                timerManager.TowerTime(decreaseTime);
                //timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.All, decreaseTime);
            }
            else
            {
                timerManager.timerPhotonView.RPC("TowerTime", RpcTarget.MasterClient, decreaseTime);
            }

            //timerManager.TowerTime(decreaseTime);   // Ÿ�� �Ǽ��� ���� �ð� ����


            GameObject newGauge = PhotonNetwork.Instantiate("GaugePrefab", Vector3.zero, Quaternion.identity);
            //newGauge.transform.SetParent(cnavasGaugesTransform);
            //this.GetComponent<PhotonView>().RPC("SetGaugeofTower", RpcTarget.All, newGauge);
            //newTower.GetComponent<TowerInfo>().cnavasGaugesTransform = this.cnavasGaugesTransform;
            newTower.GetComponent<PhotonView>().RPC("SetGauge", RpcTarget.All, newGauge.GetComponent<PhotonView>().ViewID);
            //GameObject newGauge = Instantiate(towerGaugePrefab, cnavasGaugesTransform); // ������ ����
            //newTower.GetComponent<TowerInfo>().gauge = newGauge;    // Ÿ���� �������� ����


            //gameWinManager.TowerCountCheck();   // Ÿ���� ���� �� �̻� ���������� Ȯ��
            gameWinManager.gameObject.GetPhotonView().RPC("TowerCountCheck", RpcTarget.All);    // Ÿ���� ���� �� �̻� ���������� Ȯ��

        }
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        timerManager = GameObject.Find("Timer").GetComponent<TimerManager>();
        //buildComunicationButton = GameObject.Find("BuildTowerButton").GetComponent<Button>();
        cnavasGaugesTransform = GameObject.Find("Gauges").transform;
        gameWinManager = GameObject.Find("GameOverManager").GetComponent<GameWinManager>();
        towerParentTransfotm = GameObject.Find("Towers").transform;
        //escapePrisonButton = GameObject.Find("EscapePrisonButton");

        btnManager = GameObject.Find("Buttons").GetComponent<ButtonIconManager>();

        btnManager.buildTowerComunicationButton.onClick.AddListener(OnClickBuildOrRadioComunicationButton);
    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f) // ������ ����� ��� Ż�� ���� üũ
        {
            spyBeaverEscape = true;
            //btnManager.escapePrisonButton.gameObject.SetActive(true);
            btnManager.escapePrisonButton.interactable = true;
        }
    }
}
