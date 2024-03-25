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

        Destroy(eventData.pointerDrag);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
