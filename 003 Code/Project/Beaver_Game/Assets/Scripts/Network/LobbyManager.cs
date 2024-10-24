using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField inputRoomName; // �� �̸��� �Է¹޴� InputField
    public Button btnJoin;           // �� ���� ��ư
    public Button btnCreate;         // �� ���� ��ư
    public InputField inputMaxPlayers; // �ִ� �ο����� �Է¹޴� InputField
    public GameObject roomListItem;
    public Transform rtContent;

    Dictionary<string, RoomInfo> dicRoomInfo = new Dictionary<string, RoomInfo>();


    void Start()
    {
        // �ʱ⿡�� ��ư���� ��Ȱ��ȭ
        btnJoin.interactable = false;
        btnCreate.interactable = false;

        // InputField�� ���� ����� �� ȣ��Ǵ� ������ �߰�
        inputRoomName.onValueChanged.AddListener(OnRoomNameValueChanged);
        inputMaxPlayers.onValueChanged.AddListener(OnMaxPlayerValueChanged);


        /*
        // ���� ���� �� �κ� �ڵ����� �����ϵ��� JoinLobby() ȣ��
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (!PhotonNetwork.InLobby)
            {
                Debug.Log("�ƾƾ�");
                PhotonNetwork.JoinLobby(); // �κ�� �ٽ� ����
            }
        }
        else
        {
            // ������ ������ ����Ǿ� ���� �ʴٸ� ������ �õ�
            PhotonNetwork.ConnectUsingSettings();
        }

        if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby(); // �κ�� ����
            }
        }
        */
    }

    // �濡�� �������� �ٽ� �κ� ���� ������Ʈ
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ ������ ����Ǿ����ϴ�.");
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // �κ� ����
        }
    }

    // �κ� ���������� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        //Content�� �ڽ����� �پ��ִ� Item�� �� ����
        DeleteRoomListItem();
        //dicRoomInfo ������ roomList�� �̿��ؼ� ����
        UpdateRoomListItem(roomList);
        //dicRoom�� ������� roomListItem�� ������
        CreateRoomListItem();

    }
    void SelectRoomItem(string roomName)
    {
        inputRoomName.text = roomName;
    }
    void DeleteRoomListItem()
    {

        foreach (Transform tr in rtContent)
        {
            Destroy(tr.gameObject);
        }
    }
    void UpdateRoomListItem(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //dicRoomInfo�� info �� ���̸����� �Ǿ��ִ� key���� �����ϴ°�
            if (dicRoomInfo.ContainsKey(info.Name))
            {
                //���࿡ ���� �����Ǿ�����?
                if (info.RemovedFromList)
                {
                    dicRoomInfo.Remove(info.Name); //����
                    continue;
                }
            }
            dicRoomInfo[info.Name] = info; //�߰�
        }
    }
    void CreateRoomListItem()
    {
        foreach (RoomInfo info in dicRoomInfo.Values)
        {
            //�� ���� ������ ���ÿ� ScrollView-> Content�� �ڽ����� ����
            GameObject go = Instantiate(roomListItem, rtContent);
            //������ item���� RoomListItem ������Ʈ�� �����´�.
            RoomListItem item = go.GetComponent<RoomListItem>();
            //������ ������Ʈ�� ������ �ִ� SetInfo �Լ� ����
            item.SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);
            //item Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ� ���
            item.onDelegate = SelectRoomItem;
        }
    }
    void OnNameValueChanged(string s)
    {
        btnJoin.interactable = s.Length > 0;
        if (inputRoomName.text == "")
            btnCreate.interactable = false;
    }
    void OnPlayerValueChange(string s)
    {
        btnCreate.interactable = s.Length > 0;
        if (inputMaxPlayers.text == "")
            btnCreate.interactable = false;
    }






    // ���� �����ϴ� �޼ҵ�
    public void CreateRoom()
    {
        string roomName = inputRoomName.text; // �Է¹��� �� �̸�
        byte maxPlayers;
        
        // �Է¹��� �ִ� �ο����� byte Ÿ������ ��ȯ, ���� �� �⺻�� 5 ����
        if (!byte.TryParse(inputMaxPlayers.text, out maxPlayers))
        {
            maxPlayers = 5;
        }

        // �� �ɼ� ����
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = maxPlayers, // �ִ� �ο��� ����
            IsVisible = true         // ���� �����ǵ��� ����
        };

        // �� ���� ��û
        PhotonNetwork.CreateRoom(roomName, roomOptions);
    }

    // �� ���� �޼ҵ�
    public void JoinRoom()
    {
        string roomName = inputRoomName.text; // �Է¹��� �� �̸�
        PhotonNetwork.JoinRoom(roomName);     // �� ���� ��û
    }

    // �� �̸��� ����� �� ȣ��Ǵ� �޼ҵ�
    private void OnRoomNameValueChanged(string room)
    {
        bool isInteractable = !string.IsNullOrEmpty(room); // �� �̸��� ������� ������ Ȯ��
        btnJoin.interactable = isInteractable;             // �� �̸��� ���� ���� ���� ��ư Ȱ��ȭ
        btnCreate.interactable = isInteractable && !string.IsNullOrEmpty(inputMaxPlayers.text); // �� �̸��� �ִ� �ο����� ���� ���� ���� ��ư Ȱ��ȭ
    }

    // �ִ� �ο����� ����� �� ȣ��Ǵ� �޼ҵ�
    private void OnMaxPlayerValueChanged(string max)
    {
        btnCreate.interactable = !string.IsNullOrEmpty(max) && !string.IsNullOrEmpty(inputRoomName.text); // �ִ� �ο����� �� �̸��� ���� ���� ���� ��ư Ȱ��ȭ
    }




    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("OnCreatedRoom");
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("OnCreateRoomFailed, " + returnCode + " , " + message);
        // �ʿ� �� ���� ó�� (��: ����ڿ��� ���� �޽��� ǥ��)
    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("OnJoinedRoom");
        PhotonNetwork.LoadLevel("MainGameScene"); // �濡 �����ϸ� "MainGameScene" �ε�

    }

    // �� ���� ���� �� ȣ��Ǵ� �ݹ� �޼ҵ�
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("OnJoinRoomFailed, " + returnCode + ", " + message);
        // �ʿ� �� ���� ó�� (��: ����ڿ��� ���� �޽��� ǥ��)
    }



    public void OnClickBackToTitleButton()
    {
        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby(); // �κ񿡼� ������ �۾� ����
        }
        else
        {
            PhotonNetwork.Disconnect(); // �ٷ� �������� ������
        }
    }

    // �κ� ���� �� ȣ��Ǵ� �ݹ�
    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        print("OnLeftLobby");
        PhotonNetwork.Disconnect(); // �κ� ���� �� ���� ���� ����
    }

    // ���� ������ ���� �� ȣ��Ǵ� �ݹ�
    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        print("OnDisconnected");
        SceneManager.LoadScene("ConnectionScene"); // ���� ������ ���� �� �� ��ȯ
    }

}