using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;
    //public Vector3 waitingRoomPos;
    public Transform waitingRoomPos;
    public Transform[] startPos = new Transform[5];
    public TimerManager timerManager;

    public TMP_Text playerCountText;
    public Button gameStartButton;
    public Button outRoomButton;

    private GameObject myBeaver = null;

    public CinemachineManager cinemachineManager;
    public GameObject player;
    public ShowRole showRole;

    public DamManager[] damManagers;

    public bool onGame = false;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);    // ȭ�� ���μ��� ����
        //PhotonNetwork.ConnectUsingSettings();

        
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos.position, Quaternion.identity);    // �÷��̾� ��� ����
        myBeaver = createdBeaver;
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.gameObject.SetActive(true);
            gameStartButton.interactable = false;
        }

        UpdatePlayerCount();

        // ���� �� �ٽ� ���ƿ��� ���� ���� �ο��� üũ
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = true;
        }

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        if (myBeaver = null)
        {
            GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", waitingRoomPos.position, Quaternion.identity);    // �÷��̾� ��� ����
            if (createdBeaver.GetPhotonView().IsMine)
            {
                cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
                cinemachineVirtualCamera.LookAt = createdBeaver.transform;
            }
        }
        
        
    }


    // �÷��̾ �濡 ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            gameStartButton.interactable = true;
        }
    }

    // �÷��̾ �濡�� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCount();

        if (PhotonNetwork.IsMasterClient && !onGame)
        {
            gameStartButton.gameObject.SetActive(true);

            if (PhotonNetwork.CurrentRoom.PlayerCount != PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                gameStartButton.interactable = false;
            }
        }
    }

    // �÷��̾� �� ǥ��
    private void UpdatePlayerCount()
    {
        if (PhotonNetwork.InRoom && playerCountText != null)
        {
            // TextMeshPro ������Ʈ
            playerCountText.text = "Player Count\n" + PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
        }
    }

    // ���� ���� ��ư
    public void OnClickGameStartButton()
    {
        //player.GetComponent<PhotonView>().RPC("SetSpyBool", RpcTarget.All, true);
        //myBeaver.GetComponent<PhotonView>().RPC("SetSpyBool", RpcTarget.All, false);
        //showRole.SetShowRuleImage(false);

        this.gameObject.GetPhotonView().RPC("CallSetSpyBool", RpcTarget.All, false);

        if (PhotonNetwork.IsMasterClient)
        {
            timerManager.SetTimerOn();
            gameStartButton.gameObject.SetActive(false);
            PhotonNetwork.CurrentRoom.IsOpen = false;   // ���� �濡 �÷��̾ ������ �� �ϵ���
            PhotonNetwork.CurrentRoom.IsVisible = false;    // ���� ���� ǥ�õ��� �ʵ��� �����

            int spyNum = Random.Range(0, PhotonNetwork.CurrentRoom.PlayerCount);

            Debug.Log("Player Count: " + PhotonNetwork.CurrentRoom.PlayerCount);
            Debug.Log("Spy Number: " + spyNum);
            Debug.Log("Selected Player: " + PhotonNetwork.PlayerList[spyNum].NickName); // �÷��̾��� NickName Ȯ��
            Debug.Log("TagObject: " + PhotonNetwork.PlayerList[spyNum].TagObject);

            //player.GetComponent<PhotonView>().RPC("SetSpyBool", PhotonNetwork.PlayerList[spyNum], true);
            this.gameObject.GetPhotonView().RPC("CallSetSpyBool", PhotonNetwork.PlayerList[spyNum], true);

            for(int i = 0; i < damManagers.Length; i++)
            {
                damManagers[i].DamRandomPrice();
            }

            //showRole.SetShowRuleImage(true);
        }


        this.gameObject.GetPhotonView().RPC("SetStartSetting", RpcTarget.All);
    }

    [PunRPC]
    public void CallSetSpyBool(bool isSpy)
    {
        Debug.Log("������ ����");
        myBeaver.GetComponent<SpyBoolManager>().SetSpyBool(isSpy);
    }

    public void OnClickOutRoomButton()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;   // ���� �濡 �÷��̾ ������ �� �ϵ���
                PhotonNetwork.CurrentRoom.IsVisible = false;    // ���� ���� ǥ�õ��� �ʵ��� �����
            }

            PhotonNetwork.LeaveRoom();
        }
    }

    // ���� ���������� ���� �� ȣ��Ǵ� �ݹ�
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        print("OnLeftRoom");

        SceneManager.LoadScene("LobbyScene");

        /*
        // ���� ���� �� �κ� �ڵ����� �����ϵ��� JoinLobby() ȣ��
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby(); // �κ�� �ٽ� ����
        }
        else
        {
            SceneManager.LoadScene("LobbyScene");
        }
        */
    }

    // �κ� ���������� ������ �� ȣ��Ǵ� �ݹ�
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print("OnJoinedLobby");
        SceneManager.LoadScene("LobbyScene"); // �κ� ������ �Ŀ� �� ��ȯ
    }


    [PunRPC]
    public void SetStartSetting()
    {
        showRole.gameObject.SetActive(true);

        myBeaver.transform.position = startPos[myBeaver.layer - 6].position;
        playerCountText.gameObject.SetActive(false);
        outRoomButton.gameObject.SetActive(false);
        cinemachineManager.SetCameraRange(1);   // ī�޶� ������ ���濡�� ���� ������ ����
        onGame = true;
    }

    /*
    public void CreateItem(string itemName, Vector3 createPos)    // �ڿ� ä��ĭ���� ��ư ������ �ڿ� ������ �ʵ忡 ����
    {
        GameObject newItem = PhotonNetwork.Instantiate(itemName, createPos, Quaternion.identity);
        //newResource.transform.position = resourcePos;
    }
    */

    /*
    public void StorageResource()
    {
        //PhotonNetwork.
    }
    */

}
