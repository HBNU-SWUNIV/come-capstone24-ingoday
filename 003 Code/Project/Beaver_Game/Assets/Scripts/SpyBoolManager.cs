using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;    // ������ ����

    //public Button buildTowerButton;
    public SpyBeaverAction spyAction;
    //private Button spyChangeButton;
    private ShowRole showRole;
    public ButtonIconManager btnManager;

    public GameObject towerPriceObject;

    public bool isSpy() // ������ ���� Ȯ��
    {
        return is_Spy;
    }

    public void OnClickSpyChangeButton()    // �׽�Ʈ��, ���������� �ù����� ���� ���¸� �ٲ�
    {
        is_Spy = !is_Spy;
        SpyManager();
    }

    public void SetSpyBool(bool isSpy)
    {

        Debug.Log("������ ����: " + isSpy);

        is_Spy = isSpy;
        SpyManager();
    }


    public void SpyManager()
    {
        /*
        if (isSpy)  // �����̶�� �����̸� �� �� �ִ� �ൿ �ѱ�
        {
            spyAction.enabled = true;
            buildTowerButton.gameObject.SetActive(true);
            showRole.SetShowRuleImage(true);
        }
        else    // �����̰� �ƴ϶�� �����̸� �� �� �ִ� �ൿ ����
        {
            spyAction.enabled = false;
            buildTowerButton.gameObject.SetActive(false);
            showRole.SetShowRuleImage(false);
        }
        */

        Debug.Log("SpyManager called. is_Spy: " + is_Spy);

        spyAction.enabled = is_Spy;
        btnManager.buildTowerComunicationButton.gameObject.SetActive(is_Spy);
        showRole.SetShowRuleImage(is_Spy);
        btnManager.SetButtonIcons(is_Spy);

        towerPriceObject.SetActive(is_Spy);
    }

    void Start()
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        //spyAction = GetComponent<SpyBeaverAction>();
        //buildTowerButton = GameObject.Find("BuildTowerButton").GetComponent<Button>();
        btnManager = GameObject.Find("Buttons").GetComponent<ButtonIconManager>();
        //spyChangeButton = GameObject.Find("SpyChangeButton").GetComponent<Button>();
        //spyChangeButton.onClick.AddListener(OnClickSpyChangeButton);
        showRole = GameObject.Find("ShowRoleImage").GetComponent<ShowRole>();
        showRole.gameObject.SetActive(false);
        towerPriceObject = GameObject.Find("TowerPrice");
        towerPriceObject.SetActive(false);

        //SpyManager(isSpy());
    }

    void Update()
    {
        
    }
}
