using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipCharacterCamera : MonoBehaviour
{
    // ������ ����ȭ�鿡 ����� �����ֱ� ���� ī�޶�, ���� ������ ������ ȹ���� �� Layer�� EquipItem���� �صα�, Player�� EquipItem Layer���� �����ִ� ī�޶�

    [SerializeField]
    private Transform playerTransform;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, this.transform.position.z);    // ī�޶� ��ġ ����
    }
}
