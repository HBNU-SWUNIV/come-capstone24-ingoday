using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomListItem : MonoBehaviour
{
    public TMP_Text roomInfo;


    //Ŭ���Ǿ����� ȣ��Ǵ� �Լ�
    public Action<string> onDelegate;

    public void SetInfo(string roomName, int currPlayer, int maxPlayer)
    {
        name = roomName;
        roomInfo.text = roomName + '(' + currPlayer + '/' + maxPlayer + ')';
    }

    public void OnClick()
    {
        //���� onDelegate �� ���� ����ִٸ� ����
        if (onDelegate != null)
        {
            onDelegate(name);
        }
        ////InputRoomName ã�ƿ���
        GameObject go = GameObject.Find("InputRoomName");
        ////ã�ƿ� ���ӿ�����Ʈ���� InputField ������Ʈ ��������
        InputField inputField = go.GetComponent<InputField>();
        ////������ ������Ʈ���� text ���� ���� �̸����� �����ϱ�
        inputField.text = name;
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
