using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public class DamManager : MonoBehaviour
{
    public int[] requiredResources = new int[4]; // 0: ����, 1: ����, 2: ��, 3: ��ö
    public int totalDamRequiredResource = 20;
    public InventorySlotGroup inventorySlotGroup;
    public InventorySlotGroup storageInventorySlotGroup;
    public GameObject gaugePrefab;
    private GameObject buildGauge;
    public Transform cnavasGaugesTransform;

    [SerializeField]
    private float gaugePlusYPos = 0.8f; 
    public bool buildNow = false;
    public bool buildComplete = false;

    public float obstract = 0.0f;
    public float accelerate = 0.0f;
    public GameWinManager gameWinManager;


    public void ObstructBuild() // �� �Ǽ� ����(������ ����)
    {
        obstract = -0.015f; // ���� �� ���� �ӵ�
    }

    public void AccelerateBuild() // �� �Ǽ� ����(�Ϲ� ��� ����)
    {
        accelerate = 0.01f; // ���� �� ���� �ӵ�
    }

    public void DamCreate() // ���� �κ��丮 -> â�� ���ε� â�� -> �κ��丮 ������ ����
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
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-storageInventorySlotGroup.resourceCountInts[i]); // â���� �ڿ� ����
                    remainNum[i] -= storageInventorySlotGroup.resourceCountInts[i]; // ���� �� �����ؾ��� �ڿ� ��
                }
                else   // �ʿ� �ڿ��� â�� ����� �ڿ����� ���ų� ���� ���
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-remainNum[i]);   // â���� �ڿ� ����
                    remainNum[i] = 0;
                }
            }
            storageInventorySlotGroup.NowResourceCount();

            inventorySlotGroup.UseResource(remainNum);
            inventorySlotGroup.NowResourceCount();

            buildGauge.SetActive(true);
            buildNow = true;

        }
    }

    void Start()
    {
        
        List<int> randomBoundary = new List<int>(); // �� �Ǽ��ϴµ� �ʿ��� �ڿ� �������� ���ϱ�

        for (int i = 0; i < 3; i++)
        {
            randomBoundary.Add(Random.Range(0, totalDamRequiredResource + 1));
        }
        randomBoundary.Sort();

        requiredResources[0] = randomBoundary[0];
        requiredResources[1] = randomBoundary[1] - randomBoundary[0];
        requiredResources[2] = randomBoundary[2] - randomBoundary[1];
        requiredResources[3] = totalDamRequiredResource - randomBoundary[2];


        buildGauge = Instantiate(gaugePrefab, cnavasGaugesTransform);
        buildGauge.SetActive(false);

    }

    void Update()
    {
        if (buildNow)
        {
            buildGauge.transform.position = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));
            buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount += Time.deltaTime * (0.01f + accelerate + obstract); // 100��

            if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount >= 1.0f)
            {
                buildNow = false;
                buildComplete = true;
                buildGauge.SetActive(false);

                Color damColor = gameObject.GetComponent<SpriteRenderer>().color;
                damColor.a = 150;
                gameObject.GetComponent<SpriteRenderer>().color = damColor;

                gameWinManager.DamCountCheck();
            }
            else if (buildGauge.transform.GetChild(2).gameObject.GetComponent<Image>().fillAmount <= 0.0f)
            {
                buildNow = false;
                buildGauge.SetActive(false);
            }
                
        }
        

    }
}
