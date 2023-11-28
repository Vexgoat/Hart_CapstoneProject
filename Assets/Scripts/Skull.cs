using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Skull : MonoBehaviourPunCallbacks
{

    public GameObject player1;
    public GameObject player2;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    //This method will check for both player tags then load then call the method loadnextscene
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Player2"))
        {
            // Call the PunRPC method to load the scene across all clients
            view.RPC("LoadNextScene", RpcTarget.AllBuffered);
        }
    }

    //This method will load the next scene in the build index for all players across the network
    [PunRPC]
    private void LoadNextScene()
    {
        // Load the next scene
        if(PhotonNetwork.IsMasterClient){
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PhotonNetwork.LoadLevel(currentSceneIndex + 1);
        }
    }
}

