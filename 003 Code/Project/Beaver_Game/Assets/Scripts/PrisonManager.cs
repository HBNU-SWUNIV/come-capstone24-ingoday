using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrisonManager : MonoBehaviour
{
    private float prisonTimer = 20.0f;  // ���� Ÿ�̸�
    private bool inPrison = false;      // ���� �ȿ� �ִ��� ����
    private int caughtCount = 0;        // ������ ���� Ƚ��
    public float inPrisonTime = 30.0f;  // ���� Ÿ�̸��� �ʱ� �ð�
    public MapImages mapImage;      // Ż�� �� ����ϴ� ����
    //public bool escapePosSelect = false;
    public TMP_Text prisonTimerText;    // ���� Ÿ�̸� ǥ���� �ؽ�Ʈ
    public int keyCount = 0;    // ���� ������ �ִ� ���� ��
    public InventorySlotGroup inventorySlotGroup;   // �κ��丮(���� ����)
    public Button escapePrisonButton;   // Ż�� ��ư
    private Transform prisonTransform;  // ���� ��ġ
    //private NavMeshAgent navMeshAgent;

    public void ShowPrisonTimer()   // ���� Ÿ�̸� �����ֱ�
    {
        prisonTimerText.text = Mathf.FloorToInt(prisonTimer / 60.0f).ToString() + " : ";
        if (prisonTimer % 60.0f < 10)
            prisonTimerText.text += "0";
        prisonTimerText.text += Mathf.FloorToInt(prisonTimer % 60.0f).ToString();
    }

    [PunRPC]
    public void CaughtByRope()  // ������ ������ ��
    {
        /*
        if (navMeshAgent == null || prisonTransform == null)
            return;
        */
        //navMeshAgent.Warp(prisonTransform.position);

        if (!GetComponent<PhotonView>().IsMine)
            return;

        this.gameObject.transform.position = prisonTransform.position; // ���� �÷���� �������� ����

        //prisonTimerText.gameObject.SetActive(true);
        caughtCount++;  // ���� Ƚ�� ����
        prisonTimer = inPrisonTime + (caughtCount - 1) * 10.0f; // ���� Ƚ���� ���� ���� �ð� ����
        inPrison = true;
    }

    public void EscapePrison(bool clickButton)  // ���� Ż��
    {
        if (!inPrison)  // ���� �ȿ� �������� ���
            return;

        inPrison = false;
        if (clickButton)    // ���� Ȥ�� ������ �ɷ����� Ż�� ��
        {
            if (keyCount <= 0)  // ���谡 ���� ��Ȳ���� ������ �ɷ����� Ż��
            {
                this.gameObject.GetComponent<SpyBeaverAction>().useEmergencyEscape = true;
                //escapePrisonButton.gameObject.SetActive(false);
                escapePrisonButton.interactable = false;
                
                //Color escapeButtonColor = escapePrisonButton.GetComponent<Image>().color;
                //escapeButtonColor.a = 0.5f;
                //escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
                Debug.Log(this.gameObject.name + " �����̰� �������� Ż���߽��ϴ�.");
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.black;

                this.gameObject.GetPhotonView().RPC("SpyEmergencyEscape", RpcTarget.All, this.gameObject.GetPhotonView().ViewID);
            }
            else    // ����� Ż��
            {
                inventorySlotGroup.UseItem(5, 1, false);
            }
            keyCount--;
        }

        // �ð� �� �Ǿ Ż���ϴ� �Ͱ� ����, ������ �ɷ����� Ż���ϴ� �Ϳ� �������� ����
        mapImage.gameObject.transform.localPosition = Vector3.zero;
        //mapImage.gameObject.SetActive(true);
        //prisonTimerText.gameObject.SetActive(false);
        prisonTimerText.text = "";
    }

    [PunRPC]
    public void SpyEmergencyEscape(int ViewID)
    {
        Color spyColor = new Color(1.0f, 0.75f, 0.75f);
        PhotonView.Find(ViewID).gameObject.GetComponent<SpriteRenderer>().color = spyColor;

    }

    void Start()
    {
        if (!GetComponent<PhotonView>().IsMine)
            return;

        prisonTransform = GameObject.Find("Prison").transform;
        mapImage = GameObject.Find("MapImagePieces").GetComponent<MapImages>();
        prisonTimerText = GameObject.Find("PrisonTimer").gameObject.GetComponent<TMP_Text>();
        prisonTimerText.text = "";
        inventorySlotGroup = GameObject.Find("InventorySlots").gameObject.GetComponent<InventorySlotGroup>();
        escapePrisonButton = GameObject.Find("EscapePrisonButton").gameObject.GetComponent<Button>();
        escapePrisonButton.onClick.AddListener(() => EscapePrison(true));
        //escapePrisonButton.gameObject.SetActive(false);
        escapePrisonButton.interactable = false;
        //navMeshAgent = this.gameObject.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (inPrison)   // ���� Ÿ�̸�
        {
            prisonTimer -= Time.deltaTime;
            ShowPrisonTimer();

            if (prisonTimer <= 0.0f)    // �ð� �� �Ǹ� Ǯ����
            {
                EscapePrison(false);
            }
        }

    }
}
