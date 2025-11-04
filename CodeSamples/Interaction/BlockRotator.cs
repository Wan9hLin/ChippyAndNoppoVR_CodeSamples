using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BlockRotator : MonoBehaviourPunCallbacks
{

    private GameObject currentBlockInArea; // block currently in the area


    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("cubeToy") ||
            other.gameObject.CompareTag("Leg") ||
            other.gameObject.CompareTag("Arm") ||
            other.gameObject.CompareTag("cylinder"))
        {
            currentBlockInArea = other.gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
       
        if (other.gameObject == currentBlockInArea)
        {
            currentBlockInArea = null;
        }
    }

    public void RotateBlockYMinus90()
    {
        RotateBlockInArea(-90, Vector3.up);
    }

    public void RotateBlockYPlus90()
    {
        RotateBlockInArea(90, Vector3.up);
    }

    public void RotateBlockZMinus90()
    {
        RotateBlockInArea(-90, Vector3.forward);
    }

    public void RotateBlockZPlus90()
    {
        RotateBlockInArea(90, Vector3.forward);
    }

    private void RotateBlockInArea(float angleDelta, Vector3 axis)
    {
        if (currentBlockInArea != null && currentBlockInArea.GetPhotonView() != null)
        {
            photonView.RPC("RotateBlockRPC", RpcTarget.All, currentBlockInArea.GetPhotonView().ViewID, angleDelta, axis);
        }
    }

    [PunRPC]
    void RotateBlockRPC(int blockViewID, float angleDelta, Vector3 axis)
    {
        PhotonView targetView = PhotonView.Find(blockViewID);
        if (targetView != null)
        {
            GameObject blockToRotate = targetView.gameObject;

            // Performing rotations in world space
            blockToRotate.transform.Rotate(axis, angleDelta, Space.World);
        }
    }


}
