using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public SpriteRenderer itemPrefab = null;    // ������ ������
    public Image copyItemImage; // �巡�� �� ���� �ִ� ��ġ�� ��ġ�� ������ ���� �̹��� (���� ������)
    public InventorySlotGroup storageInventorySlotGroup;    // â�� �κ��丮

    public Vector3 normalPos;   // ������ ��ǥ
    public Transform normalParent;  // ������ ��ġ(�������� �θ�)

    public int itemIndexNumber;     // ������ ��ȣ
    public int keepItemCount = 0;   // ��Ŭ������ �������� �������� ������� ���� ���� ������ ��
    private bool keepItemBool = false;  // ��Ŭ������ �������� ����
    public bool dropped = false;    // �������� ������ ����

    public void OnPointerDown(PointerEventData eventData)   // �������� Ŭ��������
    {
        normalParent = this.transform.parent;   // �� ������ �θ�� ����
        normalPos = this.transform.position;    // �� ������ ��ġ�� �̵�
        keepItemCount = 0;
        copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString(); // ���� �������� ����
        dropped = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        copyItemImage.sprite = itemPrefab.sprite;   // ���� �������� �̹��� ����
        /*
        copyItemImage.transform.localRotation = itemPrefab.transform.localRotation; // ���߿� ������ �׸� �ϼ��ǰ� ������ ����
        copyItemImage.transform.localScale = itemPrefab.transform.localScale; // ���߿� ������ �׸� �ϼ��ǰ� ������ ����
        */
        copyItemImage.gameObject.transform.position = normalPos;    // ���� �������� ��ġ ����

        Color copyColor = itemPrefab.color; // ���� �������� �������ϰ�
        copyColor.a = 0.5f;
        copyItemImage.color = copyColor;


        this.transform.SetParent(this.transform.parent.parent.parent);  // �θ� �� ������ ������Ʈ�� -> �巡�� �ϰ� �ִ� �������� ���� ���� ���̵���

        this.GetComponent<Image>().raycastTarget = false;   // �� ������ ���콺�� �ν� �� �ϵ���
        keepItemBool = true;
    }

    public void OnDrag(PointerEventData eventData)  // �巡�� ���϶� ������ ��ġ�� ���콺�� ���������
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)   // �巡�� ���� ��
    {
        keepItemBool = false;
        copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ���
        this.transform.position = normalPos;    // �� �������� ����ġ��
        this.transform.SetParent(normalParent); // �� �������� ���� �θ��

        if (this.transform.parent.gameObject.name != "StorageSlots")    // â�� ������ �ƴ� ���
        {
            this.GetComponent<Image>().raycastTarget = true;    // �ٽ� ������� ���콺�� �ν� �����ϵ���
        }
        else
        {
            storageInventorySlotGroup.NowResourceCount();   // â���� ��� ������ �� ����
        }
    }

    public void ItemDrop(Vector3 dropItmePos, Transform dropItemParent, bool combine)   // �������� ��������
    {
        keepItemBool = false;
        if (keepItemCount > 0)  // �巡���Ҷ� ���� ��ҿ� �������� ���ܵ״ٸ�(���� �����ٸ�) �� ��ġ�� ���� ����
        {
            GameObject newObj = Instantiate(this.gameObject);   // ������ ����
            newObj.transform.position = normalPos;  // ���� ���� ������ ��ġ ����
            newObj.transform.SetParent(normalParent);   // ���� ���� ������ �θ� ����  
            newObj.GetComponent<ItemCount>().SetCountText();    // ������ �� ���� �ؽ�Ʈ ����
            newObj.GetComponent<ItemCount>().count = 0;     // ������ ��
            newObj.GetComponent<ItemCount>().ShowItemCount(keepItemCount);  // ������ �ؽ�Ʈ�� ������ �� ���
            newObj.GetComponent<Image>().raycastTarget = true;  // ���콺�� �ν��� �� �ֵ���
        }

        if (combine)    // �ٸ� ���� �������� ������ ��� �������� ���� �̹����� �����ڸ��� �ǵ����� ���� �巡���� ������ ����
        {
            copyItemImage.transform.position = new Vector3(2100.0f, 1200.0f, 0.0f); // ���� ������ ġ���
            Destroy(this.gameObject);   // �巡�� �� ������ ����
        }

        normalPos = dropItmePos;
        normalParent = dropItemParent;
    }

    public void ItemChange(Vector3 changeItmePos, Transform changeItemParent)   // ������ ��ü(���� �θ�(����) ��ġ�� ���� ��ġ(��ǥ)�� ����)
    {
        normalPos = changeItmePos;
        normalParent = changeItemParent;
        this.transform.position = normalPos;
        this.transform.SetParent(normalParent);
    }

    public void SetSpriteRender(SpriteRenderer prefabSprite)    // �������� �̹��� ����
    {
        itemPrefab = prefabSprite;
        this.GetComponent<Image>().sprite = itemPrefab.sprite;
        /*
        this.GetComponent<Image>().color = itemPrefab.color;
        this.transform.localRotation = itemPrefab.transform.localRotation;
        this.transform.localScale = itemPrefab.transform.localScale;
        */
        itemIndexNumber = itemPrefab.gameObject.GetComponent<ItemInfo>().GetItemIndexNumber();  // ������ ��ȣ ����
    }

    public void SetNromalState()    // ������ ������ �� �⺻ ��ǥ �� �θ� ����
    {
        this.transform.localPosition = Vector3.zero;
        normalParent = this.transform.parent;
        normalPos = this.transform.position;

    }



    void Start()
    {
        copyItemImage = GameObject.Find("CopyItemImage").GetComponent<Image>();

        if (itemPrefab != null) // ������ �̹��� ����
        {
            SetSpriteRender(itemPrefab);
        }
    }

    void Update()
    {
        if (keepItemBool && Input.GetMouseButtonDown(1))    // �������� �� ä�� ��Ŭ�� �� ���� ������ ����
        {
            keepItemCount += this.GetComponent<ItemCount>().ItemCountHalf();
            copyItemImage.gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keepItemCount.ToString();
        }
    }
}
