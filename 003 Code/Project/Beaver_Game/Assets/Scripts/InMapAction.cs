using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InMapAction : MonoBehaviour
{
    public GameObject productionImage;
    public GetResourceManager getResourceManager;
    public Button actionButtonImage;
    private string tagName = "";
    private GameObject damGameObject = null;
    
    private Transform ResourcePos;

    public InventorySlotGroup storageSlotGroup;


    private void OnTriggerEnter2D(Collider2D collision) // ��ư Ȱ��ȭ
    {
        if (collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter")
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
        if (collision.gameObject.transform.tag == "Mud" || collision.gameObject.transform.tag == "Forest" || collision.gameObject.transform.tag == "Stone" || collision.gameObject.transform.tag == "Dump" 
            || collision.gameObject.transform.tag == "Storage" || collision.gameObject.transform.tag == "Dam" || collision.gameObject.tag == "ProductionCenter")
        {
            Color buttonColor = actionButtonImage.GetComponent<Image>().color;
            buttonColor.a = 100;
            actionButtonImage.GetComponent<Image>().color = buttonColor;

            tagName = "";
            Debug.Log(tagName);
            actionButtonImage.interactable = false;

            if (collision.gameObject.transform.tag == "Dam")
            {
                if (this.gameObject.GetComponent<SpyBoolManager>().isSpy())
                {
                    damGameObject.GetComponent<DamManager>().obstract = 0.0f;
                }
                else
                {
                    damGameObject.GetComponent<DamManager>().accelerate = 0.0f;
                }
            }
        }
    }

    public void OnClickActionButton()
    {
        switch (tagName)
        {
            case "Mud":
                getResourceManager.GetResourceActive(0, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Forest":
                getResourceManager.GetResourceActive(1, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Stone":
                getResourceManager.GetResourceActive(2, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Dump":
                getResourceManager.GetResourceActive(3, ResourcePos);
                getResourceManager.gameObject.transform.localPosition = Vector3.zero;
                break;
            case "Storage":
                storageSlotGroup.gameObject.transform.parent.localPosition = Vector3.zero;
                storageSlotGroup.gameObject.transform.localPosition = new Vector3(-350.0f, 0.0f, 0.0f);
                break;
            case "Dam":
                if (!damGameObject.GetComponent<DamManager>().buildComplete)    // �� �ϰ� ��
                {
                    if (damGameObject.GetComponent<DamManager>().buildNow)  // �� �Ǽ� ��
                    {
                        if (this.gameObject.GetComponent<SpyBoolManager>().isSpy()) // �����̶�� �� �Ǽ� ����
                        {
                            damGameObject.GetComponent<DamManager>().ObstructBuild();
                        }
                        else    // �� �Ǽ� ����
                        {
                            damGameObject.GetComponent<DamManager>().AccelerateBuild();
                        }

                    }
                    else    // �� �Ǽ� ���� ��
                    {
                        damGameObject.GetComponent<DamManager>().DamCreate();   // �� �Ǽ� ����
                    }
                }
                break;
            case "ProductionCenter":
                productionImage.transform.localPosition = Vector3.zero;
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
