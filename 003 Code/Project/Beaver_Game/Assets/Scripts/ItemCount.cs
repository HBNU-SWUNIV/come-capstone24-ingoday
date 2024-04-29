using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCount : MonoBehaviour
{
    // 인벤토리, 창고 등에서의 아이템 수를 관리

    public int count;
    private TMP_Text countText;
    

    public int ItemCountHalf()  // 아이템 숫자 반으로 나누기, 마우스 오른쪽 버튼을 통해 인벤토리에서 들고있는 아이템 수를 반으로 할때 사용
    {
        if (count <= 1)
            return 0;
        int temp = count - count / 2;
        ShowItemCount(-temp);
        return temp;
    }

    public void ShowItemCount(int addCount) // 현재 해당 아이템의 수 출력
    {
        count += addCount;
        countText.text = count.ToString();
    }

    public void SetCountText()  // 아이템과 그 아이템의 수를 적을 TMP 연결
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
