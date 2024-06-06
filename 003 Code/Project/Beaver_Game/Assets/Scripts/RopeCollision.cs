using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollision : MonoBehaviour
{
    //private Transform prisonTransform;  // ���� ��ġ
    //public Vector3 direction = Vector3.zero;    
    private InventorySlotGroup inventorySlotGroup;  // �κ��丮
    public int ropeIndexNum = 4;   // ������ ������ ��ȣ�� �ٲ�� �ٲ���� ��
    public float lifeTime = 5.0f;   // ������ ���ư��� �ð�

    /*
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")   // ���� ������ �÷��̾ ������
        {
            Player targetPlayer = PhotonNetwork.CurrentRoom.GetPlayer(collision.gameObject.GetPhotonView().ViewID);
            //PhotonView photonView = collision.gameObject.GetPhotonView();
            collision.gameObject.GetPhotonView().RPC("CaughtByRope", targetPlayer);

            //collision.gameObject.transform.position = prisonTransform.position; // ���� �÷���� �������� ����
            //collision.gameObject.GetComponent<PrisonManager>().CaughtByRope();  // ������ �ý��� �۵�

            if (this.gameObject.GetPhotonView().IsMine)
                inventorySlotGroup.UseItem(ropeIndexNum, 1, false);  // ������ ������ ��ȣ(4), 1�� ���

            Destroy(this.gameObject);   // ���� ���� ����
        }
    }

    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        //prisonTransform = GameObject.Find("Prison").transform;

        Destroy(this.gameObject, lifeTime); // ����(������) ���� lifeTime�� �� �Ǹ� �� ������ ���� 
    }

    void Update()
    {
        //this.transform.Translate(direction * Time.deltaTime * 5.0f);

        this.transform.Translate(Vector3.left * Time.deltaTime * 5.0f); // ������Ÿ��
    }
}
