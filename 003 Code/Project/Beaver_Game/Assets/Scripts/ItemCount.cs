using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCount : MonoBehaviour
{
    // �κ��丮, â�� ����� ������ ���� ����

    public int count;
    private TMP_Text countText;
    

    public int ItemCountHalf()  // ������ ���� ������ ������, ���콺 ������ ��ư�� ���� �κ��丮���� ����ִ� ������ ���� ������ �Ҷ� ���
    {
        if (count <= 1)
            return 0;
        int temp = count - count / 2;
        ShowItemCount(-temp);
        return temp;
    }

    public void ShowItemCount(int addCount) // ���� �ش� �������� �� ���
    {
        count += addCount;
        countText.text = count.ToString();
    }

    public void SetCountText()  // �����۰� �� �������� ���� ���� TMP ����
    {
        countText = this.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    void Start()
    {
        SetCountText();
    }

    void Update()
    {
        
    }
}
