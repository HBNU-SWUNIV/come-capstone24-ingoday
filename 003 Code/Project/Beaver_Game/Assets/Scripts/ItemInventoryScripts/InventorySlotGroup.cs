using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotGroup : MonoBehaviourPunCallbacks
{
    public List<ItemSlot> itemSlots = new List<ItemSlot>();     // ������ ����
    public int[] resourceCountInts = new int[4] { 0, 0, 0, 0 }; // �⺻ �ڿ�(����, ����, ��, ö)�� ��
    public TMP_Text[] resourceCountTexts = new TMP_Text[4];     // �⺻ �ڿ��� ���� ���� �ؽ�Ʈ(UI �»��)
    public Button throwRopeButton = null;   // ���� ������ ��ư
    public Button escapePrisonButton = null;    // ���� Ż�� ��ư
    public SpyBoolManager spyBoolManager = null;    // ������ ����

    public void ShowResourceText()  // �⺻ �ڿ� ���� �ؽ�Ʈ�� ���
    {
        for (int i = 0; i < resourceCountInts.Length; i++)
        {
            resourceCountTexts[i].text = resourceCountInts[i].ToString();
        }
    }

    public void StorageResourceCount()
    {
        photonView.RPC("NowResourceCount", RpcTarget.All);
    }

    [PunRPC]
    public void NowResourceCount()   // �κ��丮 �� �ڿ� ���� ����
    {
        for (int i = 0; i < resourceCountInts.Length; i++)  // �⺻ �ڿ� ���� ���� 0���� �ʱ�ȭ
        {
            resourceCountInts[i] = 0;
        }

        for (int i = 0; i < itemSlots.Count; i++)   // ���Ե��� �ϳ��� üũ
        {
            if (itemSlots[i].gameObject.transform.childCount != 0)  // ���Կ� �������� ������ ���
            {
                GameObject nowItemSlotChild = itemSlots[i].gameObject.transform.GetChild(0).gameObject; // ���� üũ�ϴ� ������ ������

                if (nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber < resourceCountInts.Length)   // �� ������ �������� �⺻ �ڿ��� ��� �� ���ϱ�
                {
                    resourceCountInts[nowItemSlotChild.GetComponent<ItemDrag>().itemIndexNumber] += nowItemSlotChild.GetComponent<ItemCount>().count;
                }
            }
        }
        ShowResourceText(); // �ڿ� �� �ؽ�Ʈ�� ���
    }

    public bool RequireResourceCountCheck(int[] requirement)    // �κ��丮 �� �ڿ� ���ڰ� �Ǽ�, ���� �� �ʿ� �ڿ������� ������ üũ
    {
        bool buildable = true;
        NowResourceCount(); // ���� �ڿ� �� üũ
        for (int i = 0; i < resourceCountInts.Length; i++)  // ���� �ڿ� ���� �ʿ� �ڿ� ������ ������ �Ǽ� �Ұ�
        {
            if (resourceCountInts[i] < requirement[i])
            {
                buildable = false;
                break;
            }
        }
        return buildable;
    }

    public void UseItem(int itemIndexNum, int useItemCount, bool keepItem) // �ϳ��� ������ �κ��丮 ��ü�� ���켭 ���� ���(�ڿ� �̿��� ������ ���� ���� ����Ҷ� ����)
    {
        int remainResourceCount = useItemCount; // ����� ������ ��
        int ropeIndexNum = 4;   // ���� ������ �ε��� �ѹ��� �ٲ�� �� ���� �ٲ��ֱ�
        int keyIndexNum = 5;    // ���� Ż��Ű�� �ε��� �ѹ��� �ٲ�� �� ���� �ٲ��ֱ�
        bool haveRope = false;  // ���� ������ �ִ���
        bool haveKey = false;   // ���� ������ �ִ���


        for (int i = 0; i < itemSlots.Count; i++)   // ���Ե��� �ϳ��� üũ
        {
            // ���� üũ�ϴ� ������ �������� ����� �������� ���
            if (itemSlots[i].gameObject.transform.childCount != 0 && itemIndexNum == itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
            {
                GameObject childItem = itemSlots[i].gameObject.transform.GetChild(0).gameObject;    // ���� ������ ������
                if (childItem.GetComponent<ItemCount>().count < remainResourceCount)    // ���� ������ ������ ���� ����� ������ ������ ������
                {
                    remainResourceCount -= childItem.GetComponent<ItemCount>().count;   // ����� ������ ���� ���� ������ ������ ����ŭ ����
                    Destroy(childItem); // ���� ������ ������ ����(��� ���)
                }
                else    // ���� ������ ������ ���� ����� ������ ������ ���ų� ������
                {
                    childItem.GetComponent<ItemCount>().ShowItemCount(-remainResourceCount);    // ���� ������ ������ ���� ����� ������ ����ŭ ����
                    remainResourceCount = 0;

                    if (childItem.GetComponent<ItemCount>().count <= 0) // ������ ������ ���� ����� ������ ���� ���� ��쿡�� �κ��丮���� ����
                    {
                        Destroy(childItem);
                    }
                    else if (itemIndexNum == ropeIndexNum)  // ���� ����
                    {
                        haveRope = true;
                    }
                    else if (itemIndexNum == keyIndexNum)   // ���� ����
                    {
                        haveKey = true;
                    }

                }
            }
        }

        

        if (throwRopeButton != null && itemIndexNum == ropeIndexNum && !haveRope && !keepItem)  // ������ ���� ��� ��ư ��Ȱ��ȭ
        {
            //throwRopeButton.gameObject.SetActive(false);
            throwRopeButton.enabled = false;
            Color throwRopeButtonColor = throwRopeButton.GetComponent<Image>().color;
            throwRopeButtonColor.a = 0.5f;
            throwRopeButton.GetComponent<Image>().color = throwRopeButtonColor;
        }
        else if (escapePrisonButton != null && itemIndexNum == keyIndexNum && !haveKey && !keepItem) // ���谡 ���� (�����̸鼭 ���Ż���� ����� �� �ִ� ��찡 �ƴϸ�) ��ư ��Ȱ��ȭ
        {
            if (spyBoolManager.isSpy() && !spyBoolManager.gameObject.GetComponent<SpyBeaverAction>().spyBeaverEscape)     // �� ���� if���� �ϳ��� ��ġ�� ����
            {

            }
            else
            {
                //escapePrisonButton.SetActive(false);
                escapePrisonButton.enabled = false;
                Color escapeButtonColor = escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
                
        }

    }

    public void UseResource(int[] useResourceCount) // �κ��丮 ��ü�� ���Ǹ鼭 4���� �ڿ��̶�� ����ϴ� ���
    {
        int[] remainResource = new int[4];
        
        for (int i = 0; i < 4; i++)
        {
            remainResource[i] = useResourceCount[i];    // ����� �ڿ�
        }

        for (int i = 0; i < itemSlots.Count; i++)   // ���Ե��� �ϳ��� üũ
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
                    remainResource[itemNum] -= childItemObj.GetComponent<ItemCount>().count;    // ���� ����� �ڿ��� ���� ������ ������ ���� ��
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-childItemObj.GetComponent<ItemCount>().count);    // ���� ������ ������ ���� 0����

                    if (!itemSlots[i].gameObject.GetComponent<ItemSlot>().storageSlot) // â������ �ƴϸ� -> �÷��̾� �κ��丮 �����̶�� ������ ����
                    {
                        Destroy(childItemObj);
                    }
                }
                else    // �����ؾ��� �ڿ��� ���� ���Կ� �ִ� ������ ������ ���� ���
                {
                    childItemObj.GetComponent<ItemCount>().ShowItemCount(-remainResource[itemNum]); // ���� ������ ������ ���� ����
                    remainResource[itemNum] = 0;    // ���� ����� �ڿ� ���� 0����

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
