using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InvnetorySlotGroup : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public int[] playerResourceCountInts = new int[4] { 0, 0, 0, 0 };
    public TMP_Text[] playerResourceCountTexts = new TMP_Text[4];

    public void ShowResourceText()
    {
        for (int i = 0; i < playerResourceCountInts.Length; i++)
        {
            playerResourceCountTexts[i].text = playerResourceCountInts[i].ToString();
        }
    }

    public void PlayerResourceCount()   // 인벤토리 내 자원 숫자 세는거
    {
        for (int i = 0; i < playerResourceCountInts.Length; i++)
        {
            playerResourceCountInts[i] = 0;
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].gameObject.transform.childCount != 0)
            {
                GameObject nowItemSlotChild = itemSlots[i].gameObject.transform.GetChild(0).gameObject;

                if (nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber < playerResourceCountInts.Length)
                {
                    playerResourceCountInts[nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber] = nowItemSlotChild.GetComponent<ItemCount>().count;
                }
            }
        }
        ShowResourceText();
    }

    public bool RequireResourceCountCheck(int[] requirement)    // 인벤토리 내 자원 숫자가 건설, 제작 시 필요 자원량보다 많은지 체크
    {
        bool buildable = true;
        PlayerResourceCount();
        for (int i = 0; i < playerResourceCountInts.Length; i++)
        {
            if (playerResourceCountInts[i] < requirement[i])
            {
                buildable = false;
                break;
            }
        }
        return buildable;
    }

    public void UseItem(int itemIndexNum, int useItemCount) // 인벤토리 내 아이템 소비
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            Transform itemSlotTransform = itemSlots[i].gameObject.transform;
            if (itemSlots[i].gameObject.transform.childCount != 0 && itemIndexNum == itemSlotTransform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                itemSlotTransform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(-useItemCount);

                if (itemSlotTransform.GetChild(0).gameObject.GetComponent<ItemCount>().count <= 0)
                {
                    Destroy(itemSlotTransform.GetChild(0).gameObject);
                }
                break;
            }
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
