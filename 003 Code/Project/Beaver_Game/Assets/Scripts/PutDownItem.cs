using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PutDownItem : MonoBehaviour, IDropHandler
{
    public Transform playerPos;
    public GameObject copyItemImage;


    public void OnDrop(PointerEventData eventData)
    {
        GameObject newResource = Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject);
        newResource.transform.position = playerPos.position + Vector3.down * 2;
        newResource.GetComponent<ItemCollisionManager>().itemCount = eventData.pointerDrag.GetComponent<ItemCount>().count;

        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);

        if (eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().keepItemCount > 0)    // 만약 수를 나눈 상태라면
        {
            eventData.pointerDrag.gameObject.GetComponent<ItemDrag>().ItemDrop(this.transform.position, this.transform, true);  // 앞의 두 변수는 뒤가 true면 안 쓰임
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
