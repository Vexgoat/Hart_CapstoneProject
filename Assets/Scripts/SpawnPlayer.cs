using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;

    public Transform spawnPosition1;
    public Transform spawnPosition2;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // Instantiate the player prefab for player 1 using the local player's PhotonView
                GameObject player1 = PhotonNetwork.Instantiate(playerPrefab1.name, spawnPosition1.position, Quaternion.identity);
                
                // Transfer ownership to the local player
                player1.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            else
            {
                // Instantiate the player prefab for player 2
                PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition2.position, Quaternion.identity);
            }
        }
    }
}
