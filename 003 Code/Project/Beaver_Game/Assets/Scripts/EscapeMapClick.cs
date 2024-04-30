using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Transform escapeTransform;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(gameObject.name);

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Color color = new Color(1.0f, 0.8f, 0.8f);
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Image>().color = Color.white;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}