using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class BarrierManager : MonoBehaviourPunCallbacks
{
    public GameObject barrier;   
    private bool initialConnectionDone = false;  
    private PhotonView photonView;  

    void Awake()
    {
        photonView = GetComponent<PhotonView>();  
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !initialConnectionDone)
        {          
            photonView.RPC("RemoveBarrier", RpcTarget.AllBuffered);
            initialConnectionDone = true; 
        }
    }
    [PunRPC]
    public void RemoveBarrier()
    {
        if (barrier != null)
        {
            barrier.SetActive(false);
        }
    }
}
