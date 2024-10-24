using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class DamManager : MonoBehaviourPunCallbacks
{
    public Sprite[] damSprite = new Sprite[4];      // �� �̹�����
    private int degreeOfConstruction = 0;

    public int[] requiredResources = new int[4];    // �� �Ǽ��� �ʿ��� �ڿ� �� ����, 0: ����, 1: ����, 2: ��, 3: ��ö
    public int totalDamRequiredResource = 20;       // �� ����µ� �ʿ��� �� �ڿ��� ��, �� ���ڸ� 4���� �ڿ����� �������� ������.
    public InventorySlotGroup inventorySlotGroup;   // ������ �κ��丮
    public InventorySlotGroup storageInventorySlotGroup;    // â���� �κ��丮
    public GameObject gaugePrefab;          // �� �Ǽ� ������ ������
    private GameObject buildGauge;          // ���������� ������ �� �Ǽ� �������� ����
    public Transform cnavasGaugesTransform; // �������� �θ� ��ġ(UI�� �ϱ� ����)

    [SerializeField]
    private float gaugePlusYPos = 0.8f; // ������ ��ġ(�� �߽����κ��� ���� ���ʿ� ���⵵��)
    public bool buildNow = false;       // �� �Ǽ� ������ ����
    public bool buildComplete = false;  // �� �Ǽ� �Ϸ��ߴ��� ����

    public float obstract = 0.0f;   // ������ ����� ���� �ӵ�
    public float accelerate = 0.0f; // �Ϲ� ����� ���� �ӵ�
    public GameWinManager gameWinManager;   // �� ��� �Ǽ� �� �¸��ϰ� �ϵ���

    public float dameGaugeSpeedRate = 1.0f;    // �� ���������� �ӵ� ����, �۾��� �������� ����


    [PunRPC]
    public void ObstructBuild(bool turnOn) // �� �Ǽ� ����(������ ����)
    {
        if (turnOn)
        {
            obstract = -0.02f * dameGaugeSpeedRate; // ���� �� ���� �ӵ�
        }
        else
        {
            obstract = 0.0f;
        }
    }

    [PunRPC]
    public void AccelerateBuild(bool turnOn) // �� �Ǽ� ����(�Ϲ� ��� ����)
    {
        if (turnOn)
        {
            accelerate = 0.01f * dameGaugeSpeedRate; // ���� �� ���� �ӵ�
            GetComponent<AudioSource>().Play();
        }
        else
        {
            accelerate = 0.0f;
            GetComponent<AudioSource>().Stop();
        }
    }

    public void DamCreate() // â�� -> �κ��丮 ������ �ڿ� �Һ��Ͽ� �� �Ǽ�
    {
        bool damCreateBool = true;
        
        for (int i = 0; i < 4; i++) // ���� ���� �ڿ��� ������� üũ
        {
            if (requiredResources[i] > inventorySlotGroup.resourceCountInts[i] + storageInventorySlotGroup.resourceCountInts[i])
            {
                damCreateBool = false;
                break;
            }
        }

        if (damCreateBool)
        {
            int[] remainNum = requiredResources;    // ���� ����µ� �ʿ��� �ڿ�

            for (int i = 0; i < 4; i++) // â���� �ڿ� ���� �κ�
            {
                if (requiredResources[i] > storageInventorySlotGroup.resourceCountInts[i])  // �ʿ� �ڿ��� â�� ����� �ڿ����� ���� ���
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetPhotonView().RPC("ShowItemCount", RpcTarget.All, -storageInventorySlotGroup.resourceCountInts[i]);
                    //storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-storageInventorySlotGroup.resourceCountInts[i]); // â���� �ڿ� ����
                    remainNum[i] -= storageInventorySlotGroup.resourceCountInts[i]; // ���� �� �����ؾ��� �ڿ� ��
                }
                else   // �ʿ� �ڿ��� â�� ����� �ڿ����� ���ų� ���� ���
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetPhotonView().RPC("ShowItemCount", RpcTarget.All, -remainNum[i]);
                    //storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-remainNum[i]);   // â���� �ڿ� ����
                    remainNum[i] = 0;
                }
            }
            //storageInventorySlotGroup.NowResourceCount();   // â���� �ڿ� �� ����
            storageInventorySlotGroup.gameObject.GetPhotonView().RPC("NowResourceCount", RpcTarget.All);

            inventorySlotGroup.UseResource(remainNum);  // �κ��丮�� �ڿ� ���
            inventorySlotGroup.NowResourceCount();  // �κ��丮�� �ڿ� �� ����

            this.gameObject.GetPhotonView().RPC("TurnOnBuildGauge", RpcTarget.All);
            //buildGauge.SetActive(true); // �Ǽ� ������ Ȱ��ȭ
            //buildNow = true;

        }
    }

    [PunRPC]
    public void TurnOnBuildGauge()  // ������ �� �Ǽ��� �ϸ� �Ǽ� ������ Ȱ��ȭ, �Ǽ��� ���·� ����
    {
        buildGauge.SetActive(true);
        buildNow = true;
    }


    [PunRPC]
    public void SetDamCreateResource(int randomBoundary0, int randomBoundary1, int randomBoundary2)
    {
        requiredResources[0] = randomBoundary0;
        requiredResources[1] = randomBoundary1 - randomBoundary0;
        requiredResources[2] = randomBoundary2 - randomBoundary1;
        requiredResources[3] = totalDamRequiredResource - randomBoundary2;
    }

    /*
    public override void OnJoinedRoom()
    {
        
        //if (!PhotonNetwork.IsMasterClient)  // ���߿� ������������ �ٲٸ� �׶� �̰� �ѱ�
        //    return;
        
        List<int> randomBoundary = new List<int>(); // �� �Ǽ��ϴµ� �ʿ��� �ڿ� �������� ���ϱ�

        for (int i = 0; i < 3; i++)
        {
            randomBoundary.Add(Random.Range(0, totalDamRequiredResource + 1));
        }
        randomBoundary.Sort();

        this.gameObject.GetPhotonView().RPC("SetDamCreateResource", RpcTarget.All, randomBoundary[0], randomBoundary[1], randomBoundary[2]);
    }
    */

    public void DamRandomPrice()
    {

        List<int> randomBoundary = new List<int>(); // �� �Ǽ��ϴµ� �ʿ��� �ڿ� �������� ���ϱ�

        for (int i = 0; i < 3; i++)
        {
            randomBoundary.Add(Random.Range(0, totalDamRequiredResource + 1));
        }
        randomBoundary.Sort();

        this.gameObject.GetPhotonView().RPC("SetDamCreateResource", RpcTarget.All, randomBoundary[0], randomBoundary[1], randomBoundary[2]);
    }


    void Start()
    {
        buildGauge = Instantiate(gaugePrefab, cnavasGaugesTransform);
        buildGauge.SetActive(false);
        
        /*
        requiredResources[0] = randomBoundary[0];
        requiredResources[1] = randomBoundary[1] - randomBoundary[0];
        requiredResources[2] = randomBoundary[2] - randomBoundary[1];
        requiredResources[3] = totalDamRequiredResource - randomBoundary[2];
        */
    }

    void Update()
    {
        if (buildNow)
        {
            buildGauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));    // ������ ��ġ ����(UI��)
            buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount += Time.deltaTime * (0.01f + accelerate + obstract); // ����� 100��, ��ġ ���� ����


            float constructFillAmount = buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount;
            int constructDegree = (Mathf.FloorToInt(constructFillAmount * 100.0f) + 50) / 50;


            if (constructDegree != degreeOfConstruction)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[constructDegree];
                degreeOfConstruction = constructDegree;
            }

            /*
            if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount >= 1.0f)// �������� �� á�� ���
            {

                // ������ ��Ȱ��ȭ
                buildNow = false;
                buildComplete = true;
                buildGauge.SetActive(false);

                // �� �ϼ� �̹����� �ٲٱ�, ����� �� �ٲٴ� �ɷ� ����, int�� ���� �Ǽ� ���൵�� �����ϰ� ���� Amount�� ���൵(int)�� ��ġ���� ������ �̹��� ������Ʈ �ϱ�
                Color damColor = gameObject.GetComponent<SpriteRenderer>().color;
                damColor.a = 150;
                gameObject.GetComponent<SpriteRenderer>().color = damColor;

                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[3];


                gameWinManager.DamCountCheck();
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount >= 0.5f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[2];
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount > 0.0f)
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[1];
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount <= 0.0f)  // ���� ���ؿ� ���� �������� �� �������� ��� �Ǽ� ���
            {
                
                buildNow = false;
                buildGauge.SetActive(false);
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[0];
            }
            */




            if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount >= 1.0f)// �������� �� á�� ���
            {
                
                // ������ ��Ȱ��ȭ
                buildNow = false;
                buildComplete = true;
                buildGauge.SetActive(false);
                /*
                // �� �ϼ� �̹����� �ٲٱ�, ����� �� �ٲٴ� �ɷ� ����, int�� ���� �Ǽ� ���൵�� �����ϰ� ���� Amount�� ���൵(int)�� ��ġ���� ������ �̹��� ������Ʈ �ϱ�
                Color damColor = gameObject.GetComponent<SpriteRenderer>().color;
                damColor.a = 150;
                gameObject.GetComponent<SpriteRenderer>().color = damColor;
                */
                gameWinManager.DamCountCheck();
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount <= 0.0f)  // ���� ���ؿ� ���� �������� �� �������� ��� �Ǽ� ���
            {
                buildNow = false;
                buildGauge.SetActive(false);
                degreeOfConstruction = 0;
                this.gameObject.GetComponent<SpriteRenderer>().sprite = damSprite[degreeOfConstruction];
            }
                
        }
        

    }
}
