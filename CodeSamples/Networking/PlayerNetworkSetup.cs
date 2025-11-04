using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
public class PlayerNetworkSetup : MonoBehaviourPunCallbacks
{

    public GameObject LocalXRRigGameobject;
    public GameObject MainAvatarGameobject;

    public GameObject AvatarHeadGameobject;
    public GameObject AvatarBodyGameobject;


    public GameObject[] AvatarModelPrefabs;

    public TextMeshProUGUI PlayerName_Text;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            //The player is local
            LocalXRRigGameobject.SetActive(true);

            //Getting the avatar selection data so that the correct avatar model can be instantiated.
            object avatarSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
            {
                Debug.Log("Avatar selection number: " + (int)avatarSelectionNumber);
                photonView.RPC("InitializeSelectedAvatarModel", RpcTarget.AllBuffered, (int)avatarSelectionNumber);
            }


            SetLayerRecursively(AvatarHeadGameobject, 6);
            SetLayerRecursively(AvatarBodyGameobject, 7);

            TeleportationArea[] teleportationAreas = GameObject.FindObjectsOfType<TeleportationArea>();
            if (teleportationAreas.Length > 0)
            {
                Debug.Log("Found " + teleportationAreas.Length + " teleportation area. ");
                foreach (var item in teleportationAreas)
                {
                    item.teleportationProvider = LocalXRRigGameobject.GetComponent<TeleportationProvider>();
                }
            }
            MainAvatarGameobject.AddComponent<AudioListener>();

        }
        else
        {
            //The player is remote
            LocalXRRigGameobject.SetActive(false);

            SetLayerRecursively(AvatarHeadGameobject, 0);
            SetLayerRecursively(AvatarBodyGameobject, 0);
        }

        if (PlayerName_Text != null)
        {
            PlayerName_Text.text = photonView.Owner.NickName;
        }
    }


    void SetLayerRecursively(GameObject go, int layerNumber)
    {
        if (go == null) return;
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }

    [PunRPC]
    public void InitializeSelectedAvatarModel(int avatarSelectionNumber)
    {
        GameObject selectedAvatarGameobject = Instantiate(AvatarModelPrefabs[avatarSelectionNumber], LocalXRRigGameobject.transform);

        AvatarInputConverter avatarInputConverter = LocalXRRigGameobject.GetComponent<AvatarInputConverter>();
        ActionBasedContinuousMoveProvider continuousMoveProvider = LocalXRRigGameobject.GetComponent<ActionBasedContinuousMoveProvider>();
        AvatarHolder avatarHolder = selectedAvatarGameobject.GetComponent<AvatarHolder>();
        SetUpAvatarGameobject(avatarHolder.HeadTransform, avatarInputConverter.AvatarHead);
        SetUpAvatarGameobject(avatarHolder.BodyTransform, avatarInputConverter.AvatarBody);
        SetUpAvatarGameobject(avatarHolder.HandLeftTransform, avatarInputConverter.AvatarHand_Left);
        SetUpAvatarGameobject(avatarHolder.HandRightTransform, avatarInputConverter.AvatarHand_Right);

        CharacterController characterController = LocalXRRigGameobject.GetComponent<CharacterController>();

        if (avatarSelectionNumber == 0)  // Assuming 0 is the index for Role 1
        {
            // Set parameters specific to Role 1
            avatarInputConverter.headPositionOffset = new Vector3(0, -0.5f, 0);
            characterController.center = new Vector3(0, 1.2f, 0);
            characterController.height = 1.45f;
            characterController.radius = 0.28f;
            continuousMoveProvider.moveSpeed = 1.6f;
            

          
        }
        else if (avatarSelectionNumber == 1)  // Assuming 1 is the index for Role 2
        {
            // Set parameters specific to Role 2
            avatarInputConverter.headPositionOffset = new Vector3(0, 0f, 0);
            characterController.center = new Vector3(0, 1.45f, 0);
            characterController.height = 1.02f;
            characterController.radius = 0.34f;
            continuousMoveProvider.moveSpeed = 1.85f;

            AvatarHeadGameobject.transform.localPosition = new Vector3(AvatarHeadGameobject.transform.localPosition.x, 0.1f, AvatarHeadGameobject.transform.localPosition.z);  // Replace 1.5f with the desired Y position for Role 2

        }



        SetupAttachPoints();
    }

    void SetUpAvatarGameobject(Transform avatarModelTransform, Transform mainAvatarTransform)
    {
        avatarModelTransform.SetParent(mainAvatarTransform);
        avatarModelTransform.localPosition = Vector3.zero;
        avatarModelTransform.localRotation = Quaternion.identity;
    }


    public void SetupAttachPoints()
    {
        Transform leftHandPokeController = LocalXRRigGameobject.transform.Find("Camera Offset/Left Hand Parent/LeftHand Poke Controller");
        Transform rightHandPokeController = LocalXRRigGameobject.transform.Find("Camera Offset/Right Hand Parent/RightHand Poke Controller");

        GameObject leftAttachPoint = null;
        GameObject rightAttachPoint = null;

        object avatarSelectionNumber;
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(MultiplayerVRConstants.AVATAR_SELECTION_NUMBER, out avatarSelectionNumber))
        {
            if ((int)avatarSelectionNumber == 0)  // Ava1
            {
                leftAttachPoint = GameObject.FindGameObjectWithTag("Ava1_Lhand");
                rightAttachPoint = GameObject.FindGameObjectWithTag("Ava1_Rhand");
            }
            else if ((int)avatarSelectionNumber == 1)  // Ava2
            {
                leftAttachPoint = GameObject.FindGameObjectWithTag("Ava2_Lhand");
                rightAttachPoint = GameObject.FindGameObjectWithTag("Ava2_Rhand");
            }

            if (leftHandPokeController != null && leftAttachPoint != null)
            {
                XRBaseInteractor leftInteractor = leftHandPokeController.GetComponent<XRBaseInteractor>();
                leftInteractor.attachTransform = leftAttachPoint.transform;
            }

            if (rightHandPokeController != null && rightAttachPoint != null)
            {
                XRBaseInteractor rightInteractor = rightHandPokeController.GetComponent<XRBaseInteractor>();
                rightInteractor.attachTransform = rightAttachPoint.transform;
            }
        }
    }



}
