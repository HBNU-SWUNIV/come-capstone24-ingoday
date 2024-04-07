using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollisionManager : MonoBehaviour
{
    private InventorySlotGroup inventorySlotGroup;
    private GameObject itemImage;
    private ItemIndex itemIndex;
    public int itemCount = 1;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        int emptySlotNum = 0;
        bool addInventory = false;
        for (int i = inventorySlotGroup.itemSlots.Count - 1; i >= 0; i--)
        {
            if (inventorySlotGroup.itemSlots[i].gameObject.transform.childCount > 0)
            {
                if (inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().GetItemIndexNumber()
                    == this.GetComponent<ItemInfo>().GetItemIndexNumber())
                {
                    inventorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(itemCount);


                    addInventory = true;
                    break;
                }
            }
            else
            {
                emptySlotNum = i;
            }
        }

        if (!addInventory)
        {
            GameObject newItemImage = Instantiate(itemImage);

            newItemImage.GetComponent<ItemDrag>().SetSpriteRender(itemIndex.items[this.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber()].gameObject.GetComponent<SpriteRenderer>());


            newItemImage.transform.SetParent(inventorySlotGroup.itemSlots[emptySlotNum].gameObject.transform);
            newItemImage.GetComponent<ItemDrag>().SetNromalState();

            newItemImage.GetComponent<ItemCount>().SetCountText();
            newItemImage.GetComponent<ItemCount>().ShowItemCount(itemCount);
        }

        inventorySlotGroup.NowResourceCount();

        Destroy(this.gameObject);
    }


    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        itemImage = GameObject.Find("ItemImage").gameObject;
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
    }

    void Update()
    {

    }
}
