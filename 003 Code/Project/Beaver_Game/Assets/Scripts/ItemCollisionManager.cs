using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollisionManager : MonoBehaviour
{
    private InvnetorySlotGroup invnetorySlotGroup;
    private GameObject itemImage;
    private ItemIndex itemIndex;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;

        int emptySlotNum = 0;
        bool addInventory = false;
        for (int i = invnetorySlotGroup.itemSlots.Count - 1; i >= 0; i--)
        {
            if (invnetorySlotGroup.itemSlots[i].gameObject.transform.childCount > 0)
            {
                if (invnetorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().GetItemIndexNumber()
                    == this.GetComponent<ItemInfo>().GetItemIndexNumber())
                {
                    invnetorySlotGroup.itemSlots[i].gameObject.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(1);


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


            newItemImage.transform.SetParent(invnetorySlotGroup.itemSlots[emptySlotNum].gameObject.transform);
            newItemImage.GetComponent<ItemDrag>().SetNromalState();

            newItemImage.GetComponent<ItemCount>().SetCountText();
            newItemImage.GetComponent<ItemCount>().ShowItemCount(0);
        }



        Destroy(this.gameObject);
    }


    void Start()
    {
        invnetorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InvnetorySlotGroup>();
        itemImage = GameObject.Find("ItemImage").gameObject;
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
    }

    void Update()
    {

    }
}
