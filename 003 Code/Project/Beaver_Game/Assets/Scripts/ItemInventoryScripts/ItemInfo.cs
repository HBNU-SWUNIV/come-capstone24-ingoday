using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEngine;

public class ItemInfo : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private int itemIndexNumber;    // ������ ���� ��ȣ(���� ��ȣ)
    public int itemCategory;    // ������ ����, 0: �ڿ�, 1: �Ӹ� ���, 2: �� ���, 3: �� ��� or ���� �����ϴ� ���
    public int[] requiredResourceOfItem = new int[4];   // ������ ���ۿ� �ʿ��� �ڿ� ��
    public string itemName;     // ������ �̸�
    public string itemInformation;  // ������ �����

    public int GetItemIndexNumber() // ������ ��ȣ �ҷ�����
    {
        return itemIndexNumber;
    }

    [PunRPC]
    public void equipItemSet(int equipPlayerViewID)
    {
        this.gameObject.GetComponent<ItemCollisionManager>().enabled = false;

        this.transform.SetParent(PhotonView.Find(equipPlayerViewID).gameObject.transform);
        this.transform.localScale = Vector3.one;
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 11; // ĳ���ͺ��� ���� ���̰� �ϱ� ���ؼ� ����(ĳ���ʹ� 10)

        this.gameObject.layer = this.gameObject.transform.parent.gameObject.layer;
        //this.gameObject.layer = 7;    // ������ �������� �κ��丮�� ������ ����ȭ�鿡 ���̵��� Layer�� 7(EquipItem)���� ����
    }

    [PunRPC]
    public void equipItemDestroy()
    {
        Destroy(this.gameObject);
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
