using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetResourceManager : MonoBehaviourPunCallbacks
{
    public ItemIndex itemIndex; // ������ ���
    private int getResourceNum = 0; // ������ ��ȣ
    private Transform resourceItemPos;  // ������ ��ġ
    public Sprite[] getResourceSprite;  // �ڿ� ä�� ȭ�� ��� �̹�����
    public Animator beaverWorkAnimator; // �ڿ� ä�� ��� �ִϸ��̼�

    public GetResourceScrollbar resourceScrollbar;
    public SoundEffectManager soundEffectManager;

    //public NetworkManager networkManager;


    public void CloseGetResourceScreen()   // �ڿ� ä�� ȭ�� ������ X ��ư Ŭ��, �ڿ� ä�� ȭ�� �� ���̵��� �ϱ�
    {
        this.gameObject.transform.localPosition = new Vector3(-2000, 0, 0);
        beaverWorkAnimator.SetBool("Work", false);  // �ڿ� ä�� �ִϸ��̼� ���� -> idle ���·�

        soundEffectManager.StopGetResourceSound();
    }

    public void GetResourceActive(int resourceNum, Transform resourceDropPos)   // �ڿ� ä�� ȭ�� ����
    {
        this.transform.GetChild(0).GetComponent<Image>().sprite = getResourceSprite[resourceNum];   // ��� �̹��� ����
        beaverWorkAnimator.SetBool("Work", true);   // �ִϸ��̼� �ѱ�
        getResourceNum = resourceNum;       // �ش� �ڿ��� ��ȣ
        resourceItemPos = resourceDropPos;  // �ش� �ڿ� ä���ϴ� ���� ��ġ

        resourceScrollbar.SetScrollbar(getResourceNum);
        soundEffectManager.PlayGetResourceSound(getResourceNum);
    }

    public void OnClickButtonInGetResource()    // �ڿ� ä�� ��ư Ŭ��
    {
        int resourceResult = resourceScrollbar.StopScrolling();
        //networkManager.CreateItem(itemIndex.items[getResourceNum].gameObject.name, resourceItemPos.position);   // �ڿ� ����

        for (int i = 0; i < resourceResult; i++)
        {
            //Vector3 pos = this.transform.position;
            //float range = 1.5f;

            Vector3 rand = Random.insideUnitCircle * 1.5f;

            PhotonNetwork.Instantiate(itemIndex.items[getResourceNum].gameObject.name, resourceItemPos.position + rand, Quaternion.identity);

            
        }

        /*
        GameObject cloneobj = GameObject.Instantiate(obj);

        Vector3 pos = this.transform.position;
        float range = 1.5f;

        Vector3 rand = Random.insideUnitCircle * range;
        pos = pos + rand;


        cloneobj.transform.position = pos;
        */

        /*
        GameObject newResource = PhotonNetwork.Instantiate(itemIndex.items[getResourceNum].gameObject.name, Vector3.zero, Quaternion.identity);
        newResource.transform.position = resourceItemPos.position;
        */

        CloseGetResourceScreen();   // �ڿ� ä�� ȭ�� �ݱ�
    }

    void Start()
    {

    }

    void Update()
    {
        
    }
}
