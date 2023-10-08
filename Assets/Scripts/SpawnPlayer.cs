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
                PhotonNetwork.Instantiate(playerPrefab1.name, spawnPosition1.position, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(playerPrefab2.name, spawnPosition2.position, Quaternion.identity);
            }
        }
    }
}

    

    

