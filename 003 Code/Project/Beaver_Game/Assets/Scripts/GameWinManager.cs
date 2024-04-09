using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinManager : MonoBehaviour
{
    [SerializeField]
    private int damCount = 0;
    public GameObject towers;
    public SpyBoolManager spyBoolManager;

    public void DamCountCheck()
    {
        if (++damCount >= 5)
        {
            GameEnding(false);
        }
    }

    public void TowerCountCheck()
    {
        if (towers.transform.childCount >= 10)
        {
            GameEnding(true);
        }
    }

    public void TimeCheck()
    {
        GameEnding(true);
    }

    public void GameEnding(bool spyWin)
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
