using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ThrowAwayItem : MonoBehaviour, IDropHandler
{
    public GameObject copyItemImage;    // ���� �̹���
    public PrisonManager prisonManager; // ����(���� �� ����)


    public void OnDrop(PointerEventData eventData)  // ������ ������
    {
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ��


        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 4) // ���� ������ 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(4, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
        }
        else if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().itemIndexNumber == 5)    // Ű ������ 0���� ��ư ��Ȱ��ȭ �ϱ�
        {
            prisonManager.keyCount -= eventData.pointerDrag.GetComponent<ItemCount>().count;
            copyItemImage.transform.parent.gameObject.GetComponent<InventorySlotGroup>().UseItem(5, 0, eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0);
            /*
            if (prisonManager.keyCount <= 0 && eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount <= 0)
            {
                //prisonManager.escapePrisonButton.gameObject.SetActive(false);
                prisonManager.escapePrisonButton.enabled = false;
                Color escapeButtonColor = prisonManager.escapePrisonButton.GetComponent<Image>().color;
                escapeButtonColor.a = 0.5f;
                prisonManager.escapePrisonButton.GetComponent<Image>().color = escapeButtonColor;
            }
            */
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipSlotType > 0)   // ���� �������̾��ٸ� �ʵ��� �����۵� ����
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem.GetPhotonView().RPC("equipItemDestroy", RpcTarget.All);
            //Destroy(eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().normalParent.gameObject.GetComponent<ItemSlot>().equipItem);
        }

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // ���� ���� ���� ���¶��
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // ���� �� ������ �ڰ� true�� �� ����
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount(); // ������ �� ����
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
