using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviourPunCallbacks
{
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;

    public Transform spawnPosition1;
    public Transform spawnPosition2;

    void Start()
    {

        PhotonNetwork.AutomaticallySyncScene = true;

        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Master client - Spawning player 1");
                PhotonNetwork.Instantiate(playerPrefab1.name, spawnPosition1.position, Quaternion.identity);
            }
            else
            {
                Debug.Log("Not master client - Spawning player 2");
                PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition2.position, Quaternion.identity);
            }
        }
    }
}

