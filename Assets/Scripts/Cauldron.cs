using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Cauldron : MonoBehaviourPunCallbacks
{
    public float interactionRadius = 1.0f;
    public GameObject keyPrefab;
    public Vector2 keySpawn = new Vector2(1f, 1f);
    public Transform interactionPoint; 

    PhotonView view;

    private void Start()
    {
        view = GetComponent<PhotonView>();
    }

    private void Update()
    {
        // Check if the player is not holding down right click
        if (!Input.GetMouseButton(1))
        {
            // Check if the cauldron has the specific tag
            if (gameObject.CompareTag("Cauldron"))
            {
                // Get all colliders within the interaction radius of the cauldron at the specific interaction point
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius);

                // Iterate through all the colliders found
                foreach (Collider2D collider in hitColliders)
                {
                    // Check if the collider has the "grab" tag and is owned by the local player
                    if (collider.CompareTag("grab") && collider.gameObject.GetPhotonView().IsMine)
                    {
                        // Destroy the object with the "grab" tag
                        PhotonNetwork.Destroy(collider.gameObject);

                        // Instantiate the prefab
                        Chest.key = PhotonNetwork.Instantiate("Key", keySpawn, Quaternion.identity);

                        //Chest.key.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer);

                        // Turn off the collider of the cauldron
                        GetComponent<Collider2D>().enabled = false;

                        // Break out of the loop if you only want to interact with one object at a time
                        break;
                    }
                }
            }
        }
    }
}
