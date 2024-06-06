using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionToggle : MonoBehaviour
{
    private ProductionManager productionManager = null;
    private ItemIndex itemIndex = null;
    public int itemNumber;
    private GameObject itemCreateObject;


    public void OnChangedToggle()   // �� �������� ���õǸ� �� ������ ����(���� ������ ���� ����)
    {
        if (this.gameObject.GetComponent<Toggle>().isOn)
        {
            itemCreateObject.transform.localPosition = new Vector3(300.0f, 0.0f, 0.0f);
            productionManager.SetSelectedItemmInfo(itemNumber);


        }
    }

    void Start()
    {
        productionManager = GameObject.Find("SelectedItemBackground").GetComponent<ProductionManager>();
        itemCreateObject = GameObject.Find("SelectedItemBackground");
        itemIndex = GameObject.Find("ItemManager").GetComponent<ItemIndex>();
        this.transform.GetChild(1).GetComponent<Image>().sprite = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().sprite;
        this.transform.GetChild(1).GetComponent<Image>().preserveAspect = true;
        /*
        this.transform.GetChild(1).GetComponent<Image>().color = itemIndex.items[itemNumber].GetComponent<SpriteRenderer>().color;  // ���߿� ������ �׸� �ϼ��ǰ� ������ ����
        this.transform.GetChild(1).rotation = itemIndex.items[itemNumber].transform.rotation;   // ���߿� ������ �׸� �ϼ��ǰ� ������ ����
        this.transform.GetChild(1).localScale = itemIndex.items[itemNumber].transform.localScale;   // ���߿� ������ �׸� �ϼ��ǰ� ������ ����
        */
    }

    void Update()
    {
        
    }
}
