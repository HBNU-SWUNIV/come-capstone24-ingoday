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
    private float scrollSpeed = 0.5f;   // ��ũ�ѹ� ���ǵ�

    private bool scrolling = false; // ��ũ�ѹٰ� �����̰� �ִ���

    public void SetScrollbar()  // ��ũ�ѹ��� �ʱⰪ, ��ǥ ������ ����
    {
        scrollbar.value = 0.0f; // �ʱⰪ
        randRange = Random.value;   // ��ǥ ������ ����
        ScrollbarRandRangeImage.localPosition = new Vector3(randRange * 1000.0f, -50.0f, 0.0f); // ��ǥ ���� �̹��� ��ġ
        scrolling = true;

        StartCoroutine(AutoScrolling());
    }

    public int StopScrolling()
    {
        scrolling = false;  // ��ũ�ѹ� ���� ����
        StopCoroutine(AutoScrolling());

        // ��� ��������� ���� ȹ�� �ڿ� �� �ٸ���
        if (scrollbar.value >= randRange - 0.05f && scrollbar.value <= randRange + 0.05f)
        {
            return 2;
        }
        else if (scrollbar.value >= randRange - 0.15f && scrollbar.value <= randRange + 0.15f)
        {
            return 1;
        }
        else
        {
            return 0;
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
