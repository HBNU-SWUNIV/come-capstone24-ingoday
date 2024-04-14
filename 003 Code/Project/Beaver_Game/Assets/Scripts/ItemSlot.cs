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
    GameObject equipItem = null;

    public int equipSlotType = 0;   // 1: �Ӹ�, 2: �� ����, 3: �ٸ� ��..  itemInfo�� itemCategory�� ���ڰ� ������, 0�� ������ ���� ������ �ƴ��� �ǹ�


    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null || eventData.pointerDrag.GetComponent<ItemDrag>().dropped)
        {
            return;
        }
            
        if (equipSlotType != 0 && eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.GetComponent<ItemInfo>().itemCategory != equipSlotType) // �������Կ��� �׿� �ش��ϴ� �����۸� ������
        {
            return;
        }



        if (this.transform.childCount > 0)  // �ش� ���Կ� �������� �ִ� ���
        {
            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType != 0) // �������Կ��� ���� �������� �� �������� �ֵ���
            {
                return;
            }


            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber)    // �巡���� �����۰� ���� ���
            {
                this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemCount>().count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
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
        else    // �ش� ���Կ� �������� ���� ���
        {
            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType != 0) // �������Կ��� ���� �������� ĳ���Ϳ��� �����
            {
                Destroy(eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);
            }


            if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalPos == this.transform.position)
            {
                eventData.pointerDrag.gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount);
            }
            else
            {
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);   // �巡���� ������ ���� �������� �ű��
            }
            
        }


        if (equipSlotType > 0)  // �������Կ� �������� ������ ���
        {
            if (equipItem != null)
            {
                Destroy(equipItem);
            }

            equipItem = Instantiate(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject, this.transform.parent.GetComponent<ItemEquipManager>().player.transform);
            equipItem.GetComponent<SpriteRenderer>().sortingOrder = 11;


            EquipItem();
        }


        eventData.pointerDrag.GetComponent<ItemDrag>().dropped = true;
    }

    public void EquipItem()
    {

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
