using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Key : MonoBehaviourPunCallbacks
{
    public AudioClip keyPickupSound;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    //This method checks for both player1 and player2 tags, and checks the views before calling the methods below
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if ((other.CompareTag("Player") || other.CompareTag("Player2")) && view != null && view.IsMine)
        {
            view.RPC("KeyPickupSound", RpcTarget.All);
            view.RPC("DestroyKey", RpcTarget.All);
        }
    }

    //This will play the key pickup sound
    [PunRPC]
    private void KeyPickupSound()
    {
        AudioSource.PlayClipAtPoint(keyPickupSound, transform.position);
    }

    //This will check the view before destroy the key to make sure the person who owns it destroys it
    [PunRPC]
    public void DestroyKey()
    {
        if (view != null && view.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
            Chest.key = null;
        }
    }
}
