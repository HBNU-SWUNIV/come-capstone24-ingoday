using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBeaverAction : MonoBehaviour
{
    public InventorySlotGroup inventorySlotGroup;
    public TowerInfo towerInfo;
    public TimerManager timerManager;
    private TowerInfo nowTower = null;

    public Button buildComunicationButton;
    public GameObject towerGaugePrefab;
    public Transform cnavasGaugesTransform;

    public GameWinManager gameWinManager;
    public Transform towerParentTransfotm;

    private bool spyBeaverEscape = false;
    public bool useEmergencyEscape = false;
    public GameObject escapePrisonButton;


    [SerializeField]
    private float decreaseTime = 30.0f;

    private bool onTower = false;

    // ���߿� Ÿ�� ö�� ���� �ڵ忡 �ִ� Ʈ���ſ� �� Ʈ���Ÿ� �� ���� ���ļ� Ÿ�� �����ϱ�
    private void OnTriggerEnter2D(Collider2D collision) // Ÿ�� ���� �ִ��� Ȯ��, ���� �ִٸ� Ÿ�� ���� ��������
    {
        if (collision.gameObject.tag == "Tower")
        {
            if (onTower)    // �ϳ��� Ÿ������ ������ ����� ���� �ٸ� Ÿ���� ����� ���
            {
                nowTower.gauge.SetActive(false);
                timerManager.RadioComunicationTime(1.0f, nowTower);
            }

            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Radio\nComuni\ncation";   // ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�

            onTower = true;
            nowTower = collision.gameObject.GetComponent<TowerInfo>();
            nowTower.gauge.SetActive(true);
            timerManager.SetTimeSpeedRecoverTimer(nowTower.remainComunicationTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)  // Ÿ������ ����� ����ϴ��� �ڵ����� ����
    {
        if (collision.gameObject.tag == "Tower")
        {
            buildComunicationButton.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = "Build\nTower";    // ���߿� text�� �ٲٴ� ��� ���ڰ� ���� �̹����� button���� �ٲ�� �ϱ�

            onTower = false;
            nowTower.gauge.SetActive(false);
            timerManager.RadioComunicationTime(1.0f, nowTower);
        }
    }

    public void OnClickBuildOrRadioComunicationButton()    // Ÿ�� �Ǽ� �Ǵ� ��� ��ư
    {
        if (onTower)
        {
            if (nowTower.remainComunicationTime > 0.0f) // Ÿ������ ���
            {
                timerManager.RadioComunicationTime(2.0f, nowTower);
            }
        }
        else if (inventorySlotGroup.RequireResourceCountCheck(towerInfo.requiredResourceOfTowers))  // Ÿ�� ���� ��ᰡ ����ϸ� �Ǽ�
        {
            inventorySlotGroup.UseResource(towerInfo.requiredResourceOfTowers);
            inventorySlotGroup.NowResourceCount();

            GameObject newTower = GameObject.Instantiate(towerInfo.gameObject, towerParentTransfotm);
            newTower.transform.position = this.transform.position;
            timerManager.TowerTime(decreaseTime);

            GameObject newGauge = Instantiate(towerGaugePrefab, cnavasGaugesTransform);
            newTower.GetComponent<TowerInfo>().gauge = newGauge;

            gameWinManager.TowerCountCheck();
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (!spyBeaverEscape && !useEmergencyEscape && timerManager.GetNowTime() <= 120.0f)
        {
            spyBeaverEscape = true;
            escapePrisonButton.SetActive(true);
        }
    }
}
