using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Photon.Pun;
using Photon.Realtime;
using Unity.XR.PXR;
using static Unity.XR.PXR.PXR_Input;

public class BlockController_Photon : MonoBehaviourPunCallbacks
{
    private bool isGrabbed = false;
    private XRGrabInteractable grabInteractable;
    private PhotonView photonView;
    private Collider otherCollider;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.onSelectEntered.AddListener(HandleSelectEntered);
        grabInteractable.onSelectExited.AddListener(HandleSelectExited);

    }

    private void HandleSelectEntered(XRBaseInteractor interactor)
    {
        isGrabbed = true;
    }

    private void HandleSelectExited(XRBaseInteractor interactor)
    {

        isGrabbed = false;  
    }

    public bool IsGrabbed()
    {
        return isGrabbed;
    }

    // Prepare for attaching this block to another
    public void PrepareForAttachment(Collider other, Vector3 thisFacePosition, Vector3 otherFacePosition)
    {
        this.otherCollider = other;
        if (photonView.IsMine) // Ensure only the owner can execute the RPC
        {          
            photonView.RPC("AttachBlocks", RpcTarget.All, thisFacePosition, otherFacePosition, other.GetComponentInParent<PhotonView>().ViewID);
        }
    }


    // RPC method to attach blocks together
    [PunRPC]
    public void AttachBlocks(Vector3 thisFacePosition, Vector3 otherFacePosition, int otherColliderViewID)
    {
      //  PXR_Input.SendHapticImpulse(VibrateType.RightController, 1.0f, 130, 300);
        GameObject block1 = this.gameObject;

        PhotonView otherPhotonView = PhotonView.Find(otherColliderViewID);
        if (otherPhotonView == null)
        {
            Debug.LogError("PhotonView not found for the given ID: " + otherColliderViewID);
            return;
        }

        GameObject block2 = otherPhotonView.gameObject;
        if (block2 == null)
        {
            Debug.LogError("No gameObject found for the given PhotonView ID: " + otherColliderViewID);
            return;
        }

        Vector3 offset = otherFacePosition - thisFacePosition;   
        Vector3 newPosition = block1.transform.position + offset;
        block1.transform.position = newPosition; 
        photonView.RPC("SyncFixedJoint", RpcTarget.All, otherColliderViewID);

    }

    // game initialization bond block
    [PunRPC]
    public void InitialAttachBlocks(Vector3 thisFacePosition, Vector3 otherFacePosition, int otherColliderViewID)
    {

        GameObject block1 = this.gameObject;

        PhotonView otherPhotonView = PhotonView.Find(otherColliderViewID);
        if (otherPhotonView == null)
        {
            Debug.LogError("PhotonView not found for the given ID: " + otherColliderViewID);
            return;
        }

        GameObject block2 = otherPhotonView.gameObject;
        if (block2 == null)
        {
            Debug.LogError("No gameObject found for the given PhotonView ID: " + otherColliderViewID);
            return;
        }

        
        Vector3 offset = otherFacePosition - thisFacePosition;

       
        Vector3 newPosition = block1.transform.position + offset;

       
        block1.transform.position = newPosition;

        
        photonView.RPC("SyncFixedJoint", RpcTarget.All, otherColliderViewID);



    }

    [PunRPC]
    public void ReAttachBlocks(Vector3 thisFacePosition, Vector3 otherFacePosition, int otherColliderViewID)
    {

        GameObject block1 = this.gameObject;

        PhotonView otherPhotonView = PhotonView.Find(otherColliderViewID);
        if (otherPhotonView == null)
        {
            Debug.LogError("PhotonView not found for the given ID: " + otherColliderViewID);
            return;
        }

        GameObject block2 = otherPhotonView.gameObject;
        if (block2 == null)
        {
            Debug.LogError("No gameObject found for the given PhotonView ID: " + otherColliderViewID);
            return;
        }

       
        Vector3 offset = otherFacePosition - thisFacePosition;

      
        Vector3 newPosition = block1.transform.position + offset;

       
        block1.transform.position = newPosition;

     
        photonView.RPC("SyncFixedJoint", RpcTarget.All, otherColliderViewID);



    }

    // RPC method to sync fixed joint attachment
    [PunRPC]
    public void SyncFixedJoint(int connectedBodyViewID)
    {
        PhotonView connectedBodyPhotonView = PhotonView.Find(connectedBodyViewID);
        Rigidbody connectedBody = connectedBodyPhotonView.GetComponent<Rigidbody>();

        FixedJoint fj = this.gameObject.AddComponent<FixedJoint>();
        fj.connectedBody = connectedBody;
        fj.breakForce = Mathf.Infinity;
        fj.breakTorque = Mathf.Infinity;
        fj.enableCollision = false;
        fj.enablePreprocessing = false;
    }

    // Get all face positions of the block (used for alignment checks)
    public List<Vector3> GetAllFacePositions()
    {
        List<Vector3> facePositions = new List<Vector3>();
        Transform[] faces = this.GetComponentsInChildren<Transform>();
        foreach (Transform face in faces)
        {
            if (face.gameObject.tag == "CubeFace")
            {
                facePositions.Add(face.position);
            }
        }
        return facePositions;
    }

}