using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class DamManager : MonoBehaviour
{
    public int[] requiredResources = new int[4]; // 0: 나무, 1: 진흙, 2: 돌, 3: 강철
    public int totalDamRequiredResource = 20;
    public InventorySlotGroup inventorySlotGroup;
    public InventorySlotGroup storageInventorySlotGroup;
    public GameObject gaugePrefab;
    private GameObject buildGauge;
    public Transform cnavasGaugesTransform;

    [SerializeField]
    private float gaugePlusYPos = 0.8f; 


    public void DamCreate() // 현재 인벤토리 -> 창고 순인데 창고 -> 인벤토리 순으로 변경
    {
        bool damCreateBool = true;
        
        for (int i = 0; i < 4; i++) // 댐을 만들 자원이 충분한지 체크
        {
            if (requiredResources[i] > inventorySlotGroup.resourceCountInts[i] + storageInventorySlotGroup.resourceCountInts[i])
            {
                damCreateBool = false;
                break;
            }
        }

        if (damCreateBool)
        {
            int[] remainNum = requiredResources;    // 댐을 만드는데 필요한 자원

            for (int i = 0; i < 4; i++) // 창고에서 자원 빼는 부분
            {
                if (requiredResources[i] > storageInventorySlotGroup.resourceCountInts[i])  // 필요 자원이 창고에 저장된 자원보다 많을 경우
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-storageInventorySlotGroup.resourceCountInts[i]); // 창고에서 자원 빼기
                    remainNum[i] -= storageInventorySlotGroup.resourceCountInts[i]; // 남은 더 지불해야할 자원 수
                }
                else   // 필요 자원이 창고에 저장된 자원보다 적거나 같을 경우
                {
                    storageInventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-remainNum[i]);   // 창고에서 자원 빼기
                    remainNum[i] = 0;
                }
            }
            storageInventorySlotGroup.NowResourceCount();

            inventorySlotGroup.UseResource(remainNum);

            /*
            for (int i = 0; i < 4; i++)
            {
                if (requiredResources[i] > inventorySlotGroup.resourceCountInts[i])
                {
                    remainNum[i] = requiredResources[i] - inventorySlotGroup.resourceCountInts[i];
                }

                inventorySlotGroup.UseItem(i, requiredResources[i] - remainNum[i]);
                requiredResources[i] = 0;
            }
            */

            Color damColor =  gameObject.GetComponent<SpriteRenderer>().color;
            damColor.a = 150;
            gameObject.GetComponent<SpriteRenderer>().color = damColor;

            inventorySlotGroup.NowResourceCount();
        }

    }

    void Start()
    {
        
        List<int> randomBoundary = new List<int>(); // 댐 건설하는데 필요한 자원 랜덤으로 정하기

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

    }

    void Update()
    {
        Vector3 gaugePos = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + gaugePlusYPos, 0.0f));
        buildGauge.transform.position = gaugePos;

    }
}
