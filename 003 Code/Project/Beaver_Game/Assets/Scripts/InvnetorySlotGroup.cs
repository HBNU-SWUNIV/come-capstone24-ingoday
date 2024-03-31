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

    public void PlayerResourceCount()   // �κ��丮 �� �ڿ� ���� ���°�
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

    public bool RequireResourceCountCheck(int[] requirement)    // �κ��丮 �� �ڿ� ���ڰ� �Ǽ�, ���� �� �ʿ� �ڿ������� ������ üũ
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

    public void UseItem(int itemIndexNum, int useItemCount) // �κ��丮 �� ������ �Һ�
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
