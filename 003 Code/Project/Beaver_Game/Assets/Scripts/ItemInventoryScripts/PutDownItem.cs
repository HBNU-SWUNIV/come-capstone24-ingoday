using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PutDownItem : MonoBehaviourPunCallbacks, IDropHandler
{
    public Transform playerPos; // �÷��̾� ��ġ(������ ���������� ���)
    public GameObject copyItemImage;    // ���� ������


    public void OnDrop(PointerEventData eventData)  // ������ ��������
    {
        // �ʵ忡 ������ ����
        GameObject newDropItem = PhotonNetwork.Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject.name, playerPos.position + Vector3.down * 2.0f, Quaternion.identity);
        //newDropItem.transform.position = playerPos.position + Vector3.down * 2;
        //newDropItem.GetComponent<ItemCollisionManager>().itemCount = eventData.pointerDrag.GetComponent<ItemCount>().count;
        newDropItem.GetPhotonView().RPC("SetDropItemCount", RpcTarget.All, newDropItem.GetPhotonView().ViewID, eventData.pointerDrag.GetComponent<ItemCount>().count);


        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ���


        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 4) // ���� ���������� 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
        }
        else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 5)    // Ű ���������� 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            playerPos.gameObject.GetComponent<PrisonManager>().keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(5, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
            /*
            if (playerPos.gameObject.GetComponent<PrisonManager>().keyCount <= 0 && eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount <= 0)
            {
                //playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.gameObject.SetActive(false);
                playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.enabled = false;
                Color escapeButtonColor = playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                playerPos.gameObject.GetComponent<PrisonManager>().escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
            */
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)   // ���Ǿ��ִ� �������̶��
        {
            //Destroy(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);  // �ʵ忡 ����ϰ� �ִ� �͵� ����
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // ���� ���� ���� ���¶�� ���� �ִ� ��ġ�� �����״� ����ŭ �ǵ�����
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // ���� �� ������ �ڰ� true�� �� ����
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }


        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount(); // �κ��丮�� �̹��� ����
    }

    


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
