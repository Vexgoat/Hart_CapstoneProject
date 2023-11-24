using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class Key : MonoBehaviourPunCallbacks
{

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the key is the player and if the 'view' is not null
        if ((other.CompareTag("Player") || other.CompareTag("Player2")) && view != null && view.IsMine)
        {
            Debug.Log("Collided");
            // Call the RPC to destroy the key object for all players
            view.RPC("DestroyKey", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DestroyKey()
    {
        // Check if 'view' is not null before accessing its properties
        if (view != null && view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            Chest.key = null;
        }
    }
}