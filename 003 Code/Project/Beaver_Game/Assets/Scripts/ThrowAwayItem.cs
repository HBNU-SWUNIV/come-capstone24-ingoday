using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ThrowAwayItem : MonoBehaviour, IDropHandler
{
    public GameObject copyItemImage;


    public void OnDrop(PointerEventData eventData)
    {
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // ���� ���� ���� ���¶��
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // ���� �� ������ �ڰ� true�� �� ����
        }
        else
        {
            Destroy(eventData.pointerDrag);
        }

        this.gameObject.transform.parent.GetComponent<InventorySlotGroup>().NowResourceCount();
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
