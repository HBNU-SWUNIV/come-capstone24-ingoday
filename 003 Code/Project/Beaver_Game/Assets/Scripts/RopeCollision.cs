using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeCollision : MonoBehaviour
{
    private Transform prisonTransform;
    public Vector3 direction = Vector3.zero;
    private InventorySlotGroup inventorySlotGroup;
    public int ropeIndexNum = 4;   // ������ ������ ��ȣ�� �ٲ�� �ٲ���� ��
    public float lifeTime = 5.0f;

    /*
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    */

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.gameObject.name);
            collision.gameObject.transform.position = prisonTransform.position;
            collision.gameObject.GetComponent<PrisonManager>().CaughtByRope();
            inventorySlotGroup.UseItem(ropeIndexNum, 1);  // ������ ������ ��ȣ(10), 1�� ���

            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        inventorySlotGroup = GameObject.Find("InventorySlots").GetComponent<InventorySlotGroup>();
        prisonTransform = GameObject.Find("Prison").transform;

        Destroy(this.gameObject, lifeTime);
    }

    void Update()
    {
        //this.transform.Translate(direction * Time.deltaTime * 5.0f);

        this.transform.Translate(Vector3.left * Time.deltaTime * 5.0f);
    }
}
