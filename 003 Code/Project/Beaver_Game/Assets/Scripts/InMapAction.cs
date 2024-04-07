using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    public GetResourceManager getResourceManager;
    public Button actionButtonImage;
    private string tagName = "";
    private GameObject damGameObject = null;
    
    private Transform ResourcePos;

    public InventorySlotGroup storageSlotGroup;


    private void OnTriggerEnter2D(Collider2D collision) // ��ư Ȱ��ȭ
    {
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Stone" ||
            collision.gameObject.transform.tag == "Dump" || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
        {
            Color buttonColor = actionButtonImage.GetComponent<Image>().color;
            buttonColor.a = 200;
            actionButtonImage.GetComponent<Image>().color = buttonColor;

            // ��ġ�� ���� ���� ��ư �׸� �ٲ��

            tagName = collision.gameObject.tag;
            Debug.Log(tagName);
            actionButtonImage.interactable = true;

            if (collision.gameObject.transform.tag == "Dam")
            {
                damGameObject = collision.gameObject;
            }
            else
            {
                ResourcePos = collision.transform;
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Stone" || 
            collision.gameObject.transform.tag == "Dump" || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam")
        {
            Color buttonColor = actionButtonImage.GetComponent<Image>().color;
            buttonColor.a = 100;
            actionButtonImage.GetComponent<Image>().color = buttonColor;

            tagName = "";
            Debug.Log(tagName);
            actionButtonImage.interactable = false;
        }
    }

    public void OnClickActionButton()
    {
        switch (tagName)
        {
            case "Forest":
                getResourceManager.GetResourceActive(1, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Mud":
                getResourceManager.GetResourceActive(0, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Dump":
                getResourceManager.GetResourceActive(3, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Stone":
                getResourceManager.GetResourceActive(2, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Storage":
                storageSlotGroup.gameObject.transform.parent.localPosition = Vector3.zero;  // ���߿� �κ�, ���ĭ, â��ĭ �������� invntoryObj�� ��ũ��Ʈ ���� ���� ��Ƽ� �����ϵ���
                storageSlotGroup.gameObject.transform.localPosition = new Vector3(-350.0f, 0.0f, 0.0f);
                break;
            case "Dam":
                damGameObject.GetComponent<DamManager>().DamCreate();
                break;
            default:
                break;
        }
        
    }


    void Start()
    {

    }

    void Update()
    {
        
    }
}
