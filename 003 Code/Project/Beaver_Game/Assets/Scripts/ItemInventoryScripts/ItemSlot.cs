using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviourPunCallbacks, IDropHandler
{
    public bool storageSlot = false;    // â���� �������� ����
    private InventorySlotGroup playerInventory = null;  // �κ��丮
    public GameObject equipItem = null; // ������ ������(�κ��丮�� �ƴ� �� ���� �÷��̾ ����ϰ� �ִ� ������Ʈ)
    public int equipSlotType = 0;   // 1: �Ӹ�, 2: �� ����, 3: �ٸ� ��..  itemInfo�� itemCategory�� ���ڰ� ������, 0�� ������ ���� ������ �ƴ��� �ǹ�
    private int clawNum = 18;   // ���� ����� ��ȣ

    public void OnDrop(PointerEventData eventData)  // �� ���Կ� �������� �巡���ؼ� ������ �ν�
    {
        if (eventData.pointerDrag == null || eventData.pointerDrag.GetComponent<ItemDrag>().dropped)    // �巡�� �� ��ӿ��� ���� ����
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

            if (this.transform.GetChild(0).gameObject.GetComponent<ItemDrag>().itemIndexNumber == eventData.pointerDrag.GetComponent<ItemDrag>().itemIndexNumber)    // �巡���� �����۰� ���� ���
            {
                //this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.GetComponent<ItemCount>().count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
                //eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);

                if (storageSlot)    // â�� ������ ��� â��� ���� �κ��丮 ���ʿ� �ڿ� �� ����
                {
                    this.gameObject.GetPhotonView().RPC("UpdateStorageSlotResourceCount", RpcTarget.All, eventData.pointerDrag.GetComponent<ItemCount>().count);
                    //this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.GetComponent<ItemCount>().count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
                    eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);
                    playerInventory.NowResourceCount();
                    //photonView.RPC("UpdateStorageSlotResourceCount", RpcTarget.All);
                    
                }
                else
                {
                    this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.GetComponent<ItemCount>().count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
                    eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);
                }
            }
            else if (eventData.pointerDrag.GetComponent<ItemDrag>().keepItemCount < 1 && !storageSlot)   // �������� ��Ŭ������ ������ �ʾ�����(�巡�� �� �����۰� �ٸ� �������� ���)
            {
                // �巡�� �� �����۰� ���� ������ ������ ��ȯ
                this.transform.GetChild(0).GetComponent<ItemDrag>().ItemChange(eventData.pointerDrag.GetComponent<ItemDrag>().normalPos, eventData.pointerDrag.GetComponent<ItemDrag>().normalParent);
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);
            }
            else    // �������� ��� �ٽ� ������� ������
            {
                eventData.pointerDrag.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.GetComponent<ItemDrag>().keepItemCount);
            }
        }
        else    // �ش� ���Կ� �������� ���� ���
        {
            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType != 0) // �������Կ��� ���� �������� ĳ���Ϳ��� �����
            {

                // �����Ǿ��ִ� ������ ȿ�� ����� �Լ� �����
                //this.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(eventData.pointerDrag.GetComponent<ItemDrag>().itemIndexNumber, false);
                eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().gameObject.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(eventData.pointerDrag.GetComponent<ItemDrag>().itemIndexNumber, false);

                //Destroy(eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);
                eventData.pointerDrag.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            if (eventData.pointerDrag.GetComponent<ItemDrag>().normalPos == this.transform.position) // ���� �ִ� ���Կ� �״�� �� ��� �ٽ� �ǵ�����
            {
                eventData.pointerDrag.GetComponent<ItemCount>().ShowItemCount(eventData.pointerDrag.GetComponent<ItemDrag>().keepItemCount);
            }
            else    // ���� �ִ� ������ �ƴϸ� ���� �ű��
            {
                eventData.pointerDrag.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, false);   // �巡���� ������ ���� �������� �ű��
            }
            
        }


        if (equipSlotType > 0)  // �������Կ� �������� ������ ���
        {
            if (equipItem != null)  // ������ ���� ī�װ��� �����Ǿ��ִ� �������� ����(ĳ������ �ڽ����� ������� ������ ������Ʈ)
            {

                // �����Ǿ��ִ� ������ ȿ�� ����� �Լ� �����
                this.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), false);


                Destroy(equipItem);
                equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            }

            Transform itemNormalTransform = eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject.transform;
            //Vector3 playerPos = this.transform.parent.GetComponent<ItemEquipManager>().player.transform.position;
            GameObject playerObject = this.transform.parent.GetComponent<ItemEquipManager>().player;
            /*
            int setItemPosX = 1;
            if (playerObject.GetComponent<PlayerMove>().leftRightChange)
            {
                setItemPosX = -1;
            }
            */
            Vector3 newEquipItemPos = playerObject.transform.position + new Vector3(itemNormalTransform.localPosition.x * playerObject.transform.localScale.x, itemNormalTransform.localPosition.y * playerObject.transform.localScale.y, 0.0f);

            // ���� ������ �������� ĳ������ �ڽ����� ����
            equipItem = PhotonNetwork.Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject.name, newEquipItemPos, Quaternion.identity);
            //equipItem.transform.SetParent(this.transform.parent.GetComponent<ItemEquipManager>().player.transform);
            //equipItem.GetPhotonView().ViewID
            equipItem.GetPhotonView().RPC("equipItemSet", RpcTarget.All, playerObject.GetPhotonView().ViewID);

            //equipItem = Instantiate(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemPrefab.gameObject, this.transform.parent.GetComponent<ItemEquipManager>().player.transform);
            //equipItem.transform.localScale = Vector3.one;
            //equipItem.GetComponent<ItemCollisionManager>().enabled = false;
            //equipItem.GetComponent<SpriteRenderer>().sortingOrder = 11; // ĳ���ͺ��� ���� ���̰� �ϱ� ���ؼ� ����(ĳ���ʹ� 10)
            //equipItem.layer = 7;    // ������ �������� �κ��丮�� ������ ����ȭ�鿡 ���̵��� Layer�� 7(EquipItem)���� ����

            if (equipItem.GetComponent<ItemInfo>().GetItemIndexNumber() == clawNum)  // ������ ��� ũ�� ����
            {
                equipItem.transform.localScale = Vector3.one * 0.5f;
            }


            // ������ ������ ȿ�� �ߵ���Ű�� �Լ� �����
            this.transform.parent.gameObject.GetComponent<ItemEquipManager>().SetItemEffect(equipItem.GetComponent<ItemInfo>().GetItemIndexNumber(), true);
        }



        eventData.pointerDrag.GetComponent<ItemDrag>().dropped = true;
    }

    [PunRPC]
    public void UpdateStorageSlotResourceCount(int count)
    {
        this.transform.GetChild(0).gameObject.GetComponent<ItemCount>().ShowItemCount(count);    // ������ ������ �� ����(�巡���� �� ���ϱ�)
        //this.transform.parent.gameObject.GetComponent<InventorySlotGroup>().NowResourceCount();
        this.transform.parent.gameObject.GetComponent<InventorySlotGroup>().StorageResourceCount();
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
