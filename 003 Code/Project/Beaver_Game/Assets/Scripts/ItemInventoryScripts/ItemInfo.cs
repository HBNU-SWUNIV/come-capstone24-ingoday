using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField]
    private int itemIndexNumber;
    public int itemCategory;    // 0: �ڿ�, 1: �Ӹ� ���, 2: �� ���, 3: �� ��� or ���� �����ϴ� ���
    public int[] requiredResourceOfItem = new int[4];
    public string itemName;
    public string itemInformation;

    public int GetItemIndexNumber()
    {
        return itemIndexNumber;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
