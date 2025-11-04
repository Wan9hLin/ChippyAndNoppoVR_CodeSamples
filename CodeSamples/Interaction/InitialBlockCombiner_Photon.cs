using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class InitialBlockCombiner_Photon : MonoBehaviourPunCallbacks
{
    public GameObject[] blocks;  
    public float distanceThreshold = 0.1f; 
    private bool initialConnectionDone = false; 
   

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

       
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2 && !initialConnectionDone)
        {
            InitializeBlockConnections();
            initialConnectionDone = true; 
            Debug.Log("Initial Connect Done");
            
        }


    }

    void InitializeBlockConnections()
    {
        
        for (int i = 0; i < blocks.Length; i++)
        {
            Transform[] faces1 = blocks[i].GetComponentsInChildren<Transform>();
            PhotonView photonView1 = blocks[i].GetComponent<PhotonView>();

            for (int j = i + 1; j < blocks.Length; j++)
            {
                Transform[] faces2 = blocks[j].GetComponentsInChildren<Transform>();
                PhotonView photonView2 = blocks[j].GetComponent<PhotonView>();

                foreach (Transform face1 in faces1)
                {
                    if (face1.gameObject.tag != "CubeFace") continue;

                    foreach (Transform face2 in faces2)
                    {
                        if (face2.gameObject.tag != "CubeFace") continue;

                   
                        if (Vector3.Distance(face1.position, face2.position) < distanceThreshold)
                        {
                            
                            photonView1.RPC("InitialAttachBlocks", RpcTarget.AllBuffered, face1.position, face2.position, photonView2.ViewID);
                        }
                    }
                }
            }
        }
    }


}
