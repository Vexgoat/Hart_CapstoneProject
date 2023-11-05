using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        
        // Check if the object colliding with the key is the player
        if (other.CompareTag("Player") && view.IsMine)
        {
            Debug.Log("Collided");
            // Call the RPC to destroy the key object for all players
            photonView.RPC("DestroyKey", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DestroyKey()
    {
        // Ensure that this is executed only on the owner client
        if (view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            Chest.key = null;
            
        }
    }
}