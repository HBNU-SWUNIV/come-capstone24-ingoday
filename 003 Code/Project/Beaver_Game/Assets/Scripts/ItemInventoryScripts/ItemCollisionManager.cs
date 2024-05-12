using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollisionManager : MonoBehaviour
{
    private InventorySlotGroup inventorySlotGroup;
    private Button throwRopeButton;
    [SerializeField]
    private Button escapePrisonButton;
    private GameObject itemImage;
    private ItemIndex itemIndex;
    public int itemCount = 1;


    
    private void OnTriggerEnter2D(Collider2D collision) // ������ ȹ��(�ݱ�)
    {
        if (!this.enabled || collision.gameObject.tag != "Player")
            return;

        int emptySlotNum = 0;
        bool findEmptySlot = false;
        bool addInventory = false;

        for (int i = 0; i < inventorySlotGroup.itemSlots.Count; i++)   // �κ��丮 �� ���� ����
        {
            if (inventorySlotGroup.itemSlots[i].gameObject.transform.childCount > 0)    // �� ĭ�� �ƴ϶��
            {
                if (inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().GetItemIndexNumber()
                    == this.GetComponent<ItemInfo>().GetItemIndexNumber())  // ������ �����۰� ���� ������ �������� ���� ���
                {
                    inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount); // ������ �� ���ϱ�
                    addInventory = true;    // �������� ���ߴٴ� üũ

                    if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")
                    {
                        collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                    }

                    break;
                }
            }
            else if (!findEmptySlot)    // ���� �������� ���� ��츦 ���� �� ĭ ���
            {
                emptySlotNum = i;
                findEmptySlot = true;
            }
        }

        if (!addInventory && findEmptySlot)  // �κ��丮�� ���� �������� ���� �������� ������ ���ߴٸ�
        {
            GameObject newItemImage = Instantiate(itemImage);   // �κ��丮�� �� ������ ����

            newItemImage.GetComponent<ItemDrag>().SetSpriteRender(itemIndex.items[this.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber()].gameObject.GetComponent<SpriteRenderer>());    // ���� ������ �κ��丮�� �����ܿ� Sprite �ֱ�

            newItemImage.transform.SetParent(inventorySlotGroup.itemSlots[emptySlotNum].gameObject.transform);  // ������ ������ �θ� ����
            newItemImage.GetComponent<ItemDrag>().SetNromalState(); // ������ ������ ��ġ ����

            newItemImage.GetComponent<ItemCount>().SetCountText();  // ������ �������� ���� ������ TMP ����
            newItemImage.GetComponent<ItemCount>().ShowItemCount(itemCount);    // ������ TMP�� ���� ������ �������� �� �����ֱ�


            if (this.gameObject.GetComponent<ItemInfo>().itemName == "Rope")
            {
                throwRopeButton.gameObject.SetActive(true);
            }
            else if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")
            {
                collision.gameObject.GetComponent<PrisonManager>().keyCount++;
                escapePrisonButton.gameObject.SetActive(true);
            }
        }

        inventorySlotGroup.NowResourceCount();

        // �������� �ֿ��� ���(������ �ڿ��� ���߰ų� ���� �� ĭ�� ���������) �ٴ��� ������ ����
        if (addInventory || findEmptySlot)
            Destroy(this.gameObject);
    }


    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        itemImage = GameObject.Find("ItemImage").gameObject;
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();

        if (this.gameObject.GetComponent<ItemInfo>().itemName == "Rope")
        {
            throwRopeButton = GameObject.Find("ThrowRopeLine").GetComponent<RopeManager>().throwRopeButton;
        }
        else if (this.gameObject.GetComponent<ItemInfo>().itemName == "Key")
        {
            Debug.Log("11111");
            escapePrisonButton = GameObject.Find("PlayerBeaver").GetComponent<PrisonManager>().escapePrisonButton;
        }

    }

    void Update()
    {

    }
}
