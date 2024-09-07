using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera cinemachineVirtualCamera;

    void Start()
    {
        Screen.SetResolution(1920, 1080, false);    // ȭ�� ���μ��� ����
        PhotonNetwork.ConnectUsingSettings();


        
        Vector3 startPos = new Vector3(25.0f, 5.0f, 0.0f);
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", startPos, Quaternion.identity);    // �÷��̾� ��� ����
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 5 }, null, null);
    }

    public override void OnJoinedRoom()
    {
        Vector3 startPos = new Vector3(25.0f, 5.0f, 0.0f);
        GameObject createdBeaver = PhotonNetwork.Instantiate("PlayerBeaver", startPos, Quaternion.identity);    // �÷��̾� ��� ����
        if (createdBeaver.GetPhotonView().IsMine)
        {
            cinemachineVirtualCamera.Follow = createdBeaver.transform;  // �÷��̾�� �ó׸ӽ� ī�޶� ����
            cinemachineVirtualCamera.LookAt = createdBeaver.transform;
        }
        
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
