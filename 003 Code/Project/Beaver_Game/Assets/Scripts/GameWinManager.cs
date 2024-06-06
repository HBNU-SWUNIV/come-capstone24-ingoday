using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [SerializeField]
    private int damCount = 0;   // ������ �� ��
    public GameObject towers;   // ������ Ÿ���� ��Ƶ� ������Ʈ(�ڽ� ���� ������ Ÿ�� ��)
    public SpyBoolManager spyBoolManager;   // ���������� ����

    public void DamCountCheck() // �� �� üũ
    {
        if (++damCount >= 5)    // ���� 5�� �������� ��
        {
            GameEnding(false);
        }
    }

    public void TowerCountCheck()   // Ÿ�� �� üũ
    {
        if (towers.transform.childCount >= 10)  // Ÿ���� �ʿ� 10�� �̻� ���ÿ� �����ϸ� ��
        {
            GameEnding(true);
        }
    }

    public void TimeCheck() // �ð� üũ, �ð��� 0�� �Ǹ� �� �Լ��� ����
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin) // ���� ���
    {
        if (spyBoolManager.isSpy())
        {
            if (spyWin)
            {
                Debug.Log("������ ��� win");
                Debug.Log("����� �¸�");
            }
            else
            {
                Debug.Log("�ù� ��� win");
                Debug.Log("����� �й�");
            }
        }
        else
        {
            if (spyWin)
            {
                Debug.Log("������ ��� win");
                Debug.Log("����� �й�");
            }
            else
            {
                Debug.Log("�ù� ��� win");
                Debug.Log("����� �¸�");
            }
        }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
