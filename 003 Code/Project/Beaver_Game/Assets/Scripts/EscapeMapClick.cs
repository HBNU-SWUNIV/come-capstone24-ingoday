using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EscapeMapClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //public GameObject player;   // �÷��̾�
    public Transform escapeTransform;   // �ش� ������ Ŭ���� �� ������ Ż�� ��ġ


    public void OnPointerClick(PointerEventData eventData)  // �÷��̾ Ż�� ��ġ�� ����
    {
        //this.gameObject.transform.parent.gameObject.GetComponent<MapImages>().player.transform.position = escapeTransform.position;

        this.transform.parent.gameObject.GetComponent<MapImages>().player.gameObject.GetComponent<NavMeshAgent>().Warp(escapeTransform.position);

        this.gameObject.GetComponent<Image>().color = Color.white;
        this.transform.parent.position = new Vector3(0.0f, -1200.0f, 0.0f); // ������ �� ���̵��� ȭ�� ������ �̵�
        //this.transform.parent.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)  // ���콺�� �� ���� ���� ������ �ٸ� ������ ���� ǥ��
    {
        Color color = new Color(1.0f, 0.8f, 0.8f);
        this.gameObject.GetComponent<Image>().color = color;
    }

    public void OnPointerExit(PointerEventData eventData)   // ���콺�� ��ġ�� �ִٰ� ���������� ���� ������ �ǵ���
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
