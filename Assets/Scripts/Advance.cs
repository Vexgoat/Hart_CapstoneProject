using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Advance : MonoBehaviourPunCallbacks
{
    public GameObject keyPrefab;

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Logic
    }
}
