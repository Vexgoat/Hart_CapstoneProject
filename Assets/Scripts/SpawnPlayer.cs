using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public Transform spawnPosition1;
    public Transform spawnPosition2;

    //This method will sync the scene that player1 is one across the network for other local players(player2)
    //It will also spawn in player1 first and then player2 keeping them on the master and local cilents
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(playerPrefab1.name, spawnPosition1.position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition2.position, Quaternion.identity);
            }
        }
    }
}

