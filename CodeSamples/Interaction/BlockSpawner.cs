using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlockSpawner : MonoBehaviourPun
{
    public GameObject[] blockPrefabs;
    public Transform spawnPoint; 

    
    public void SpawnBlock(int index)
    {
        if (PhotonNetwork.IsConnected && index >= 0 && index < blockPrefabs.Length)
        {
            
            PhotonNetwork.Instantiate(blockPrefabs[index].name, spawnPoint.position, Quaternion.identity);
        }
    }
}
