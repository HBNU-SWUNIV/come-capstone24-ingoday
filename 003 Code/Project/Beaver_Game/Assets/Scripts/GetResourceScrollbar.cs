using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetResourceScrollbar : MonoBehaviour
{
    public RectTransform ScrollbarRandRangeImage;   // ��ũ�ѹٿ��� ���� ������ �������ִ� ���, �ʷ� �簢�� �̹���
    private float randRange;    // ���� ����
    private Scrollbar scrollbar;    // ��ũ�ѹ�
    private bool scrollingForward = true;   // ��ũ�ѹ� ���� ����(�� �ʿ� ������ �ݴ�� �����ϵ���)
    public float scrollSpeed = 0.5f;   // ��ũ�ѹ� ���ǵ�

    private bool scrolling = false; // ��ũ�ѹٰ� �����̰� �ִ���

    public int[] equipItemLevel = { 0, 0, 0, 0 };   // ���� ��� ��� �����ϰ� �ִ���, ��, ����, ��, ö�� ������ �ִ� ����, 0: �⺻, 1: �������, 2: �����, 3: ö���
    public int fourleafCloverBonus = 0;  // Ŭ�ι� ������ ���� �� �ڿ� ȹ�淮 ����
    private int nowResource = 0;    // ���� ��Ĩ�ϴ� �ڿ�, 0: ��, 1: ����, 2: ��, 3: ö

    public void SetScrollbar(int resourceNum)  // ��ũ�ѹ��� �ʱⰪ, ��ǥ ������ ����
    {
        nowResource = resourceNum;
        scrollbar.value = 0.0f; // �ʱⰪ
        randRange = Random.value;   // ��ǥ ������ ����
        ScrollbarRandRangeImage.localPosition = new Vector3(randRange * 1000.0f, -50.0f, 0.0f); // ��ǥ ���� �̹��� ��ġ
        ScrollbarRandRangeImage.localScale = new Vector3(1.0f + equipItemLevel[nowResource] / 3.0f, 1.0f, 1.0f);    // ��ǥ ���� �̹��� ũ��


        scrolling = true;

        StartCoroutine(AutoScrolling());
    }

    public int StopScrolling()
    {
        scrolling = false;  // ��ũ�ѹ� ���� ����
        StopCoroutine(AutoScrolling());

        // ��� ��������� ���� ȹ�� �ڿ� �� �ٸ���
        if (scrollbar.value >= randRange - 0.05f * (3.0f + equipItemLevel[nowResource]) / 3.0f && scrollbar.value <= randRange + 0.05f * (3.0f + equipItemLevel[nowResource]) / 3.0f)
        {
            return 2 + equipItemLevel[nowResource] * 4 / 3 + fourleafCloverBonus;
        }
        else if (scrollbar.value >= randRange - 0.15f * (3.0f + equipItemLevel[nowResource]) / 3.0f && scrollbar.value <= randRange + 0.15f * (3.0f + equipItemLevel[nowResource]) / 3.0f)
        {
            return (1 + equipItemLevel[nowResource] * 2 / 3) + fourleafCloverBonus;
        }
        else
        {
            return fourleafCloverBonus;
        }
        
    }


    IEnumerator AutoScrolling() // ��ũ�ѹ� �¿�� ��� �̵�
    {
        while (scrolling)
        {
            if (scrollingForward)   // �� �� ���� ������ �ݴ������� �����ϵ���
            {
                scrollbar.value += scrollSpeed * Time.deltaTime;
                if (scrollbar.value >= 1.0f)
                {
                    scrollbar.value = 1.0f;
                    scrollingForward = false;
                }
            }
            else
            {
                scrollbar.value -= scrollSpeed * Time.deltaTime;
                if (scrollbar.value <= 0.0f)
                {
                    scrollbar.value = 0.0f;
                    scrollingForward = true;
                }
            }
            yield return null;
        }
    }

    void Start()
    {
        scrollbar = this.GetComponent<Scrollbar>();

    }

    void Update()
    {

    }
}
