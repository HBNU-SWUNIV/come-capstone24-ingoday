using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public bool storageSlot = false;
    private InventorySlotGroup playerInventory = null;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || eventData.pointerDrag.GetComponent<ItemDrag>().dropped)
        {
            return;
        }
            


        if (this.transform.childCount > 0)  // 해당 슬롯에 아이템이 있는 경우
        {
            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber)    // 드래그한 아이템과 같을 경우
            {
                this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemCount>().count);    // 슬롯의 아이템 수 변경(드래그한 수 더하기)
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);

                if (storageSlot)
                {
                    this.transform.parent.gameObject.GetComponent<InventorySlotGroup>().NowResourceCount();
                    playerInventory.NowResourceCount();
                }
            }
            else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount < 1 && !storageSlot)
            {
                this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(eventData.pointerDrag.GetComponent<ItemDrag>().normalPos, eventData.pointerDrag.GetComponent<ItemDrag>().normalParent);
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);
            }
            else
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount);
            }
            
        }
        else
        {
            if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalPos == this.transform.position)
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount);
            }
            else
            {
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);   // 드래그한 도구를 현재 슬롯으로 옮기기
            }
            
        }
        eventData.pointerDrag.GetComponent<ItemDrag>().dropped = true;
    }


    void Start()
    {
        if (storageSlot)
        {
            playerInventory = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        }


    }

    void Update()
    {
        
    }
}
