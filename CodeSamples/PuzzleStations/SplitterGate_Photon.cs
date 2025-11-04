using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class SplitterGate_Photon : MonoBehaviourPunCallbacks
{
    public List<GameObject> blocksInGate = new List<GameObject>();
    public GameObject SuccessFx;
    public GameObject cubePrefab;
    public GameObject trianglePrefab;
    public GameObject conePrefab;
    private PhotonView photonView;

    public List<GameObject> blocksInFrontTrigger = new List<GameObject>();
    public List<GameObject> blocksInBackTrigger = new List<GameObject>();

    public GameObject uiPanel;
    public GameObject GateLight;

    public bool isUIActive;

  
    public static int splitCounter = 0;



    void Start()
    {
        photonView = GetComponent<PhotonView>();
        isUIActive = false; 
    }

    public void SplitBlocks()
    {
        if (isUIActive) 
        {
            return;
        }
     
        photonView.RPC("SyncSplitBlocks", RpcTarget.MasterClient);

    }

    [PunRPC]
    public void ButtonPressed()
    {
        RequestOwnershipForAllBlocks(); // Request ownership of all blocks when the button is pressed
        SplitBlocks();
    }

    // RPC method to handle block splitting
    [PunRPC]
    void SyncSplitBlocks()
    {
        if (PhotonNetwork.IsMasterClient) 
        {
            foreach (GameObject oldBlock in blocksInGate)
            {
                PhotonView blockPhotonView = oldBlock.GetComponent<PhotonView>();
                if (blockPhotonView != null && blockPhotonView.IsMine)
                {
                    GameObject fxInstance = PhotonNetwork.Instantiate(SuccessFx.name, oldBlock.transform.position, Quaternion.identity);
                    Destroy(fxInstance, 3f);

                    // Save the state of the old block
                    Vector3 oldPosition = oldBlock.transform.position;
                    Quaternion oldRotation = oldBlock.transform.rotation;
                    string oldTag = oldBlock.tag;
                    Material oldMaterial = oldBlock.GetComponent<Renderer>().material;
                    
                    PhotonNetwork.Destroy(oldBlock);
                    
                    GameObject newBlock = CreateNewBlock(oldTag, oldPosition, oldRotation);
            
                    UpdateBlockMaterial(newBlock, oldMaterial);
                }
            }
            // Clear the list as all blocks have been replaced
            blocksInGate.Clear();

            photonView.RPC("SyncReconnectBlocks", RpcTarget.All);
            
            photonView.RPC("ClearBlocksInGate", RpcTarget.Others);

            if (AreBlocksInTriggers())
            {
                photonView.RPC("UpdateUIState", RpcTarget.All, true);
            }
            else
            {
                photonView.RPC("UpdateUIState", RpcTarget.All, false);
            }

           splitCounter++;
       
           photonView.RPC("UpdateSplitCounter", RpcTarget.All, splitCounter);
        }
    }

    // RPC method to update the split counter
    [PunRPC]
     void UpdateSplitCounter(int counter)
     {
         splitCounter = counter;
         FindObjectOfType<StepsUIManager>().photonView.RPC("UpdateOperationsCounterRPC", RpcTarget.All);
     }


    // Create a new block based on tag
    GameObject CreateNewBlock(string tag, Vector3 position, Quaternion rotation)
    {
        switch (tag)
        {
            case "cubeToy":
                return PhotonNetwork.Instantiate(cubePrefab.name, position, rotation);
            case "triangleToy":
                return PhotonNetwork.Instantiate(trianglePrefab.name, position, rotation);
            case "coneToy":
                return PhotonNetwork.Instantiate(conePrefab.name, position, rotation);
            default:
                return null;
        }
    }

    // Update the material of the new block based on the old material
    void UpdateBlockMaterial(GameObject newBlock, Material oldMaterial)
    {
        if (newBlock != null)
        {
            string materialName = oldMaterial.name.Split(' ')[0];
            PhotonView newBlockPhotonView = newBlock.GetComponent<PhotonView>();
            int newBlockViewID = newBlockPhotonView.ViewID;
            photonView.RPC("UpdateBlockMaterial", RpcTarget.AllBuffered, newBlockViewID, materialName);
        }
    }

    // RPC method to update block material
    [PunRPC]
    void UpdateBlockMaterial(int blockViewID, string materialName)
    {
        PhotonView blockView = PhotonView.Find(blockViewID);
        if (blockView != null)
        {
            Renderer renderer = blockView.gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material newMaterial = Resources.Load<Material>(materialName);
                renderer.material = newMaterial;
            }
            else
            {
                Debug.LogError("Renderer not found on the block object");
            }
        }
        else
        {
            Debug.LogError("PhotonView not found for the given ID");
        }
    }


    [PunRPC]
    void SyncReconnectBlocks()
    {
        StartCoroutine(DelayedReconnect()); 
    }


    IEnumerator DelayedReconnect()
    {

        yield return new WaitForSeconds(0.5f);

        ReconnectWithinTrigger(blocksInFrontTrigger);
        ReconnectWithinTrigger(blocksInBackTrigger);
    }

    [PunRPC]
    void ClearBlocksInGate()
    {
        blocksInGate.Clear();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {
            GameObject parentBlock = other.transform.parent.gameObject;
            if (!blocksInGate.Contains(parentBlock))
            {
                blocksInGate.Add(parentBlock);
                

            }
        }
    }
    private void RequestOwnershipForAllBlocks()
    {
        foreach (GameObject block in blocksInGate)
        {
            RequestOwnership(block);
        }
    }

    private void RequestOwnership(GameObject block)
    {
        PhotonView blockPhotonView = block.GetComponent<PhotonView>();
        if (blockPhotonView != null)
        {
            blockPhotonView.TransferOwnership(PhotonNetwork.MasterClient.ActorNumber);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {
            GameObject parentBlock = other.transform.parent.gameObject;
            if (!blocksInGate.Contains(parentBlock))
            {
                blocksInGate.Add(parentBlock);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "CubeFace")
        {
            GameObject parentBlock = other.transform.parent.gameObject;
            blocksInGate.Remove(parentBlock);

            // Check if there are still blocks in the trigger
            if (!AreBlocksInTriggers())
            {
                photonView.RPC("UpdateUIState", RpcTarget.All, false);
            }
        }
    }

    [PunRPC]
    void UpdateUIState(bool showUI)
    {
        if (isUIActive != showUI)
        {
            isUIActive = showUI;
            if (showUI)
            {
                Invoke("ShowUI", 1.0f);
            }
            else
            {
                HideUI();
            }
        }
    }

    private bool AreBlocksInTriggers()
    {
        return blocksInFrontTrigger.Count > 0 || blocksInBackTrigger.Count > 0;
    }

    private void ShowUI()
    {
        uiPanel.SetActive(true);
        GateLight.SetActive(false);
    }

    private void HideUI()
    {
        uiPanel.SetActive(false);
        GateLight.SetActive(true);
    }

    private void ReconnectWithinTrigger(List<GameObject> blocks)
    {
        blocks.RemoveAll(item => item == null);
        float distanceThreshold = 0.1f;  // Distance Threshold

        // Iterate over the blocks in the same list
        for (int i = 0; i < blocks.Count; i++)
        {
            GameObject block1 = blocks[i];
            List<Vector3> faces1 = block1.GetComponent<BlockController_Photon>().GetAllFacePositions();

            for (int j = i + 1; j < blocks.Count; j++)
            {
                GameObject block2 = blocks[j];
                List<Vector3> faces2 = block2.GetComponent<BlockController_Photon>().GetAllFacePositions();

                foreach (Vector3 face1 in faces1)
                {
                    foreach (Vector3 face2 in faces2)
                    {
                        if (Vector3.Distance(face1, face2) < distanceThreshold)
                        {
                            int viewID1 = block1.GetComponent<PhotonView>().ViewID;
                            int viewID2 = block2.GetComponent<PhotonView>().ViewID;

                            block1.GetComponent<PhotonView>().RPC("ReAttachBlocks", RpcTarget.All, face1, face2, viewID2);
                            block2.GetComponent<PhotonView>().RPC("ReAttachBlocks", RpcTarget.All, face2, face1, viewID1);
                        }
                    }
                }
            }
        }
    }

}
