using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotGroup : MonoBehaviour
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();
    public int[] resourceCountInts = new int[4] { 0, 0, 0, 0 };
    public TMP_Text[] resourceCountTexts = new TMP_Text[4];
    public Button throwRopeButton = null;

    public void ShowResourceText()
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountTexts[i].text = resourceCountInts[i].ToString();
        }
    }

    public void NowResourceCount()   // �κ��丮 �� �ڿ� ���� ���°�
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountInts[i] = 0;
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (itemSlots[i].gameObject.transform.childCount != 0)
            {
                GameObject nowItemSlotChild = itemSlots[i].gameObject.transform.GetChild(0).gameObject;

                if (nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber < resourceCountInts.Length)
                {
                    resourceCountInts[nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber] += nowItemSlotChild.GetComponent<ItemCount>().count;
                }
            }
        }
        ShowResourceText();
    }

    public bool RequireResourceCountCheck(int[] requirement)    // �κ��丮 �� �ڿ� ���ڰ� �Ǽ�, ���� �� �ʿ� �ڿ������� ������ üũ
    {
        bool buildable = true;
        NowResourceCount();
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            if (resourceCountInts[i] < requirement[i])
            {
                buildable = false;
                break;
            }
        }
        return buildable;
    }

    public void UseItem(int itemIndexNum, int useItemCount) // �ϳ��� ������ �κ��丮 ��ü�� ���켭 ���� ���
    {
        int remainResourceCount = useItemCount;
        int ropeIndexNum = 10;  // ���� ������ �ε��� �ѹ��� �ٲ�� �� ���� �ٲ��ֱ�
        bool haveRope = false;

        Debug.Log("00000");

        for (int i = 0; i < itemSlots.Count; i++)
        {
            // ���� üũ�ϴ� ������ �������� ����� �������� ���
            if (itemSlots[i].gameObject.transform.childCount != 0 && itemIndexNum == itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItem = itemSlots[i].gameObject.transform.GetChild(0).gameObject;

                Debug.Log("000");

                if (childItem.GetComponent<ItemCount>().count < remainResourceCount)    // ���� ������ ������ ���� ����� ������ ������ ������
                {
                    Debug.Log("111");

                    remainResourceCount -= childItem.GetComponent<ItemCount>().count;
                    Destroy(childItem);
                }
                else    // ���� ������ ������ ���� ����� ������ ������ ���ų� ������
                {
                    Debug.Log("222");

                    childItem.GetComponent<ItemCount>().ShowItemCount(-remainResourceCount);
                    remainResourceCount = 0;

                    if (childItem.GetComponent<ItemCount>().count <= 0) // ������ ������ ���� ����� ������ ���� ���� ��쿡�� �κ��丮���� ����
                    {
                        Debug.Log("333");

                        Destroy(childItem);
                    }
                    else if (itemIndexNum == ropeIndexNum)
                    {
                        Debug.Log("444");

                        haveRope = true;
                    }
                    
                }
            }
        }

        if (throwRopeButton != null && itemIndexNum == ropeIndexNum && !haveRope)  // ��ư ��Ȱ��ȭ
        {
            throwRopeButton.gameObject.SetActive(false);
        }

    }

    public void UseResource(int[] useResourceCount) // �κ��丮 ��ü�� ���Ǹ鼭 4���� �ڿ��̶�� ����ϴ� ���
    {
        int[] remainResource = new int[4];
        
        for (int i = 0; i < 4; i++)
        {
            remainResource[i] = useResourceCount[i];
        }

        for (int i = 0; i < itemSlots.Count; i++)
        {
            // �ش� ���Կ� ������(�ڽ�)�� �ְ� �� �������� �ڿ��� ���(������ ���� ��ȣ 4 �̸�)
            if (itemSlots[i].gameObject.transform.childCount != 0 && 4 > itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItemObj = itemSlots[i].gameObject.transform.GetChild(0).gameObject; // �ش� ���Կ� �ִ� ������
                int itemNum = childItemObj.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();    // �ش� ���Կ� �ִ� �������� ���� ��ȣ

                if (remainResource[itemNum] <= 0)   // �ش� �ڿ��� �� ������ �ʿ䰡 ���ٸ� �ѱ��
                    continue;

                if (childItemObj.GetComponent<ItemCount>().count <= remainResource[itemNum]) // �����ؾ��� �ڿ��� ���� ���Կ� �ִ� ������ ������ ���ų� ���� ���
                {
                    remainResource[itemNum] -= childItemObj.GetComponent<ItemCount>().count;
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-childItemObj.GetComponent<ItemCount>().count);

                    if (!itemSlots[i].gameObject.GetComponent<ItemSlot>().storageSlot) // â������ �ƴϸ� -> �÷��̾� �κ��丮 �����̶�� ������ ����
                    {
                        Destroy(childItemObj);
                    }
                }
                else    // �����ؾ��� �ڿ��� ���� ���Կ� �ִ� ������ ������ ���� ���
                {
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-remainResource[itemNum]);
                    remainResource[itemNum] = 0;

                }

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
