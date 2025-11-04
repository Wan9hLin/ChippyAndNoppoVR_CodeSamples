using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using Unity.XR.PXR;
using static Unity.XR.PXR.PXR_Input;

public class BlockFace_Photon : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public InputActionReference CombineObj;
    private Collider otherCollider;
    public OutlineDrawer outlineDrawer;
    private PhotonView photonView;



    private void Start()
    {
        photonView = this.transform.parent.GetComponent<PhotonView>();
        CombineObj.action.performed += OnCombineObjPerformed;
        outlineDrawer.ToggleMaterial(false);
    }


    // Trigger when another collider enters this collider
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {


            BlockController_Photon parentBlockController = this.transform.parent.GetComponent<BlockController_Photon>();
            BlockController_Photon otherBlockController = other.transform.parent.GetComponent<BlockController_Photon>();

            if (parentBlockController.IsGrabbed() || otherBlockController.IsGrabbed())
            {
                if (IsCloseEnough(other.gameObject))
                {
                    otherCollider = other;
                    outlineDrawer.ToggleMaterial(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {
            outlineDrawer.ToggleMaterial(false);
        }
    }

    // Check if the other block face is within a certain distance threshold
    private bool IsCloseEnough(GameObject otherFace)
    {
        float distanceThreshold = 0.1f;
        Vector3 thisPosition = this.transform.position;
        Vector3 otherPosition = otherFace.transform.position;

        return Vector3.Distance(thisPosition, otherPosition) < distanceThreshold;
    }

    // Called when the combine input action is performed
    private void OnCombineObjPerformed(InputAction.CallbackContext context)
    {

        // Check if the PhotonView is owned by the local player
        if (!photonView.IsMine)
        {
            Debug.Log("Not owner of the PhotonView. Cannot call RPC.");
            return;
        }


        if (otherCollider != null)
        {

            BlockController_Photon parentBlockController = this.transform.parent.GetComponent<BlockController_Photon>();
            parentBlockController.PrepareForAttachment(otherCollider, this.transform.position, otherCollider.transform.position);
            outlineDrawer.ToggleMaterial(false);

            PXR_Input.SendHapticImpulse(VibrateType.RightController, 1.0f, 130, 300);


        }

    }




}

