using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemCount : MonoBehaviour
{
    public int count;
    private TMP_Text countText;


    public void ShowItemCount(int addCount)
    {
        count += addCount;
        countText.text = count.ToString();
    }

    public void SetCountText()
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
