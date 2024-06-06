using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;    // ������ ����

    public Button buildTowerButton;
    private SpyBeaverAction spyAction;
    private Button spyChangeButton;

    public bool isSpy() // ������ ���� Ȯ��
    {
        return is_Spy;
    }

    public void OnClickSpyChangeButton()    // �׽�Ʈ��, ���������� �ù����� ���� ���¸� �ٲ�
    {
        is_Spy = !is_Spy;
        SpyManager(is_Spy);
    }

    public void SpyManager(bool isSpy)
    {
        if (isSpy)  // �����̶�� �����̸� �� �� �ִ� �ൿ �ѱ�
        {
            spyAction.enabled = true;
            buildTowerButton.gameObject.SetActive(true);
        }
        else    // �����̰� �ƴ϶�� �����̸� �� �� �ִ� �ൿ ����
        {
            spyAction.enabled = false;
            buildTowerButton.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        spyAction = GetComponent<SpyBeaverAction>();
        buildTowerButton = GameObject.Find("BuildTowerButton").GetComponent<Button>();
        spyChangeButton = GameObject.Find("SpyChangeButton").GetComponent<Button>();
        spyChangeButton.onClick.AddListener(OnClickSpyChangeButton);

        //SpyManager(isSpy());
    }

    void Update()
    {
        
    }
}
