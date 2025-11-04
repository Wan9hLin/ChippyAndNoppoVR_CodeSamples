using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DyeZone_photon : MonoBehaviourPunCallbacks
{
    public Material redMaterial;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {

            GameObject parentBlock = other.transform.parent.gameObject;

            if (photonView.IsMine)
            {
                photonView.RPC("ChangeBlockColor", RpcTarget.AllBuffered, parentBlock.GetComponent<PhotonView>().ViewID);
            }
        }
    }


    [PunRPC]
    void ChangeBlockColor(int blockViewID)
    {
        PhotonView blockView = PhotonView.Find(blockViewID);
        if (blockView != null)
        {
            Renderer rend = blockView.gameObject.GetComponent<Renderer>();
            rend.material = redMaterial;
        }
    }

}
