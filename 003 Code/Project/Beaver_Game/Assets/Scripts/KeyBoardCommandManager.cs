using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardCommandManager : MonoBehaviour
{
    public GameObject inventoryObj; // �κ��丮

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))    // ���� ����Ʈ Ű�� �κ��丮 ���� �ݱ�
        {
            if (inventoryObj.transform.localPosition != Vector3.zero)
            {
                inventoryObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                inventoryObj.transform.localPosition = new Vector3(2000, 0, 0);
            }

            if (inventoryObj.transform.GetChild(1).localPosition == new Vector3(-350.0f, 0.0f, 0.0f))   // â�� ���������� �κ��丮 �����鼭 ���� �ݱ�
            {
                inventoryObj.transform.GetChild(1).localPosition = new Vector3(2000.0f, 0.0f, 0.0f);
            }
        }
    }
}
