using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RopeManager : MonoBehaviourPunCallbacks
{
    public GameObject ropePrefab;   // ����(�������� ���) ������
    public Button throwRopeButton;  // ���� ������ ��ư
    private GameObject nowRope = null;

    public void ThrowRopeLineLeftRightChange()  // �¿� ������ ���� ���� ������ ���ؼ� ����
    {
        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
    }

    public void OnClickThrowRopeButton()    // ���� ������ ��ư Ŭ���ϸ� ���ؼ� ��Ÿ��
    {
        if (this.nowRope == null)
            transform.GetChild(0).gameObject.SetActive(true);
    }

    void Start()
    {
        if (!this.gameObject.transform.parent.gameObject.GetPhotonView().IsMine)
            return;

        // ��Ʈ ������ ��ư ������ OnClickThrowRopeButton �Լ��� �۵��ǵ��� ����
        throwRopeButton = GameObject.Find("ThrowRopeButton").GetComponent<Button>();
        throwRopeButton.onClick.AddListener(OnClickThrowRopeButton);
        //throwRopeButton.gameObject.SetActive(false);
        throwRopeButton.interactable = false;
    }

    void Update()
    {
        if (this.transform.GetChild(0).gameObject.activeSelf && this.transform.parent.gameObject.GetPhotonView().IsMine)   // ���ؼ��� ��Ÿ�� ���¶��
        {
            // ���ؼ��� ���콺�� ���� ȸ���ϵ���
            Vector3 rot = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position + Vector3.forward * 10;
            float angle = Mathf.Atan2(rot.y, rot.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.Euler(0, 0, angle);

            // ���콺 ��Ŭ���ϸ� ���� ����
            if (Input.GetMouseButtonDown(0))
            {
                nowRope = PhotonNetwork.Instantiate(ropePrefab.name, this.transform.position + rot.normalized * 1.5f, Quaternion.Euler(0, 0, angle + 180.0f));
                //GameObject newRope = Instantiate(ropePrefab);   // ����(�������� ���) ����
                //newRope.transform.position = this.transform.position + rot.normalized * 3.5f;   // ���� ��ġ ����, 3.5f�� �ڱ� �ڽſ��� ���� �ʰ� �Ϸ���
                //newRope.GetComponent<RopeCollision>().SetDirection(rot.normalized);
                //newRope.transform.localRotation = Quaternion.Euler(0, 0, angle + 180.0f);   // ���� ���� ����

                if (this.transform.localRotation.eulerAngles.z < 90.0f || this.transform.localRotation.eulerAngles.z > 270.0f)  // ������ �� �Ʒ� �̹����� �������� �ʵ��� ����
                {
                    nowRope.transform.localScale = new Vector3(0.25f, -0.2f, 0.0f);
                }

                transform.GetChild(0).gameObject.SetActive(false);  // ���ؼ� ���ֱ�
            }
            
        }
    }
}
