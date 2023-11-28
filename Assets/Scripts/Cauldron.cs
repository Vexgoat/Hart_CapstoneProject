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

    //This method checks if the player is holding right click down, then it compares if cauldron and grab tags and if thats true it will destroy the object "eye" and instatiate a key.
    private void Update()
    {

        if (!Input.GetMouseButton(1))
        {
    
            if (gameObject.CompareTag("Cauldron"))
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(interactionPoint.position, interactionRadius);
                
                foreach (Collider2D collider in hitColliders)
                {
                    if (collider.CompareTag("grab") && collider.gameObject.GetPhotonView().IsMine)
                    {
                        PhotonNetwork.Destroy(collider.gameObject);            
                        Chest.key = PhotonNetwork.Instantiate("Key", keySpawn, Quaternion.identity);                                     
                        GetComponent<Collider2D>().enabled = false;
                        break;
                    }
                }
            }
        }
    }
}
