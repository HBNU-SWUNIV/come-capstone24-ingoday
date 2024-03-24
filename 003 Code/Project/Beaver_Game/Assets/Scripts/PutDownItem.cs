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
        Debug.Log("00000000");
        GameObject newResource = Instantiate(eventData.pointerDrag.GetComponent<ItemDrag>().itemPrefab.gameObject);
        newResource.transform.position = playerPos.position + Vector3.down * 2;
        newResource.GetComponent<ItemCollisionManager>().itemCount = eventData.pointerDrag.GetComponent<ItemCount>().count;

        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f);

        Destroy(eventData.pointerDrag);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
